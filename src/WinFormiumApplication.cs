﻿// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium;

using Microsoft.Extensions.DependencyInjection;

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
    /// 是否使用互斥锁
    /// </summary>
    internal bool IsUseMutex { get; set; }

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
    /// Run
    /// </summary>
    public void Run()
    {
        using var mutex = Services.GetRequiredService<Mutex>();

        if (IsUseMutex)
        {
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("已经有一个正在运行的程序，请勿重复运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        var createMainWindowAction = Services.GetRequiredService<WinFormiumCreationAction>();

        var mainWindowOptions = Services.GetRequiredService<WinFormiumOptions>();

        createMainWindowAction.Invoke(Services);

        createMainWindowAction.Dispose();

        Application.Run(mainWindowOptions.Context);

        if (IsUseMutex)
        {
            mutex?.ReleaseMutex();
        }
    }
}
