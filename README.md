# Xunet.WinFormium

A Lightweight WinForm Application Framework For .NET

Support .NET 8.0

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
namespace Xunet.WinFormium.Tests;

using Xunet.WinFormium.Core;
using Xunet.WinFormium.Tests.Models;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        ServiceConfiguration.Initialize(new StartupOptions
        {
            Headers = new()
            {
                {
                    HeaderNames.UserAgent,
                    "Mozilla/5.0 (iPhone; CPU iPhone OS 6_1_3 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Mobile/10B329 MicroMessenger/5.0.1"
                }
            },
            Storage = new()
            {
                StorageName = "Xunet.WinFormium.Tests",
                EntityTypes = [typeof(CnBlogsModel)]
            },
            Generator = new()
            {
                WorkerId = 1
            }
        });

        Application.Run(new MainForm());
    }
}
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
        AppendBox(this, "正在测试，请稍后 ...", ColorTranslator.FromHtml("#1296db"));

        var html = await DefaultClient.GetStringAsync("https://www.cnblogs.com/", cancellationToken);

        CreateHtmlDocument(html);

        var list = FindElementsByXPath("//*[@id=\"post_list\"]/article");

        foreach (var item in list)
        {
            var model = new CnBlogsModel
            {
                Id = NextIdString,
                Title = FindText(FindElementByXPath(item, "section/div/a")),
                Url = FindAttributeValue(FindElementByXPath(item, "section/div/a"), "href"),
                Summary = Trim(FindText(FindElementByXPath(item, "section/div/p"))),
                CreateTime = DateTime.Now
            };

            AppendBox(this, $"{model.Title} ...");

            await Db.Insertable(model).ExecuteCommandAsync(cancellationToken);

            await Task.Delay(new Random().Next(100, 500), cancellationToken);
        }

        AppendBox(this, "测试完成！", ColorTranslator.FromHtml("#1296db"));
    }

    protected override async Task DoExceptionAsync(Exception ex, CancellationToken cancellationToken)
    {
        AppendBox(this, "系统异常！", Color.Red);
        AppendBox(this, ex.ToString(), Color.Red);

        await Task.CompletedTask;
    }

    protected override async Task DoCanceledExceptionAsync(OperationCanceledException ex)
    {
        AppendBox(this, "任务取消！", Color.Red);

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
  // 工作周期频率（单位：秒），设置 0 时仅工作一次
  "DoWorkInterval": 60
}
```

## 更新日志

[CHANGELOG](CHANGELOG.md)
