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
using Xunet.WinFormium;
using Xunet.WinFormium.Core;
using Xunet.WinFormium.Tests;
using Xunet.WinFormium.Tests.Models;

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
        DbName = "Xunet.WinFormium.Tests",
        EntityTypes = [typeof(CnBlogsModel)]
    };
    options.Snowflake = new()
    {
        WorkerId = 1
    };
});

var app = builder.Build();

app.UseWinFormium();

app.UseSingleApp();

app.Run();
```

MainForm.cs

```c#
namespace Xunet.WinFormium.Tests;

using Xunet.WinFormium.Tests.Models;

public class MainForm : BaseForm
{
    protected override string BaseText => $"测试 - {Version}";

    protected override Size BaseClientSize => new(600, 400);

    protected override int BaseDoWorkInterval => GetConfigValue<int>("DoWorkInterval");

    protected override async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        AppendBox("正在测试，请稍后 ...", ColorTranslator.FromHtml("#1296db"));

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

            await Task.Delay(new Random().Next(100, 500), cancellationToken);
        }

        AppendBox("测试完成！", ColorTranslator.FromHtml("#1296db"));

        await Task.CompletedTask;
    }

    protected override async Task DoExceptionAsync(Exception ex, CancellationToken cancellationToken)
    {
        AppendBox("系统异常！", Color.Red);
        AppendBox(ex.ToString(), Color.Red);

        await Task.CompletedTask;
    }

    protected override async Task DoCanceledExceptionAsync(OperationCanceledException ex)
    {
        AppendBox("任务取消！", Color.Red);

        await Task.CompletedTask;
    }
}
```

CnBlogsModel.cs

```c#
namespace Xunet.WinFormium.Tests.Models;

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

appsettings.json

```json
{
  "DoWorkInterval": 60
}
```

## 更新日志

[CHANGELOG](CHANGELOG.md)
