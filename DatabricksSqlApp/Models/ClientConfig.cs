namespace DatabricksSqlApp.Models;

public class ClientConfig
{
    public string Name { get; set; } = string.Empty;
    public DatabricksConfig Databricks { get; set; } = new();

    public override string ToString() => Name;
}
