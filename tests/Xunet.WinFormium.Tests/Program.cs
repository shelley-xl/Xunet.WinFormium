using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using Xunet.WinFormium;
using Xunet.WinFormium.Core;
using Xunet.WinFormium.Tests;
using Xunet.WinFormium.Tests.Models;

// To customize application configuration such as set high DPI settings or default font,
// see https://aka.ms/applicationconfiguration.
ApplicationConfiguration.Initialize();

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
        DbName = "Xunet.WinFormium.Tests",
        EntityTypes = [typeof(CnBlogsModel)]
    };
    options.Snowflake = new()
    {
        WorkerId = 1
    };
});

builder.Services.AddWebApi((provider, services) =>
{
    services.AddSingleton(provider.GetRequiredService<ISqlSugarClient>());
});

var app = builder.Build();

app.UseWinFormium();

app.UseMutex();

app.UseWebApi();

app.Run();
