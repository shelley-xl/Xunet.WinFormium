namespace Xunet.WinFormium.Simples.Windows;

using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Xunet.WinFormium.Windows;

/// <summary>
/// 主窗体
/// </summary>
public class WebView2Form : BaseForm
{
    /// <summary>
    /// 标题
    /// </summary>
    protected override string BaseText => $"测试 - {Version}";

    /// <summary>
    /// 窗体大小
    /// </summary>
    protected override Size BaseClientSize => new(1200, 600);

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
    /// 是否使用 WebView2
    /// </summary>
    protected override bool UseWebView2 => true;

    /// <summary>
    /// WebView2初始化源
    /// </summary>
    protected override string BaseWebView2Source => "https://www.baidu.com/";

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
        AppendBox("测试一下");

        await Task.CompletedTask;
    }

    /// <summary>
    /// 初始化完成事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="cancellationToken"></param>
    protected override async Task WebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e, CancellationToken cancellationToken)
    {
        AppendBox("初始化完成！");

        await Task.CompletedTask;
    }

    /// <summary>
    /// 导航完成事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="cancellationToken"></param>
    protected override async Task WebView2NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e, CancellationToken cancellationToken)
    {
        if (sender is WebView2 webView2 && e.IsSuccess)
        {
            AppendBox(webView2.Source.AbsoluteUri);

            await Task.CompletedTask;
        }
    }
}
