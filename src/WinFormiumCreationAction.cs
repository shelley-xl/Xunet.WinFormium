// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium;

/// <summary>
/// WinFormiumCreationAction
/// </summary>
/// <param name="createAction"></param>
public sealed class WinFormiumCreationAction(Action<IServiceProvider> createAction) : IDisposable
{
    internal Action<IServiceProvider> CreateAction { get; } = createAction;

    internal void Invoke(IServiceProvider services)
    {
        CreateAction.Invoke(services);
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {

    }
}
