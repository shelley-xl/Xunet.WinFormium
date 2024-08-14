// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium;

using Microsoft.Extensions.DependencyInjection;
using Xunet.WinFormium.Core;

/// <summary>
/// WinFormiumApplication扩展
/// </summary>
public static class WinFormiumApplicationExtensions
{
    static PropertyManager? Properties { get; set; }

    /// <summary>
    /// 使用WinFormium
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WinFormiumApplication UseWinFormium(this WinFormiumApplication app)
    {
        Properties = app.Services.GetRequiredService<PropertyManager>();

        Properties?.SetValue(nameof(UseWinFormium), true);

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
        Properties?.SetValue(nameof(UseMutex), true);

        return app;
    }

    /// <summary>
    /// 使用WebApi
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WinFormiumApplication UseWebApi(this WinFormiumApplication app)
    {
        Properties?.SetValue(nameof(UseWebApi), true);

        return app;
    }
}
