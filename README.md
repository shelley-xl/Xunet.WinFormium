# Xunet.WinFormium

基于.NET Core的轻量级爬虫框架，支持标准的http请求，网页解析，网页自动化，执行js脚本，数据存储等，内置.NET WebApi支持，同时提供通用的Winform组件。

Support .NET 8.0/9.0

[![Nuget](https://img.shields.io/nuget/v/Xunet.WinFormium.svg?style=flat-square)](https://www.nuget.org/packages/Xunet.WinFormium)
[![Downloads](https://img.shields.io/nuget/dt/Xunet.WinFormium.svg?style=flat-square)](https://www.nuget.org/stats/packages/Xunet.WinFormium?groupby=Version)
[![License](https://img.shields.io/github/license/shelley-xl/Xunet.WinFormium.svg)](https://github.com/shelley-xl/Xunet.WinFormium/blob/master/LICENSE)
![Vistors](https://visitor-badge.laobi.icu/badge?page_id=https://github.com/shelley-xl/Xunet.WinFormium)

## 安装

Xunet.WinFormium 以 NuGet 包的形式提供。您可以使用 NuGet 包控制台窗口安装它：

```
PM> Install-Package Xunet.WinFormium
```

## 使用

Program.cs

```c#
using System.Reflection;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        var builder = WinFormiumApplication.CreateBuilder();

        builder.Services.AddWinFormium<MainForm>(options =>
        {
            options.Headers = new()
            {
                {
                    HeaderNames.UserAgent,
                    "Mozilla/5.0 (iPhone; CPU iPhone OS 6_1_3 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Mobile/10B329 MicroMessenger/5.0.1"
                }
            };
            options.Storage = new()
            {
                DataVersion = "24.8.9.1822",
                DbName = "Xunet.WinFormium.Simples",
                EntityTypes = [typeof(CnBlogsModel)]
            };
            options.Snowflake = new()
            {
                WorkerId = 1
            };
        });

        builder.Services.AddWebApi(Assembly.GetExecutingAssembly(), (provider, services) =>
        {
            var db = provider.GetRequiredService<ISqlSugarClient>();

            services.AddSingleton(db);
        });

        var app = builder.Build();

        app.UseWinFormium();

        app.UseSingleApp();

        app.UseWebApi();

        app.Run();
    }
}
```

MainForm.cs

```c#
namespace Xunet.WinFormium.Simples.Windows;

using SuperSpider;
using Xunet.WinFormium.Windows;
using Xunet.WinFormium.Simples.Entities;
using Xunet.WinFormium.Simples.Models;

/// <summary>
/// 主窗体
/// </summary>
public class MainForm : BaseForm
{
    /// <summary>
    /// 标题
    /// </summary>
    protected override string BaseText => $"测试 - {Version}";

    /// <summary>
    /// 窗体大小
    /// </summary>
    protected override Size BaseClientSize => new(600, 400);

    /// <summary>
    /// 工作周期频率（单位：秒），设置 0 时仅工作一次
    /// </summary>
    protected override int BaseDoWorkInterval => GetConfigValue<int>("DoWorkInterval");

    /// <summary>
    /// 是否使用默认菜单
    /// </summary>
    protected override bool UseDefaultMenu => true;

    /// <summary>
    /// 是否使用状态栏
    /// </summary>
    protected override bool UseStatusStrip => true;

    /// <summary>
    /// 是否使用表格数据展示
    /// </summary>
    protected override bool UseDatagridView => true;

    /// <summary>
    /// 任务取消
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    protected override async Task DoCanceledExceptionAsync(OperationCanceledException ex)
    {
        AppendBox("任务取消！", Color.Red);

        await Task.CompletedTask;
    }

    /// <summary>
    /// 系统异常
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task DoExceptionAsync(Exception ex, CancellationToken cancellationToken)
    {
        AppendBox("系统异常！", Color.Red);
        AppendBox(ex.ToString(), Color.Red);

        await Task.CompletedTask;
    }

    /// <summary>
    /// 执行任务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        AppendBox("正在采集数据，请稍后 ...", ColorTranslator.FromHtml("#1296db"));

        var data = await Db.Queryable<CnBlogsModel>().OrderByDescending(x => x.CreateTime).ToListAsync(cancellationToken);

        AppendDatagridView(data);

        var html = await DefaultClient.GetStringAsync("https://www.cnblogs.com/", cancellationToken);

        CreateHtmlDocument(html);

        var list = FindElementsByXPath("//*[@id=\"post_list\"]/article");

        foreach (var item in list)
        {
            var model = new CnBlogsModel
            {
                Id = CreateNextIdString(),
                Title = FindText(FindElementByXPath(item, "section/div/a")),
                Url = FindAttributeValue(FindElementByXPath(item, "section/div/a"), "href"),
                Summary = Trim(FindText(FindElementByXPath(item, "section/div/p"))),
                CreateTime = DateTime.Now
            };

            AppendBox($"{model.Title} ...");

            await Db.Insertable(model).ExecuteCommandAsync(cancellationToken);

            Thread.Sleep(500);
        }

        using var request_wb = new Request<WeiboEntity>
        {
            RequestUri = new Uri("https://s.weibo.com/top/summary?cate=realtimehot"),
            Headers =
            {
                { HeaderNames.Cookie, "SUB=_2AkMRPey0f8NxqwFRmP0QzG7jZIh-zA_EieKnYR1vJRMyHRl-yD9yqhMPtRB6Or3CWw-34jkWWR4Y0x2HL1v5PpcCYaf4" },
                { HeaderNames.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36" }
            },
            FilterConditions = x => x.RankTop.HasValue,
            DuplicateColumns = x => new { x.Keywords }
        };

        Request[] requests = [request_wb];

        await EntitySpider.Build(requests).UseStorage(Db).RunAsync();

        data = await Db.Queryable<CnBlogsModel>().OrderByDescending(x => x.CreateTime).ToListAsync(cancellationToken);

        AppendDatagridView(data);

        AppendBox("采集完成！", ColorTranslator.FromHtml("#1296db"));

        await Task.CompletedTask;
    }
}
```

CnBlogsModel.cs

```c#
namespace Xunet.WinFormium.Simples.Models;

using SqlSugar;

[SugarTable("cnblogs")]
public class CnBlogsModel
{
    [SugarColumn(IsPrimaryKey = true)]
    public string? Id { get; set; }

    public string? Title { get; set; }

    public string? Url { get; set; }

    public string? Summary { get; set; }

    public DateTime? CreateTime { get; set; }
}
```

WeiboEntity.cs

```c#
namespace Xunet.WinFormium.Simples.Entities;

using SuperSpider;

/// <summary>
/// 微博热搜
/// </summary>
[SpiderSchema("weibo", "微博热搜")]
[SpiderIndex("unique_weibo_Keywords", nameof(Keywords), true, true)]
[EntitySelector(Expression = "//*[@id=\"pl_top_realtimehot\"]/table/tbody/tr")]
public class WeiboEntity : SpiderEntity
{
    /// <summary>
    /// 排行
    /// </summary>
    [SpiderColumn(ColumnDescription = "排行", IsNullable = true)]
    [RegexFormatter(Pattern = "[0-9]+")]
    [ValueSelector(Expression = "//td[1]")]
    public int? RankTop { get; set; }

    /// <summary>
    /// 关键词
    /// </summary>
    [SpiderColumn(ColumnDescription = "关键词", IsNullable = true)]
    [ValueSelector(Expression = "//td[2]/a")]
    public string? Keywords { get; set; }

    /// <summary>
    /// HotText
    /// </summary>
    [SpiderColumn(ColumnDescription = "热度文本", IsNullable = true)]
    [RegexFormatter(Pattern = "[\u4E00-\u9FA5]+")]
    [ValueSelector(Expression = "//td[2]/span")]
    public string? HotText { get; set; }

    /// <summary>
    /// 热度值
    /// </summary>
    [SpiderColumn(ColumnDescription = "热度值", IsNullable = true)]
    [RegexFormatter(Pattern = "[0-9]+")]
    [ValueSelector(Expression = "//td[2]/span")]
    public int? HotValue { get; set; }

    /// <summary>
    /// 热度标签
    /// </summary>
    [SpiderColumn(ColumnDescription = "热度标签", IsNullable = true)]
    [ValueSelector(Expression = "//td[3]/i")]
    public string? HotTag { get; set; }

    /// <summary>
    /// 链接
    /// </summary>
    [SpiderColumn(ColumnDescription = "链接", IsNullable = true)]
    [ValueSelector(Expression = "//td[2]/a/@href")]
    public string? Url { get; set; }
}
```

HomeController.cs

```c#
namespace Xunet.WinFormium.Simples.Controllers;

using Microsoft.AspNetCore.Mvc;
using Xunet.WinFormium.Controllers;
using Xunet.WinFormium.Simples.Models;
using SqlSugar;
using Xunet.WinFormium.Simples.Entities;
using Microsoft.AspNetCore.Http;

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
    public async Task<IResult> CsdnListPage(int page = 1, int size = 20)
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
    public async Task<IResult> WeiboListPage(int page = 1, int size = 20)
    {
        RefAsync<int> totalNumber = new(0);

        var list = await Db.Queryable<WeiboEntity>().ToPageListAsync(page, size, totalNumber);

        return XunetResult(list, totalNumber);
    }
}
```

appsettings.json

```json
{
  "Kestrel": {
    "EndPoints": {
      "Http": {
        "Url": "http://0.0.0.0:12345"
      }
    }
  },
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  // 工作周期频率（单位：秒），设置 0 时仅工作一次
  "DoWorkInterval": 60,
  "SpiderConfig": {
    "ConnectionConfig": [
      {
        "ConfigId": 1,
        "DbType": 0,
        "InitKeyType": 1,
        "IsAutoCloseConnection": true,
        "ConnectionString": "server=127.0.0.1;port=3306;uid=root;pwd=123456;database=hotsearch;max pool size=8000;charset=utf8;",
        "SlaveConnectionConfigs": [
          {
            "ConnectionString": "server=127.0.0.1;port=3306;uid=root;pwd=123456;database=hotsearch;max pool size=8000;charset=utf8;",
            "HitRate": 10
          }
        ]
      }
    ]
  }
}
```

## 更新日志

[CHANGELOG](CHANGELOG.md)
