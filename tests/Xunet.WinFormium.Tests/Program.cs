namespace Xunet.WinFormium.Tests;

using Xunet.WinFormium.Core;
using Xunet.WinFormium.Tests.Models;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        ServiceConfiguration.Initialize(new StartupOptions
        {
            Headers = new()
            {
                {
                    HeaderNames.UserAgent,
                    "Mozilla/5.0 (iPhone; CPU iPhone OS 6_1_3 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Mobile/10B329 MicroMessenger/5.0.1"
                }
            },
            Storage = new()
            {
                StorageName = "Xunet.WinFormium.Tests",
                EntityTypes = [typeof(TestModel)]
            },
            Generator = new()
            {
                WorkerId = 1
            }
        });

        Application.Run(new MainForm());
    }
}
