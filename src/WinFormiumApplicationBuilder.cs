// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// WinFormiumApplicationBuilder
/// </summary>
public sealed class WinFormiumApplicationBuilder
{
    /// <summary>
    /// Services
    /// </summary>
    public IServiceCollection Services { get; }

    internal WinFormiumApplicationBuilder()
    {
        Services = new ServiceCollection();
    }

    /// <summary>
    /// Build
    /// </summary>
    /// <returns></returns>
    public WinFormiumApplication Build()
    {
        Services.AddSingleton(provider =>
        {
            return new WinFormiumApplication(Services);
        });

        return Services.BuildServiceProvider().GetRequiredService<WinFormiumApplication>();
    }
}
