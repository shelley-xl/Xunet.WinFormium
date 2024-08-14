namespace Xunet.WinFormium.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using Xunet.WinFormium.Controllers;
using Xunet.WinFormium.Tests.Models;

/// <summary>
/// 首页
/// </summary>
/// <param name="Db"></param>
[ApiExplorerSettings(GroupName = "home")]
[Route("api/home")]
public class HomeController(ISqlSugarClient Db) : BaseController
{
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [HttpGet("list/page")]
    public async Task<IActionResult> ListPage(int page = 1, int size = 20)
    {
        RefAsync<int> totalNumber = new(0);

        var list = await Db.Queryable<CnBlogsModel>().ToPageListAsync(page, size, totalNumber);

        return XunetResult(list, totalNumber);
    }
}
