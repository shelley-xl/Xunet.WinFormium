namespace Xunet.WinFormium.Core;

/// <summary>
/// 启动项
/// </summary>
public class StartupOptions
{
    /// <summary>
    /// 请求头
    /// </summary>
    public RequestHeaders? Headers { get; set; }

    /// <summary>
    /// 数据存储
    /// </summary>
    public Storageable? Storage { get; set; }

    /// <summary>
    /// 分布式雪花Id
    /// </summary>
    public IdGenerator? Generator { get; set; }
}
