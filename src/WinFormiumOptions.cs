// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// WinFormiumOptions
/// </summary>
public sealed class WinFormiumOptions
{
    internal ApplicationContext Context { get; set; } = new ApplicationContext();

    private IServiceCollection Services { get; }

    internal WinFormiumOptions(IServiceCollection services)
    {
        Services = services;
    }

    /// <summary>
    /// UseWinFormium
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public WinFormiumCreationAction UseWinFormium<T>() where T : Form
    {
        Services.AddSingleton<T>();

        return new WinFormiumCreationAction(provider =>
        {
            var form = provider.GetRequiredService<T>();

            Context.MainForm = form;
        });
    }
}
