// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunet.WinFormium.Dtos;

/// <summary>
/// 控制器基类
/// </summary>
public class BaseController : ControllerBase
{
    /// <summary>
    /// 公共查询返回
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <param name="total"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    [NonAction]
    public virtual IResult XunetResult<TValue>(TValue value, int? total = null, string format = "yyyy-MM-dd HH:mm:ss") where TValue : notnull
    {
        if (total.HasValue)
        {
            return Results.Ok(new PageResultDto<TValue>
            {
                Data = value,
                Total = total.Value,
                Code = ResultCode.Success,
                Message = "成功",
            });
        }
        else
        {
            return Results.Ok(new OperateResultDto<TValue>
            {
                Data = value,
                Code = ResultCode.Success,
                Message = "成功",
            });
        }
    }

    /// <summary>
    /// 公共操作返回
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public virtual IResult XunetResult()
    {
        return Results.Ok(new OperateResultDto
        {
            Code = ResultCode.Success,
            Message = "成功",
        });
    }
}
