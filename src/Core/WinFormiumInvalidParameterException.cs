// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium.Core;

/// <summary>
/// 无效的参数异常
/// </summary>
public partial class WinFormiumInvalidParameterException : Exception
{
    /// <summary>
    /// 无效的参数异常
    /// </summary>
    public WinFormiumInvalidParameterException() { }

    /// <summary>
    /// 无效的参数异常
    /// </summary>
    /// <param name="message">消息</param>
    public WinFormiumInvalidParameterException(string? message) : base(message) { }

    /// <summary>
    /// 消息
    /// </summary>
    public override string Message => "无效的参数";
}
