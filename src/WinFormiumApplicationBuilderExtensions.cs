// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunet.WinFormium.Core;
using SqlSugar;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Xunet.WinFormium.Dtos;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Unicode;
using Xunet.WinFormium.Http.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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
    /// <returns></returns>
    public static IServiceCollection AddWinFormium<T>(this IServiceCollection services) where T : Form
    {
        services.AddSingleton(new Mutex(false, typeof(T).Assembly.GetName().Name));

        services.AddSingleton<PropertyManager>();

        var winFormiumOpts = new WinFormiumOptions(services);

        var createAction = winFormiumOpts.UseWinFormium<T>();

        if (createAction != null)
        {
            services.AddSingleton(winFormiumOpts);

            services.AddSingleton(createAction);
        }

        var forms = typeof(T).Assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IMiniFormium)) && x.IsClass).ToArray();

        foreach (var form in forms)
        {
            services.AddSingleton(form);
        }

        return services;
    }

    /// <summary>
    /// 添加WinFormium
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="setupOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddWinFormium<T>(this IServiceCollection services, Action<StartupOptions> setupOptions) where T : Form, IMiniFormium
    {
        var startupOptions = new StartupOptions();

        setupOptions?.Invoke(startupOptions);

        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true);

        var Configuration = config.Build();

        services.AddSingleton(Configuration);

        if (startupOptions != null && startupOptions.Headers != null)
        {
            services.AddHttpClient("default", client =>
            {
                foreach (var item in startupOptions.Headers)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            });
        }

        if (startupOptions != null && startupOptions.Storage != null)
        {
            services.AddSingleton<ISqlSugarClient>(db =>
            {
                var assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "Xunet.WinFormium";
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
                        IsAutoRemoveDataCache = true,
                        SqliteCodeFirstEnableDefaultValue = true,
                        SqliteCodeFirstEnableDescription = true,
                        SqliteCodeFirstEnableDropColumn = true,
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
            SnowFlakeSingle.WorkId = startupOptions.Snowflake.WorkerId;
            SnowFlakeSingle.DatacenterId = startupOptions.Snowflake.DataCenterId;
        }

        services.AddWinFormium<T>();

        return services;
    }

    /// <summary>
    /// 添加WebApi
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <param name="setupAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddWebApi(this IServiceCollection services, Assembly assembly, Action<ServiceProvider, IServiceCollection> setupAction)
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            // 不区分大小写
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            // 忽略循环引用
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            // 使用小驼峰命名
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            // 不使用 Unicode 编码
            options.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            // 使用缩进格式
            options.SerializerOptions.WriteIndented = true;
            // 自定义时间格式
            options.SerializerOptions.Converters.Add(new DateTimeJsonConverter());
        });
        builder.Services.AddHealthChecks();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "欢迎使用 API 接口服务",
                Description = "欢迎使用 API 接口服务",
                Version = $"v{assembly.GetName().Version}"
            });
            var xmlPath1 = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            var xmlPath2 = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
            if (File.Exists(xmlPath1)) x.IncludeXmlComments(xmlPath1, true);
            if (File.Exists(xmlPath2)) x.IncludeXmlComments(xmlPath2, true);
        });

        setupAction.Invoke(services.BuildServiceProvider(), builder.Services);

        var app = builder.Build();

        app.UseHealthChecks("/health/check", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = (context, report) =>
            {
                return context.Response.WriteAsJsonAsync(new OperateResultDto
                {
                    Code = ResultCode.Success,
                    Message = report.Status.ToString(),
                });
            }
        });
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI(x =>
        {
            // 解决urls.primaryName无法重定向问题
            x.ConfigObject.AdditionalItems["queryConfigEnabled"] = true;
            x.RoutePrefix = string.Empty;
            x.DocumentTitle = "欢迎使用 API 接口服务";
            x.ShowExtensions();
            x.EnableValidator();
            // 设置隐藏models
            x.DefaultModelsExpandDepth(-1);
            x.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
        });
        app.Use(async (context, next) =>
        {
            try
            {
                await next();

                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    throw new WinFormiumNotFoundException();
                }
            }
            catch (WinFormiumNotFoundException ex)
            {
                await context.Response.WriteAsJsonAsync(new OperateResultDto
                {
                    Code = ResultCode.NotFound,
                    Message = ex.Message
                });
            }
            catch (WinFormiumFailureException ex)
            {
                await context.Response.WriteAsJsonAsync(new OperateResultDto
                {
                    Code = ResultCode.Failure,
                    Message = ex.Message
                });
            }
            catch (WinFormiumInvalidParameterException ex)
            {
                await context.Response.WriteAsJsonAsync(new OperateResultDto
                {
                    Code = ResultCode.InvalidParameter,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsJsonAsync(new OperateResultDto
                {
                    Code = ResultCode.SystemException,
                    Message = ex.ToString()
                });
            }
        });

        services.AddSingleton(app);

        return services;
    }
}
