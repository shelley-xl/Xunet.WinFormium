// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium.Windows;

using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Xunet.Newtonsoft.Json;
using Xunet.Newtonsoft.Json.Serialization;
using Xunet.FluentScheduler;
using Xunet.WinFormium.Core;
using Yitter.IdGenerator;
using HtmlAgilityPack;
using SqlSugar;
using System.Diagnostics;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;

/// <summary>
/// 窗体基类
/// </summary>
public abstract class BaseForm : Form, IDisposable
{
    #region 字段

    #region 多线程票据

    /// <summary>
    /// 多线程票据
    /// </summary>
    protected static CancellationTokenSource TokenSource { get; set; } = new();

    #endregion

    #region 私有

    /// <summary>
    /// 配置
    /// </summary>
    static IConfigurationRoot Configuration
    {
        get
        {
            return DependencyResolver.Current?.GetRequiredService<IConfigurationRoot>() ?? throw new InvalidOperationException("No service for type 'IConfigurationRoot' has been registered.");
        }
    }

    /// <summary>
    /// 屏幕缩放比例
    /// </summary>
    static float ScalingFactor
    {
        get
        {
            return DpiHelper.GetDpiScalingFactor();
        }
    }

    /// <summary>
    /// HTML文档
    /// </summary>
    static HtmlDocument? HtmlDocument { get; set; }

    /// <summary>
    /// WebView2
    /// </summary>
    static WebView2? WebView2 { get; set; }

    /// <summary>
    /// 当前窗体
    /// </summary>
    Form? HostWindow { get; set; }

    /// <summary>
    /// NotifyIcon
    /// </summary>
    NotifyIcon? NotifyIcon;

    /// <summary>
    /// BoxControl
    /// </summary>
    Control.ControlCollection? BoxControl;

    #endregion

    #region 只读

    /// <summary>
    /// 版本号
    /// </summary>
    protected static string Version
    {
        get
        {
            return $"v{Assembly.GetEntryAssembly()?.GetName().Version}";
        }
    }

    /// <summary>
    /// 默认请求客户端
    /// </summary>
    protected static HttpClient DefaultClient
    {
        get
        {
            return DependencyResolver.Current?.GetRequiredService<IHttpClientFactory>()?.CreateClient("default") ?? throw new InvalidOperationException("No headers for type 'StartupOptions.RequestHeaders' has been initialized.");
        }
    }

    /// <summary>
    /// 数据库访问
    /// </summary>
    protected static ISqlSugarClient Db
    {
        get
        {
            return DependencyResolver.Current?.GetRequiredService<ISqlSugarClient>() ?? throw new InvalidOperationException("No storage for type 'StartupOptions.SqliteStorage' has been initialized.");
        }
    }

    /// <summary>
    /// WebView2
    /// </summary>
    protected static WebView2 WebView2Instance
    {
        get
        {
            return WebView2 ?? throw new InvalidOperationException("Please override the UseWebView2 field and set it to true.");
        }
    }

    #endregion

    #region 抽象

    /// <summary>
    /// 标题
    /// </summary>
    protected abstract string BaseText { get; }

    #endregion

    #region 重写

    /// <summary>
    /// 窗体状态
    /// </summary>
    protected virtual FormWindowState BaseWindowState { get; } = FormWindowState.Normal;

    /// <summary>
    /// 是否启用最大化控件
    /// </summary>
    protected virtual bool BaseMaximizeBox { get; } = true;

    /// <summary>
    /// 窗体背景色
    /// </summary>
    protected virtual Color BaseBackColor { get; } = DefaultBackColor;

    /// <summary>
    /// 窗体边框样式
    /// </summary>
    protected virtual FormBorderStyle BaseFormBorderStyle { get; } = FormBorderStyle.Sizable;

    /// <summary>
    /// 窗体大小
    /// </summary>
    protected virtual Size BaseClientSize { get; } = new Size(400, 400);

    /// <summary>
    /// 工作周期频率（单位：秒），设置 0 时仅工作一次
    /// </summary>
    protected virtual int BaseDoWorkInterval { get; } = 0;

    /// <summary>
    /// 工作定时Cron表达式，优先级大于BaseDoWorkInterval
    /// </summary>
    protected virtual string BaseDoWorkCron { get; } = string.Empty;

    /// <summary>
    /// 是否使用默认菜单
    /// </summary>
    protected virtual bool UseDefaultMenu { get; } = false;

    /// <summary>
    /// 是否使用表格数据展示
    /// </summary>
    protected virtual bool UseDatagridView { get; } = false;

