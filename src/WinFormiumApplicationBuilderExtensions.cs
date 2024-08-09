// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunet.WinFormium.Core;
using Yitter.IdGenerator;
using SqlSugar;

/// <summary>
/// WinFormiumApplicationBuilder扩展
/// </summary>
public static class WinFormiumApplicationBuilderExtensions
{
    /// <summary>
    /// 添加WinFormium
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="setupOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddWinFormium<T>(this IServiceCollection services, Action<StartupOptions> setupOptions) where T : Form
    {
        var startupOptions = new StartupOptions();

        setupOptions?.Invoke(startupOptions);

        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true);

        var Configuration = config.Build();

        services.AddSingleton(Configuration);

        services.AddHttpClient("default", client =>
        {
            if (startupOptions != null && startupOptions.Headers != null)
            {
                foreach (var item in startupOptions.Headers)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
        });

        if (startupOptions != null && startupOptions.Storage != null)
        {
            services.AddSingleton<ISqlSugarClient>(db =>
            {
                var assemblyName = startupOptions.GetType().Assembly.GetName().Name!;
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var dbPath = Path.Combine(appDataPath, assemblyName, "data", startupOptions.Storage.DataVersion ?? string.Empty, $"{startupOptions.Storage.DbName}.db");
                return new SqlSugarScope(new ConnectionConfig
                {
                    DbType = DbType.Sqlite,
                    ConnectionString = $"Data Source={dbPath};",
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsAutoRemoveDataCache = true
                    }
                });
            });

            var serviceProvider = services.BuildServiceProvider();

            var db = serviceProvider.GetRequiredService<ISqlSugarClient>();

            db.DbMaintenance.CreateDatabase();

            db.CodeFirst.InitTables(startupOptions.Storage.EntityTypes);
        }

        if (startupOptions != null && startupOptions.Snowflake != null)
        {
            var options = new IdGeneratorOptions(startupOptions.Snowflake.WorkerId);

            YitIdHelper.SetIdGenerator(options);
        }

        var winFormiumOpts = new WinFormiumOptions(services);

        var createAction = winFormiumOpts.UseWinFormium<T>();

        if (createAction != null)
        {
            services.AddSingleton(winFormiumOpts);

            services.AddSingleton(createAction);
        }

        var forms = typeof(T).Assembly.GetTypes().Where(x => x != typeof(T) && (x.BaseType == typeof(BaseForm) || x.BaseType == typeof(Form)));

        foreach (var form in forms)
        {
            services.AddSingleton(form);
        }

        return services;
    }
}
