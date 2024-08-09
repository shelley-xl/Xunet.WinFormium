// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

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