    /// <summary>
    /// 是否使用状态栏（在线时长、内存占用情况显示）
    /// </summary>
    protected virtual bool UseStatusStrip { get; } = false;

    /// <summary>
    /// 是否使用WebView2展示，优先级大于UseDatagridView
    /// </summary>
    protected virtual bool UseWebView2 { get; } = false;

    /// <summary>
    /// WebView2初始化源
    /// </summary>
    protected virtual string BaseWebView2Source { get; } = "https://www.baidu.com/";

    #endregion   

    #endregion

    #region 设计器

    /// <summary>
    ///  必需的设计变量。
    /// </summary>
    readonly IContainer? components = null;

    /// <summary>
    ///  清理所有正在使用的资源。
    /// </summary>
    /// <param name="disposing">如果应处置托管资源，则为 true；否则为 false。</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows窗体设计器生成的代码

    /// <summary>
    ///  设计器支持所需的方法-不要修改
    ///  使用代码编辑器修改此方法的内容。
    /// </summary>
    void InitializeComponent()
    {
        SuspendLayout();
        // 
        // BaseForm
        // 
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        Icon = Properties.Resources.favicon;
        Name = "BaseForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = BaseText;
        MaximizeBox = BaseMaximizeBox;
        BackColor = BaseBackColor;
        FormBorderStyle = BaseFormBorderStyle;
        WindowState = BaseWindowState;
        ClientSize = BaseClientSize;
        MinimumSize = BaseClientSize;
        FormClosing += Form_FormClosing;
        Load += Form_Load;
        ResumeLayout(false);
    }

    #endregion

    #endregion

    #region 构造函数

    /// <summary>
    /// 构造函数
    /// </summary>
    protected BaseForm()
    {
        HostWindow = this;

        InitializeComponent();

        if (UseDefaultMenu)
        {
            InitializeDefaultMenu();
        }

        if (UseStatusStrip)
        {
            InitializeStatusStrip();
        }

        if (UseWebView2)
        {
            InitializeWebView2();
        }
        else if (UseDatagridView)
        {
            InitializeDatagridView();
        }

        InitializeControl();
    }

    #endregion

    #region 初始化默认菜单

