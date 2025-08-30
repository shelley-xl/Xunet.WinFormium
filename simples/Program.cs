internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        var builder = WinFormiumApplication.CreateBuilder();

        builder.Services.AddWinFormium<MainForm>(options =>
        {
            options.Headers = new()
            {
                {
                    HeaderNames.UserAgent,
                    "Mozilla/5.0 (iPhone; CPU iPhone OS 6_1_3 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Mobile/10B329 MicroMessenger/5.0.1"
                }
            };
            options.Storage = new()
            {
                DataVersion = "24.8.9.1822",
                DbName = "Xunet.WinFormium.Simples",
                EntityTypes = [typeof(CnBlogsModel)]
            };
            options.Snowflake = new()
            {
                WorkerId = 1
            };
        });

        builder.Services.AddWebApi(Assembly.GetExecutingAssembly(), (provider, services) =>
        {
            var db = provider.GetRequiredService<ISqlSugarClient>();

            services.AddSingleton(db);
        });

        var app = builder.Build();

        app.UseWinFormium();

        app.UseSingleApp();

        app.UseWebApi();

        app.Run();
    }
}
