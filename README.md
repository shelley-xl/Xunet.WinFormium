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
using Xunet.WinFormium.Core;

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
                //StorageName = "Xunet.WinFormium.Tests",
                //EntityTypes = [typeof(TestModel)]
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
using Xunet.WinFormium

public class MainForm : BaseForm
{
    protected override string BaseText => $"测试 - {Version}";

    protected override Size BaseClientSize => new(600, 400);

    protected override int BaseDoWorkInterval => 60;

    protected override void DoWork(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            try
            {
                AppendBox(this, "开始工作，请稍后 ...", ColorTranslator.FromHtml("#1296db"));

                // TODO

                AppendBox(this, "工作完成！", ColorTranslator.FromHtml("#1296db"));
            }
            catch (Exception ex)
            {
                AppendBox(this, "系统错误！", Color.Red);
                AppendBox(this, ex.ToString(), Color.Red);
            }
        }, cancellationToken);
    }
}
```

## 更新日志

[CHANGELOG](CHANGELOG.md)
