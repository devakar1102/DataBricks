using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DatabricksSqlApp.Models;

namespace DatabricksSqlApp.Services;

public class DatabricksSqlService
{
    private static readonly HttpClient HttpClient = new();

    public async Task<DataTable> ExecuteSqlAsync(DatabricksConfig cfg, string sql, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(cfg.Host) || string.IsNullOrWhiteSpace(cfg.HttpPath) || string.IsNullOrWhiteSpace(cfg.AccessToken))
            throw new InvalidOperationException("Missing required configuration (Host, HttpPath, AccessToken).");

        var host = cfg.Host.TrimEnd('/');
        var httpPath = cfg.HttpPath;
        var token = cfg.AccessToken;

        var submitUrl = $"{host}/api/2.0/sql/statements";

        var warehouseId = httpPath.Contains("/sql/1.0/warehouses/")
            ? httpPath.Split('/', StringSplitOptions.RemoveEmptyEntries).LastOrDefault()
            : null;

        using var submitRequest = new HttpRequestMessage(HttpMethod.Post, submitUrl)
        {
            Content = new StringContent(
                JsonSerializer.Serialize(new { statement = sql, warehouse_id = warehouseId }),
                Encoding.UTF8,
                "application/json")
        };
        submitRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (httpPath.StartsWith("/sql/1.0/warehouses/"))
            submitRequest.Headers.Add("X-Databricks-HTTP-Path", httpPath);

        var submitResponse = await HttpClient.SendAsync(submitRequest, ct);
        submitResponse.EnsureSuccessStatusCode();

        var submitJson = JsonSerializer.Deserialize<JsonElement>(await submitResponse.Content.ReadAsStringAsync(ct));
        var statementId = submitJson.GetProperty("statement_id").GetString();
        if (string.IsNullOrEmpty(statementId))
            throw new Exception("Failed to get statement ID");

        var statusUrl = $"{host}/api/2.0/sql/statements/{statementId}";
        JsonElement statusResult = default;

        for (int i = 0; i < 60; i++)
        {
            ct.ThrowIfCancellationRequested();
            await Task.Delay(5000, ct);

            using var statusRequest = new HttpRequestMessage(HttpMethod.Get, statusUrl);
            statusRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var statusResponse = await HttpClient.SendAsync(statusRequest, ct);
            statusResponse.EnsureSuccessStatusCode();

            statusResult = JsonSerializer.Deserialize<JsonElement>(await statusResponse.Content.ReadAsStringAsync(ct));
            var state = statusResult.GetProperty("status").GetProperty("state").GetString();

            if (state == "SUCCEEDED") break;
            if (state == "FAILED" || state == "CANCELED")
                throw new Exception($"Query {state.ToLower()}");
        }

        if (statusResult.ValueKind == JsonValueKind.Undefined)
            throw new Exception("Query did not complete within timeout");

        if (!statusResult.TryGetProperty("result", out var result))
        {
            Debug.WriteLine("DEBUG: Response JSON:");
            Debug.WriteLine(JsonSerializer.Serialize(statusResult, new JsonSerializerOptions { WriteIndented = true }));
            throw new Exception("Response does not contain 'result' property");
        }

        var dataTable = new DataTable();

        if (statusResult.TryGetProperty("manifest", out var manifest) && manifest.TryGetProperty("schema", out var schema))
        {
            JsonElement columnsElement = schema.ValueKind == JsonValueKind.Array
                ? schema
                : (schema.TryGetProperty("columns", out var cols) ? cols : default);

            if (columnsElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var col in columnsElement.EnumerateArray())
                {
                    var colName = col.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null;
                    dataTable.Columns.Add(colName ?? "Column", typeof(string));
                }
            }
        }

        if (dataTable.Columns.Count == 0
            && result.TryGetProperty("data_array", out var tempArray)
            && tempArray.ValueKind == JsonValueKind.Array
            && tempArray.GetArrayLength() > 0)
        {
            var firstRow = tempArray[0];
            int idx = 0;
            foreach (var _ in firstRow.EnumerateArray())
            {
                dataTable.Columns.Add($"Column{++idx}", typeof(string));
            }
        }

        if (result.TryGetProperty("data_array", out var dataArray) && dataArray.ValueKind == JsonValueKind.Array)
        {
            foreach (var row in dataArray.EnumerateArray())
            {
                if (row.ValueKind != JsonValueKind.Array) continue;

                var dataRow = dataTable.NewRow();
                int colIndex = 0;
                foreach (var cell in row.EnumerateArray())
                {
                    if (colIndex < dataTable.Columns.Count)
                    {
                        if (cell.ValueKind == JsonValueKind.Null)
                            dataRow[colIndex] = DBNull.Value;
                        else if (cell.ValueKind == JsonValueKind.String)
                            dataRow[colIndex] = cell.GetString() ?? "";
                        else
                            dataRow[colIndex] = cell.ToString();
                    }
                    colIndex++;
                }
                dataTable.Rows.Add(dataRow);
            }
        }

        return dataTable;
    }
}
