using DatabricksSqlApp.Forms;
using DatabricksSqlApp.Models;
using DatabricksSqlApp.Services;
using Microsoft.Extensions.Configuration;

namespace DatabricksSqlApp;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        AppConfig appConfig;
        try
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            appConfig = new AppConfig();
            configuration.Bind(appConfig);

            if (appConfig.Clients == null || appConfig.Clients.Count == 0)
                throw new InvalidOperationException("No clients configured in appsettings.json.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Failed to load configuration:\n\n{ex.Message}",
                "Configuration error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        var service = new DatabricksSqlService();
        Application.Run(new MainForm(appConfig, service));
    }
}
