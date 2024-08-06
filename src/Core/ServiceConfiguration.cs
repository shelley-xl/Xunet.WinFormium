namespace Xunet.WinFormium.Core;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using Yitter.IdGenerator;

/// <summary>
/// 服务注册配置
/// </summary>
public static class ServiceConfiguration
{
    /// <summary>
    /// 初始化
    /// </summary>
    public static void Initialize(StartupOptions? startupOptions = null)
    {
        var services = new ServiceCollection();

        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true);

        var Configuration = builder.Build();

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
                var dbVersion = "24.8.6.1140";
                var assemblyName = startupOptions.GetType().Assembly.GetName().Name!;
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var dbPath = Path.Combine(appDataPath, assemblyName, "data", dbVersion, $"{startupOptions.Storage.StorageName}.db");
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

        DependencyResolver.Initialize(services.BuildServiceProvider());

        if (startupOptions != null && startupOptions.Generator != null)
        {
            var options = new IdGeneratorOptions(startupOptions.Generator.WorkerId);

            YitIdHelper.SetIdGenerator(options);
        }
    }
}
