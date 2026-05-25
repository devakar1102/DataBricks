using DatabricksSqlApp.Models;
using DatabricksSqlApp.Services;

namespace DatabricksSqlApp.Forms;

public partial class MainForm : Form
{
    private readonly AppConfig _config;
    private readonly DatabricksSqlService _service;

    public MainForm(AppConfig config, DatabricksSqlService service)
    {
        _config = config;
        _service = service;

        InitializeComponent();

        cboClient.DisplayMember = nameof(ClientConfig.Name);
        cboClient.DataSource = _config.Clients;
        cboClient.SelectedIndex = -1;

        txtSql.Text = _config.DefaultQuery;
    }

    private void cboClient_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cboClient.SelectedItem is not ClientConfig client)
        {
            ClearSetup();
            btnQuery.Enabled = false;
            return;
        }

        txtHost.Text = client.Databricks.Host;
        txtHttpPath.Text = client.Databricks.HttpPath;
        txtPat.Text = client.Databricks.AccessToken;
        btnQuery.Enabled = true;
    }

    private async void btnQuery_Click(object? sender, EventArgs e)
    {
        if (cboClient.SelectedItem is not ClientConfig client)
            return;

        var sql = string.IsNullOrWhiteSpace(txtSql.Text) ? _config.DefaultQuery : txtSql.Text;

        btnQuery.Enabled = false;
        cboClient.Enabled = false;
        lblStatus.Text = "Running...";
        dgvResults.DataSource = null;
        UseWaitCursor = true;

        try
        {
            var table = await _service.ExecuteSqlAsync(client.Databricks, sql);

            dgvResults.DataSource = table;
            lblStatus.Text = $"{table.Rows.Count} row(s), {table.Columns.Count} column(s)";

            if (table.Columns.Count == 0)
                MessageBox.Show(this, "No columns found in result.", "Query", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (table.Rows.Count == 0)
                MessageBox.Show(this, "No rows returned.", "Query", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Query failed";
            MessageBox.Show(this, ex.Message, "Query failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            UseWaitCursor = false;
            cboClient.Enabled = true;
            btnQuery.Enabled = cboClient.SelectedItem is ClientConfig;
        }
    }

    private void ClearSetup()
    {
        txtHost.Text = string.Empty;
        txtHttpPath.Text = string.Empty;
        txtPat.Text = string.Empty;
    }
}
