// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium;

using Microsoft.Extensions.DependencyInjection;
using Xunet.FluentScheduler;
using Xunet.WinFormium.Core;

/// <summary>
/// WinFormiumApplication扩展
/// </summary>
public static class WinFormiumApplicationExtensions
{
    /// <summary>
    /// 使用WinFormium
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WinFormiumApplication UseWinFormium(this WinFormiumApplication app)
    {
        JobManager.Initialize();

        DependencyResolver.Initialize(app.Services);

        return app;
    }

    /// <summary>
    /// 使用互斥锁
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WinFormiumApplication UseMutex(this WinFormiumApplication app)
    {
        app.IsUseMutex = true;

        return app;
    }
}
