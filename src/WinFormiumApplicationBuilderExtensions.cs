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
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Xunet.WinFormium.Dtos;

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

        Type[] types = [typeof(BaseForm), typeof(Form)];

        var forms = typeof(T).Assembly.GetTypes().Where(x => x.Name != typeof(T).Name && types.Contains(x.BaseType)).ToList();

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
    public static IServiceCollection AddWinFormium<T>(this IServiceCollection services, Action<StartupOptions> setupOptions) where T : BaseForm
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

        services.AddWinFormium<T>();

        return services;
    }

    /// <summary>
    /// 添加WebApi
    /// </summary>
    /// <param name="services"></param>
    /// <param name="setupAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddWebApi(this IServiceCollection services, Action<ServiceProvider, IServiceCollection> setupAction)
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddControllers().AddNewtonsoftJson(JsonSettings.NewtonsoftJsonOptions());
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "欢迎使用 API 接口服务",
                Description = "欢迎使用 API 接口服务",
                Version = $"v{Assembly.GetEntryAssembly()?.GetName().Version}"
            });
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml"), true);
        });

        setupAction.Invoke(services.BuildServiceProvider(), builder.Services);

        var app = builder.Build();

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
