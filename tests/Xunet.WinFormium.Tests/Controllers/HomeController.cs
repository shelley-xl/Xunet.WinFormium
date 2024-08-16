namespace Xunet.WinFormium.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Xunet.WinFormium.Controllers;
using Xunet.WinFormium.Tests.Models;
using SqlSugar;
using Xunet.WinFormium.Tests.Entities;

/// <summary>
/// 首页
/// </summary>
/// <param name="Db"></param>
[Route("api/home")]
public class HomeController(ISqlSugarClient Db) : BaseController
{
    /// <summary>
    /// 获取csdn博客列表
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [HttpGet("csdn/list/page")]
    public async Task<IActionResult> CsdnListPage(int page = 1, int size = 20)
    {
        RefAsync<int> totalNumber = new(0);

        var list = await Db.Queryable<CnBlogsModel>().ToPageListAsync(page, size, totalNumber);

        return XunetResult(list, totalNumber);
    }

    /// <summary>
    /// 获取微博热搜列表
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [HttpGet("weibo/list/page")]
    public async Task<IActionResult> WeiboListPage(int page = 1, int size = 20)
    {
        RefAsync<int> totalNumber = new(0);

        var list = await Db.Queryable<WeiboEntity>().ToPageListAsync(page, size, totalNumber);

        return XunetResult(list, totalNumber);
    }
}