    /// <summary>
    /// 初始化默认菜单
    /// </summary>
    protected virtual void InitializeDefaultMenu()
    {
        if (HostWindow == null || HostWindow.IsDisposed) return;

        InvokeOnUIThread(() =>
        {
            // 菜单
            var menu = new MenuStrip
            {
                Name = "Menu",
            };

            // 一级
            var tsmi_work = new ToolStripMenuItem
            {
                Text = "作业"
            };

            var tsmi_fun = new ToolStripMenuItem
            {
                Text = "任务",
            };

            var tsmi_op = new ToolStripMenuItem
            {
                Text = "日志",
            };

            var tsmi_ab = new ToolStripMenuItem
            {
                Text = "关于",
            };

            // 二级
            var tsmi_start = new ToolStripMenuItem
            {
                Name = "Start",
                Text = "开启",
                Enabled = false,
            };

            var tsmi_stop = new ToolStripMenuItem
            {
                Name = "Pause",
                Text = "暂停",
                Enabled = BaseDoWorkInterval > 0,
            };

            var tsmi_exc = new ToolStripMenuItem
            {
                Name = "Execute",
                Text = "运行",
                Enabled = true,
            };

            var tsmi_can = new ToolStripMenuItem
            {
                Name = "Cancel",
                Text = "取消",
                Enabled = false,
            };

            var tsmi_export = new ToolStripMenuItem
            {
                Text = "导出",
            };

            var tsmi_clear = new ToolStripMenuItem
            {
                Text = "清空",
            };

            var tsmi_about = new ToolStripMenuItem
            {
                Text = "关于软件",
            };

            // 事件
            tsmi_start.Click += (sender, e) =>
            {
                tsmi_start.Enabled = false;
                tsmi_stop.Enabled = true;
                Task.Run(AddJob);
            };

            tsmi_stop.Click += (sender, e) =>
            {
                tsmi_stop.Enabled = false;
                tsmi_start.Enabled = true;
                JobManager.RemoveJob("DoWork");
            };

            tsmi_exc.Click += (sender, e) =>
            {
                tsmi_exc.Enabled = false;
                Task.Run(DoWork);
            };

            tsmi_can.Click += (sender, e) =>
            {
                tsmi_can.Enabled = false;
                TokenSource.Cancel();
                TokenSource = new CancellationTokenSource();
            };

            tsmi_export.Click += (sender, e) =>
            {
                var sfd = new SaveFileDialog
                {
                    // 设置保存文件对话框的标题
                    Title = "请选择要保存的文件路径",
                    // 初始化保存目录，默认exe文件目录
                    InitialDirectory = Application.StartupPath,
                    // 设置保存文件的类型
                    Filter = "日志文件|*.log",
                    // 设置默认文件名
                    FileName = $"console_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.log"
                };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (BoxControl?.Find("Box", false).FirstOrDefault() is RichTextBox box)
                    {
                        // 获得保存文件的路径
                        string filePath = sfd.FileName;
                        // 保存
                        using var writer = new StreamWriter(filePath);
                        writer.Write(box.Text);
                        writer.Flush();
                        MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            };

            tsmi_clear.Click += (sender, e) =>
            {
                if (BoxControl?.Find("Box", false).FirstOrDefault() is RichTextBox box)
                {
                    box.Clear();
                    MessageBox.Show("清空成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            tsmi_about.Click += (sender, e) =>
            {
                MessageBox.Show($"当前版本：{Version}\r\n\r\n开发作者：徐来（QQ386710057）", "关于软件", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            // 绑定
            tsmi_work.DropDownItems.AddRange(
            [
                tsmi_start,
                tsmi_stop,
            ]);

            tsmi_fun.DropDownItems.AddRange(
            [
                tsmi_exc,
                tsmi_can,
            ]);

            tsmi_op.DropDownItems.AddRange(
            [
                tsmi_export,
                tsmi_clear,
            ]);

            tsmi_ab.DropDownItems.AddRange(
            [
                tsmi_about,
            ]);

            menu.Items.AddRange(
            [
                tsmi_work,
                tsmi_fun,
                tsmi_op,
                tsmi_ab,
            ]);

            // 托盘
            var tsmi_main = new ToolStripMenuItem
            {
                Text = "主窗体"
            };

            var tsmi_about_1 = new ToolStripMenuItem
            {
                Text = "关于软件",
            };

            var tsmi_close_1 = new ToolStripMenuItem
            {
                Text = "关闭软件",
            };

            tsmi_main.Click += (sender, e) =>
            {
                Show();
                Activate();
            };

            tsmi_about_1.Click += (sender, e) =>
            {
                MessageBox.Show($"当前版本：{Version}\r\n\r\n开发作者：徐来", "关于软件", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            tsmi_close_1.Click += (sender, e) =>
            {
                Environment.Exit(0);
            };

            var cms = new ContextMenuStrip
            {
                Name = "ContextMenu",
            };

            cms.Items.Add(tsmi_main);
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.AddRange(
            [
                tsmi_about_1,
                tsmi_close_1,
            ]);

            NotifyIcon = new NotifyIcon
            {
                Icon = Properties.Resources.favicon,
                Visible = true,
                Text = Text,
                ContextMenuStrip = cms,
            };

            NotifyIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    Show();
                    Activate();
                }
            };

            HostWindow.Controls.Add(menu);
        });
    }
    #endregion

    #region 初始化状态栏

    /// <summary>
    /// 初始化状态栏
    /// </summary>
    protected virtual void InitializeStatusStrip()
    {
        if (HostWindow == null || HostWindow.IsDisposed) return;

        InvokeOnUIThread(() =>
        {
            int seconds = 0;
            var statusStrip = new StatusStrip
            {
                Name = "StatusStrip",
            };
            var timeLabel = new ToolStripStatusLabel
            {
                Text = "loading...",
            };
            statusStrip.Items.Add(timeLabel);
            HostWindow.Controls.Add(statusStrip);
            JobManager.AddJob(() =>
            {
                if (HostWindow == null || HostWindow.IsDisposed) return;

                var counter = new PerformanceCounter("Process", "Working Set - Private", Process.GetCurrentProcess().ProcessName);
                var usedMemory = Math.Round(counter.RawValue / 1024.0 / 1024.0, 1);
                int hours = seconds / 3600;
                int minutes = seconds % 3600 / 60;
                int remainingSeconds = seconds % 3600 % 60;
                var nextRun = string.Empty;
                var doWork = JobManager.GetSchedule("DoWork");
                if (doWork != null)
                {
                    nextRun = $"，下次执行还剩：{(int)(doWork.NextRun - DateTime.Now).TotalSeconds:00} 秒";
                }
                else
                {
                    nextRun = "，未开启定时作业";
                }

                BeginInvoke(new Action(() =>
                {
                    timeLabel.Text = $"在线时长：{hours:00} 小时 {minutes:00} 分 {remainingSeconds:00} 秒，内存：{usedMemory:0.0} MB{nextRun}";
                }));

                seconds++;
            },
            schedule =>
            {
                schedule.WithName("PerformanceCounter");
                schedule.ToRunNow().AndEvery(1).Seconds();
            });
        });
    }

    #endregion

    #region 初始化WebView2

    /// <summary>
    /// 初始化WebView2
    /// </summary>
    protected virtual void InitializeWebView2()
    {
        if (HostWindow == null || HostWindow.IsDisposed) return;

        InvokeOnUIThread(() =>
        {
            var bottom = 0;
            var offset = 0;
            if (HostWindow.Controls.Find("Menu", false).FirstOrDefault() is MenuStrip Menu)
            {
                offset = Menu.Height;
            }
            if (HostWindow.Controls.Find("StatusStrip", false).FirstOrDefault() is StatusStrip Status)
            {
                bottom = Status.Height;
            }

            WebView2 = new()
            {
                Name = "WebView2",
                Dock = DockStyle.Fill,
                Source = new Uri(BaseWebView2Source),
            };

            WebView2.CoreWebView2InitializationCompleted += (sender, e) =>
            {
                WebView2InitializationCompletedAsync(sender, e, TokenSource.Token);
            };

            WebView2.NavigationCompleted += (sender, e) =>
            {
                WebView2NavigationCompletedAsync(sender, e, TokenSource.Token);
            };

            // 初始化分割容器控件
            var splitContainer = new SplitContainer
            {
                Name = "Split",
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left,
                Width = HostWindow.Width,
                Height = HostWindow.ClientRectangle.Height - offset - bottom,
                Location = new Point(0, offset),
                SplitterDistance = (int)((BaseClientSize.Width - 400) * ScalingFactor),
            };

            // 分割容器左边
            splitContainer.Panel1.Controls.Add(WebView2);

            // 添加到主窗体
            HostWindow.Controls.Add(splitContainer);
        });
    }

    /// <summary>
    /// 初始化完成事件
    /// </summary>
    protected virtual Task WebView2InitializationCompletedAsync(object? sender, CoreWebView2InitializationCompletedEventArgs e, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 导航完成事件
    /// </summary>
    protected virtual Task WebView2NavigationCompletedAsync(object? sender, CoreWebView2NavigationCompletedEventArgs e, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    #endregion

    #region 初始化表格

    /// <summary>
    /// 初始化表格
    /// </summary>
    protected virtual void InitializeDatagridView()
    {
        if (HostWindow == null || HostWindow.IsDisposed) return;

        InvokeOnUIThread(() =>
        {
            var bottom = 0;
            var offset = 0;
            if (HostWindow.Controls.Find("Menu", false).FirstOrDefault() is MenuStrip Menu)
            {
                offset = Menu.Height;
            }
            if (HostWindow.Controls.Find("StatusStrip", false).FirstOrDefault() is StatusStrip Status)
            {
                bottom = Status.Height;
            }
            var Tab = new TabControl
            {
                Name = "Tab",
                SelectedIndex = 1,
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left,
                Width = HostWindow.Width,
                Height = HostWindow.ClientRectangle.Height - offset - bottom,
                Location = new Point(0, offset),
            };
            var TabPageHome = new TabPage
            {
                Name = "TabPageHome",
                Text = "首页",
            };
            var TabPageBox = new TabPage
            {
                Name = "TabPageBox",
                Text = "日志",
            };
            Tab.TabPages.Add(TabPageHome);
            Tab.TabPages.Add(TabPageBox);
            HostWindow.Controls.Add(Tab);
        });
    }

    #endregion

    #region 初始化控件

    /// <summary>
    /// 初始化控件
    /// </summary>
    protected virtual void InitializeControl()
    {

    }

    #endregion

    #region 窗体加载事件

    readonly static AsyncSemaphore _async = new();

    void IsRuning(bool enabled)
    {
        if (HostWindow == null || HostWindow.IsDisposed) return;

        InvokeOnUIThread(() =>
        {
            if (HostWindow.Controls.Find("Menu", false).FirstOrDefault() is MenuStrip menu)
            {
                if (menu.Items.Find("Execute", true).FirstOrDefault() is ToolStripMenuItem tsmi_exc)
                {
                    tsmi_exc.Enabled = !enabled;
                }
                if (menu.Items.Find("Cancel", true).FirstOrDefault() is ToolStripMenuItem tsmi_can)
                {
                    tsmi_can.Enabled = enabled;
                }
            }
        });
    }

    async void DoWork()
    {
        using (await _async.WaitAsync())
        {
            try
            {
                IsRuning(true);
                await DoWorkAsync(TokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                await DoCanceledExceptionAsync(ex);
            }
            catch (Exception ex)
            {
                await DoExceptionAsync(ex, TokenSource.Token);
            }
            finally
            {
                IsRuning(false);
            }
        }
    }

    void AddJob()
    {
        if (!string.IsNullOrEmpty(BaseDoWorkCron))
        {
            JobManager.AddJob(DoWork, schedule => schedule.WithName("DoWork").ToRunWithCron(BaseDoWorkCron));
        }
        else if (BaseDoWorkInterval > 0)
        {
            JobManager.AddJob(DoWork, schedule => schedule.WithName("DoWork").ToRunNow().AndEvery(BaseDoWorkInterval).Seconds());
        }
        else
        {
            DoWork();
        }
    }

    /// <summary>
    /// 窗体加载事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Form_Load(object? sender, EventArgs e)
    {
        Task.Run(AddJob);
    }

    #endregion

    #region 窗体关闭事件

    /// <summary>
    /// 窗体关闭事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void Form_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (!TokenSource.IsCancellationRequested)
        {
            TokenSource.Cancel();
        }
        if (e.CloseReason == CloseReason.UserClosing)
        {
            HostWindow = null;
            Application.Exit();
        }
    }

    #endregion

    #region 抽象方法

    /// <summary>
    /// 工作区间
    /// </summary>
    /// <param name="cancellationToken"></param>
    protected abstract Task DoWorkAsync(CancellationToken cancellationToken);

    /// <summary>
    /// 工作异常
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task DoExceptionAsync(Exception ex, CancellationToken cancellationToken);

    /// <summary>
    /// 工作取消
    /// </summary>
    /// <returns></returns>
    protected abstract Task DoCanceledExceptionAsync(OperationCanceledException ex);

    #endregion

    #region 可继承方法

    #region 在UI线程上异步执行Action

    /// <summary>
    /// 在UI线程上异步执行Action
    /// </summary>
    /// <param name="action"></param>
    protected void InvokeOnUIThread(Action action)
    {
        if (HostWindow == null || HostWindow.IsDisposed) return;

        if (HostWindow.InvokeRequired)
        {
            HostWindow.Invoke(new System.Windows.Forms.MethodInvoker(action));
        }
        else
        {
            action.Invoke();
        }
    }

    /// <summary>
    /// 在UI线程上异步执行Action
    /// </summary>
    /// <param name="method"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public object? InvokeOnUIThread(Delegate method, params object[] args)
    {
        if (HostWindow == null || HostWindow.IsDisposed) return default;

        if (HostWindow.InvokeRequired)
        {
            return HostWindow.Invoke(method, args);
        }

        return method.DynamicInvoke(args);
    }

    /// <summary>
    /// 在UI线程上异步执行Action
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="method"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public T? InvokeOnUIThread<T>(Delegate method, params object[] args)
    {
        if (HostWindow == null || HostWindow.IsDisposed) return default;

        if (HostWindow.InvokeRequired)
        {
            return (T?)HostWindow.Invoke(method, args);
        }

        return (T?)method.DynamicInvoke(args);
    }

    /// <summary>
    /// 在UI线程上异步执行Action
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="method"></param>
    /// <returns></returns>
    public T? InvokeOnUIThread<T>(Func<T> method)
    {
        if (HostWindow == null || HostWindow.IsDisposed) return default;

        if (HostWindow.InvokeRequired)
        {
            return HostWindow.Invoke(method);
        }

        return method.Invoke();
    }

    #endregion

    #region 显示指定窗体

    /// <summary>
    /// 显示指定窗体
    /// </summary>
    /// <param name="owner"></param>
    protected void ShowForm(BaseForm owner)
    {
        InvokeOnUIThread(() =>
        {
            HostWindow?.Hide();

            owner.HostWindow?.Show();

            owner.HostWindow?.Activate();
        });
    }

    #endregion

    #region 纯文本输出

    /// <summary>
    /// 纯文本输出
    /// </summary>
    /// <param name="text"></param>
    protected void AppendText(string? text)
    {
        if (HostWindow == null || HostWindow.IsDisposed) return;

        InvokeOnUIThread(() =>
        {
            if (HostWindow.Controls.Find("Message", false).FirstOrDefault() is not Label Message)
            {
                var titleHeight = HostWindow.Height - HostWindow.ClientRectangle.Height;
                Message = new Label
                {
                    Name = "Message",
                    ForeColor = Color.Gray,
                    Width = HostWindow.Width,
                    Height = HostWindow.Height - titleHeight,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                HostWindow.Controls.Add(Message);
            }
            Message.Text = text;
        });
    }

    #endregion

    #region 列表日志输出

    /// <summary>
    /// 列表日志输出
    /// </summary>
    /// <param name="text"></param>
    /// <param name="color"></param>
    protected void AppendBox(string text, Color? color = null)
    {
        if (HostWindow == null || HostWindow.IsDisposed) return;

        InvokeOnUIThread(() =>
        {
            var bottom = 0;
            var offset = 0;
            if (HostWindow.Controls.Find("Menu", false).FirstOrDefault() is MenuStrip Menu)
            {
                offset = Menu.Height;
            }
            if (HostWindow.Controls.Find("StatusStrip", false).FirstOrDefault() is StatusStrip Status)
            {
                bottom = Status.Height;
            }
            var width = HostWindow.Width;
            var height = HostWindow.ClientRectangle.Height - offset - bottom;
            var location = new Point(0, offset);
            BoxControl = HostWindow.Controls;
            if (HostWindow.Controls.Find("Tab", false).FirstOrDefault() is TabControl Tab)
            {
                if (Tab.TabPages["TabPageBox"] is TabPage TabPageBox)
                {
                    BoxControl = TabPageBox.Controls;
                    width = TabPageBox.Width;
                    height = TabPageBox.Height;
                    location = new Point(0, 0);
                }
            }
            if (HostWindow.Controls.Find("Split", false).FirstOrDefault() is SplitContainer Split)
            {
                BoxControl = Split.Panel2.Controls;
                width = Split.Panel2.Width;
                height = Split.Panel2.Height;
                location = new Point(0, 0);
            }
            if (BoxControl.Find("Box", false).FirstOrDefault() is not RichTextBox Box)
            {
                Box = new RichTextBox
                {
                    Name = "Box",
                    Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left,
                    Width = width,
                    Height = height,
                    ReadOnly = true,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    Location = location,
                };
                BoxControl.Add(Box);
            }
            Box.SelectionStart = Box.TextLength;
            Box.SelectionLength = 0;
            Box.SelectionColor = color ?? Color.Black;
            Box.AppendText($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}：{text}\r\n");
            Box.SelectionColor = Box.ForeColor;
            Box.ScrollToCaret();
            Box.Focus();
        });
    }

    #endregion

    #region 表格数据显示

    /// <summary>
    /// 表格数据显示
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="title"></param>
    protected void AppendDatagridView<T>(List<T> list, string? title = null)
    {
        if (HostWindow == null || HostWindow.IsDisposed) return;

        InvokeOnUIThread(() =>
        {
            if (HostWindow.Controls.Find("Tab", false).FirstOrDefault() is TabControl Tab)
            {
                if (Tab.TabPages["TabPageHome"] is TabPage TabPageHome)
                {
                    title ??= $"总记录数：{list.Count}";
                    var offset = 5;
                    if (TabPageHome.Controls.Find("DatagridViewTitle", false).FirstOrDefault() is not Label DatagridViewTitle)
                    {
                        DatagridViewTitle = new Label
                        {
                            Name = "DatagridViewTitle",
                            Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left,
                            Text = title,
                            Width = TabPageHome.Width,
                            Location = new Point(0, offset),
                        };
                        TabPageHome.Controls.Add(DatagridViewTitle);
                        offset += DatagridViewTitle.Height + offset;
                    }
                    DatagridViewTitle.Text = title;
                    if (TabPageHome.Controls.Find("DatagridView", false).FirstOrDefault() is not DataGridView DataGridView)
                    {
                        DataGridView = new DataGridView
                        {
                            Name = "DatagridView",
                            Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left,
                            Width = TabPageHome.Width,
                            Height = TabPageHome.Height - offset,
                            Location = new Point(0, offset),
                            BackgroundColor = Color.White,
                            AllowUserToAddRows = false,
                            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                        };
                        DataGridView.RowPostPaint += (sender, e) =>
                        {
                            Rectangle rectangle = new(e.RowBounds.Location.X, e.RowBounds.Location.Y, DataGridView.RowHeadersWidth - 4, e.RowBounds.Height);
                            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), DataGridView.RowHeadersDefaultCellStyle.Font, rectangle, DataGridView.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
                        };
                        foreach (var property in typeof(T).GetProperties())
                        {
                            var headerText = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? property.Name;
                            DataGridView.Columns.Add(new DataGridViewColumn
                            {
                                HeaderText = headerText,
                                DataPropertyName = property.Name,
                                CellTemplate = new DataGridViewTextBoxCell(),
                            });
                        }
                        TabPageHome.Controls.Add(DataGridView);
                    }
                    DataGridView.DataSource = list;
                }
            }
        });
    }

    #endregion

    #region 二维码输出

    /// <summary>
    /// 二维码输出
    /// </summary>
    /// <param name="url">二维码url</param>
    /// <param name="bytes">二维码byte[]</param>
    /// <param name="size">二维码大小</param>
    /// <param name="text">提示文本</param>
    protected void AppendQRCode(string? url = null, byte[]? bytes = null, int size = 300, string text = "用 [ 微信 ] 扫一扫")
    {
        if (HostWindow == null || HostWindow.IsDisposed) return;

        if (url is null && bytes is null) return;

        InvokeOnUIThread(() =>
        {
            if (HostWindow.Controls.Find("Message", false).FirstOrDefault() is Label Message)
            {
                HostWindow.Controls.Remove(Message);
            }
            if (HostWindow.Controls.Find("QRCode", false).FirstOrDefault() is not PictureBox QRCode)
            {
                QRCode = new PictureBox
                {
                    Name = "QRCode",
                    Width = size,
                    Height = size,
                    BackColor = Color.White,
                    Location = new Point((HostWindow.Width - size) / 2, (HostWindow.Height - size) / 2 - 39),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                HostWindow.Controls.Add(QRCode);
            }
            if (HostWindow.Controls.Find("QRCodeMessage", false).FirstOrDefault() is not Label QRCodeMessage)
            {
                QRCodeMessage = new Label
                {
                    Name = "QRCodeMessage",
                    Font = new Font(FontFamily.GenericSansSerif, 10),
                    ForeColor = Color.Gray,
                    Width = HostWindow.Width,
                    Location = new Point(0, HostWindow.Height - (int)(90 * ScalingFactor)),
                    TextAlign = ContentAlignment.BottomCenter
                };
                HostWindow.Controls.Add(QRCodeMessage);
            }
            if (url is not null)
            {
                QRCode.ImageLocation = url;
            }
            else if (bytes is not null)
            {
                QRCode.Image = Image.FromStream(new MemoryStream(bytes));
            }
            QRCodeMessage.Text = text;
        });
    }

    #endregion

    #region 序列化

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="value">对象值</param>
    /// <param name="namingStrategy">命名策略：0 默认，1 小驼峰，2 蛇形</param>
    /// <param name="dateFormat">时间格式，默认：yyyy-MM-dd HH:mm:ss</param>
    /// <returns></returns>
    protected virtual string JsonSerializeObject(object? value, int namingStrategy = 0, string dateFormat = "yyyy-MM-dd HH:mm:ss")
    {
        var settings = new JsonSerializerSettings
        {
            DateFormatString = dateFormat,
            ContractResolver = namingStrategy switch
            {
                0 => new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                },
                1 => new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                2 => new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                _ => new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                },
            }
        };

        var json = JsonConvert.SerializeObject(value, settings);

        return json;
    }

    #endregion

    #region 反序列化

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="json">JSON字符串</param>
    /// <param name="namingStrategy">命名策略：0 默认，1 小驼峰，2 蛇形</param>
    /// <param name="dateFormat">时间格式，默认：yyyy-MM-dd HH:mm:ss</param>
    /// <returns></returns>
    protected virtual T? JsonDeserializeObject<T>(string json, int namingStrategy = 0, string dateFormat = "yyyy-MM-dd HH:mm:ss")
    {
        var settings = new JsonSerializerSettings
        {
            DateFormatString = dateFormat,
            ContractResolver = namingStrategy switch
            {
                0 => new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                },
                1 => new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                2 => new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                _ => new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                },
            }
        };

        var value = JsonConvert.DeserializeObject<T>(json, settings);

        return value;
    }

    #endregion

    #region 获取配置

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual T? GetConfigValue<T>(string key)
    {
        return Configuration.GetSection(key).Get<T>();
    }

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual string? GetConfigValue(string key)
    {
        return Configuration[key];
    }

    #endregion

    #region HtmlAgilityPack

    #region 创建HTML文档

    /// <summary>
    /// 创建HTML文档
    /// </summary>
    /// <param name="html"></param>
    protected static void CreateHtmlDocument(string html)
    {
        HtmlDocument = new HtmlDocument();

        HtmlDocument.LoadHtml(html);
    }

    #endregion

    #region 通过XPath查找单个元素

    /// <summary>
    /// 通过XPath查找单个元素
    /// </summary>
    /// <param name="xpath">xpath</param>
    /// <returns></returns>
    protected static object? FindElementByXPath(string xpath)
    {
        return HtmlDocument?.DocumentNode.SelectSingleNode(xpath);
    }

    #endregion

    #region 通过XPath查找集合元素

    /// <summary>
    /// 通过XPath查找集合元素
    /// </summary>
    /// <param name="xpath">xpath</param>
    /// <returns></returns>
    protected static List<object> FindElementsByXPath(string xpath)
    {
        return HtmlDocument?.DocumentNode.SelectNodes(xpath)?.ToList<object>() ?? [];
    }

    #endregion

    #region 通过XPath检查元素是否存在

    /// <summary>
    /// 通过XPath检查元素是否存在
    /// </summary>
    /// <param name="xpath">xpath</param>
    /// <returns></returns>
    protected static bool IsElementExist(string xpath)
    {
        return (HtmlDocument?.DocumentNode.SelectNodes(xpath)?.Count ?? 0) != 0;
    }

    #endregion

    #region 通过XPath查找指定元素对象的单个元素

    /// <summary>
    /// 通过XPath查找指定元素对象的单个元素
    /// </summary>
    /// <param name="element">元素对象</param>
    /// <param name="xpath">xpath</param>
    /// <returns></returns>
    protected static object? FindElementByXPath(object? element, string xpath)
    {
        return (element as HtmlNode)?.SelectSingleNode(xpath);
    }

    #endregion

    #region 通过XPath查找指定元素对象的集合元素

    /// <summary>
    /// 通过XPath查找指定元素对象的集合元素
    /// </summary>
    /// <param name="element">元素对象</param>
    /// <param name="xpath">xpath</param>
    /// <returns></returns>
    protected static List<object> FindElementsByXPath(object? element, string xpath)
    {
        return (element as HtmlNode)?.SelectNodes(xpath)?.ToList<object>() ?? [];
    }

    #endregion

    #region 通过XPath检查指定元素对象的元素是否存在

    /// <summary>
    /// 通过XPath检查指定元素对象的元素是否存在
    /// </summary>
    /// <param name="element">元素对象</param>
    /// <param name="xpath">xpath</param>
    /// <returns></returns>
    protected static bool IsElementExist(object? element, string xpath)
    {
        return ((element as HtmlNode)?.SelectNodes(xpath)?.Count ?? 0) != 0;
    }

    #endregion

    #region 查找元素对象的文本值

    /// <summary>
    /// 查找元素对象的文本值
    /// </summary>
    /// <param name="element">元素对象</param>
    /// <returns></returns>
    protected static string FindText(object? element)
    {
        return (element as HtmlNode)?.InnerText ?? string.Empty;
    }

    #endregion

    #region 查找元素对象的隐藏文本值

    /// <summary>
    /// 查找元素对象的隐藏文本值
    /// </summary>
    /// <param name="element">元素对象</param>
    /// <returns></returns>
    protected static string FindHiddenText(object? element)
    {
        return (element as HtmlNode)?.GetAttributeValue("textContent", "") ?? string.Empty;
    }

    #endregion

    #region 查找元素对象的属性值

    /// <summary>
    /// 查找元素对象的属性值
    /// </summary>
    /// <param name="element">元素对象</param>
    /// <param name="attribute">属性名称</param>
    /// <returns></returns>
    protected static string FindAttributeValue(object? element, string attribute)
    {
        return (element as HtmlNode)?.GetAttributeValue(attribute, "") ?? string.Empty;
    }

    #endregion

    #region 去首尾空格换行制表符

    /// <summary>
    /// 去首尾空格换行制表符
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    protected static string Trim(string text)
    {
        return text.Trim([' ', '\t', '\r', '\n']);
    }

    #endregion

    #endregion

    #region 创建雪花Id

    /// <summary>
    /// 创建雪花Id
    /// </summary>
    /// <returns></returns>
    protected static long CreateNextId()
    {
        return YitIdHelper.NextId();
    }

    /// <summary>
    /// 创建雪花Id
    /// </summary>
    /// <returns></returns>
    protected static string CreateNextIdString()
    {
        return YitIdHelper.NextId().ToString();
    }

    #endregion

    #region 停止工作

    /// <summary>
    /// 停止工作
    /// </summary>
    protected static void StopWork()
    {
        if (!TokenSource.IsCancellationRequested)
        {
            TokenSource.Cancel();
        }
        JobManager.Stop();
    }

    #endregion

    #region 开始工作

    /// <summary>
    /// 开始工作
    /// </summary>
    protected static void StartWork()
    {
        if (TokenSource.IsCancellationRequested)
        {
            TokenSource = new();
        }
        JobManager.Start();
    }

    #endregion

    #endregion
}
