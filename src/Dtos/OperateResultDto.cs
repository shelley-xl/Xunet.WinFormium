// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium.Dtos;

using System.ComponentModel;

/// <summary>
/// 操作响应
/// </summary>
public class OperateResultDto
{
    /// <summary>
    /// 状态码
    /// </summary>
    public StatusCodes? Code { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string? Message { get; set; }
}

/// <summary>
/// 查询响应
/// </summary>
/// <typeparam name="T">泛型参数</typeparam>
public class OperateResultDto<T> : OperateResultDto
{
    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }
}

/// <summary>
/// 查询响应
/// </summary>
/// <typeparam name="T">泛型参数</typeparam>
public class PageResultDto<T> : OperateResultDto<T>
{
    /// <summary>
    /// 总记录数
    /// </summary>
    public int? Total { get; set; }
}

/// <summary>
/// 状态码
/// </summary>
public enum StatusCodes
{
    /// <summary>
    /// 成功
    /// </summary>
    [Description("成功")]
    Success = 0,

    /// <summary>
    /// 失败
    /// </summary>
    [Description("失败")]
    Failure = 1,

    /// <summary>
    /// 无效的参数
    /// </summary>
    [Description("无效的参数")]
    InvalidParameter = 2,

    /// <summary>
    /// 系统异常
    /// </summary>
    [Description("系统异常")]
    SystemException = 3
}
