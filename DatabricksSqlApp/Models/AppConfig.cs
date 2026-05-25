namespace DatabricksSqlApp.Models;

public class AppConfig
{
    public string DefaultQuery { get; set; } = string.Empty;
    public List<ClientConfig> Clients { get; set; } = new();
}
