namespace Xunet.WinFormium.Tests;

using SuperSpider;
using Xunet.WinFormium.Tests.Entities;
using Xunet.WinFormium.Tests.Models;

/// <summary>
/// MainForm
/// </summary>
public class MainForm : BaseForm
{
    /// <summary>
    /// BaseText
    /// </summary>
    protected override string BaseText => $"测试 - {Version}";

    /// <summary>
    /// BaseClientSize
    /// </summary>
    protected override Size BaseClientSize => new(600, 400);

    /// <summary>
    /// BaseDoWorkInterval
    /// </summary>
    protected override int BaseDoWorkInterval => GetConfigValue<int>("DoWorkInterval");

    /// <summary>
    /// DoWorkAsync
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

        AppendBox("测试完成！", ColorTranslator.FromHtml("#1296db"));

        await Task.CompletedTask;
    }

    /// <summary>
    /// DoExceptionAsync
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
    /// DoCanceledExceptionAsync
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    protected override async Task DoCanceledExceptionAsync(OperationCanceledException ex)
    {
        AppendBox("任务取消！", Color.Red);

        await Task.CompletedTask;
    }
}
