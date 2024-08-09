// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

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
    public SqliteStorage? Storage { get; set; }

    /// <summary>
    /// 分布式雪花Id
    /// </summary>
    public Snowflake? Snowflake { get; set; }
}
