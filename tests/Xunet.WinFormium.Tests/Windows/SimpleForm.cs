namespace Xunet.WinFormium.Tests.Windows;

using System;
using System.Threading;
using System.Threading.Tasks;
using Xunet.WinFormium.Windows;

/// <summary>
/// 主窗体
/// </summary>
public class SimpleForm : BaseForm
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

        AppendBox("采集完成！", ColorTranslator.FromHtml("#1296db"));

        await Task.CompletedTask;
    }
}
