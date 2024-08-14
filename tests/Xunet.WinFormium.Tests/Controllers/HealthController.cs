namespace Xunet.WinFormium.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Xunet.WinFormium.Controllers;

/// <summary>
/// 健康检查
/// </summary>
[Route("api/health")]
public class HealthController : BaseController
{
    /// <summary>
    /// 健康检查
    /// </summary>
    /// <returns></returns>
    [HttpGet("check")]
    public async Task<IActionResult> Check()
    {
        return XunetResult(await Task.FromResult(DateTime.Now), format: "yyyy-MM-dd HH:mm:ss.ffff");
    }
}
