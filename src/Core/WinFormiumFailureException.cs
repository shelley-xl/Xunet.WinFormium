// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium.Core;

/// <summary>
/// 操作失败异常
/// </summary>
public class WinFormiumFailureException : Exception
{
    /// <summary>
    /// 操作失败异常
    /// </summary>
    public WinFormiumFailureException() { }

    /// <summary>
    /// 操作失败异常
    /// </summary>
    /// <param name="message">消息</param>
    public WinFormiumFailureException(string? message) : base(message) { }

    /// <summary>
    /// 消息
    /// </summary>
    public override string Message => "操作失败";
}
