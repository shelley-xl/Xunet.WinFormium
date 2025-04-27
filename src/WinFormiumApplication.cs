// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunet.WinFormium.Core;

/// <summary>
/// WinFormiumApplication
/// </summary>
public class WinFormiumApplication
{
    /// <summary>
    /// Current
    /// </summary>
    public static WinFormiumApplication? Current { get; private set; }

    /// <summary>
    /// Services
    /// </summary>
    public IServiceProvider Services { get; private set; }

    /// <summary>
    /// CreateBuilder
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static WinFormiumApplicationBuilder CreateBuilder()
    {
        if (Current != null)
        {
            throw new ApplicationException("已经初始化，只允许运行一个实例。");
        }

        return new WinFormiumApplicationBuilder();
    }

    internal WinFormiumApplication(IServiceCollection services)
    {
        if (Current != null)
        {
            throw new ApplicationException("已经初始化，只允许运行一个实例。");
        }

        Current = this;

        Services = services.BuildServiceProvider();
    }

    /// <summary>
    /// 运行
    /// </summary>
    public void Run()
    {
        var Properties = Services.GetRequiredService<PropertyManager>();
        var UseWinFormium = Properties.GetValue<bool>(nameof(WinFormiumApplicationExtensions.UseWinFormium));
        var UseSingleApp = Properties.GetValue<bool>(nameof(WinFormiumApplicationExtensions.UseSingleApp));
        var UseWebApi = Properties.GetValue<bool>(nameof(WinFormiumApplicationExtensions.UseWebApi));

        // 使用单例应用
        using var mutex = Services.GetRequiredService<Mutex>();
        if (UseSingleApp)
        {
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("已经有一个正在运行的程序，请勿重复运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        // 使用WebApi
        if (UseWebApi)
        {
            var app = Services.GetRequiredService<WebApplication>();

            app.RunAsync();
        }

        // 使用WinFormium
        if (UseWinFormium)
        {
            var createMainWindowAction = Services.GetRequiredService<WinFormiumCreationAction>();

            var mainWindowOptions = Services.GetRequiredService<WinFormiumOptions>();

            createMainWindowAction.Invoke(Services);

            createMainWindowAction.Dispose();

            Application.Run(mainWindowOptions.Context);
        }

        if (UseSingleApp)
        {
            mutex?.ReleaseMutex();
        }
    }
}
