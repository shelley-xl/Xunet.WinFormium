namespace Xunet.WinFormium.Core;

/// <summary>
/// 信号量
/// </summary>
public sealed class AsyncSemaphore
{
    /// <summary>
    /// 信号量释放
    /// </summary>
    /// <param name="semaphore"></param>
    private class SemaphoreReleaser(SemaphoreSlim semaphore) : IDisposable
    {
        readonly SemaphoreSlim _semaphore = semaphore;

        public void Dispose() => _semaphore.Release();
    }

    readonly SemaphoreSlim _semaphore;

    /// <summary>
    /// 构造函数
    /// </summary>
    public AsyncSemaphore() => _semaphore = new SemaphoreSlim(1);

    /// <summary>
    /// 等待
    /// </summary>
    /// <returns></returns>
    public async Task<IDisposable> WaitAsync()
    {
        await _semaphore.WaitAsync();

        return new SemaphoreReleaser(_semaphore);
    }
}
