// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium.Core;

/// <summary>
/// 分布式雪花Id
/// </summary>
public class Snowflake
{
    /// <summary>
    /// 唯一工作机器Id
    /// </summary>
    public ushort WorkerId { get; set; } = 1;
}
