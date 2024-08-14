// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium;

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
    protected static CancellationTokenSource TokenSource { get; set; }

    #endregion

    #region 私有

    /// <summary>
    /// HTML文档
    /// </summary>
    static HtmlDocument? HtmlDocument { get; set; }

    /// <summary>
    /// 当前窗体
    /// </summary>
    Form? HostWindow { get; set; }

    #endregion

    #region 只读

    /// <summary>
    /// 版本号
    /// </summary>
    protected static string Version { get; }

    /// <summary>
    /// 默认请求客户端
    /// </summary>
    protected static HttpClient DefaultClient { get; }

    /// <summary>
    /// 数据库访问
    /// </summary>
    protected static ISqlSugarClient Db { get; }

    #endregion

    #region 抽象

    /// <summary>
    /// 标题
    /// </summary>
    protected abstract string BaseText { get; }

    #endregion

    #region 重写

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

        HostWindow = null;

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
    /// 配置
    /// </summary>
    static IConfigurationRoot Configuration { get; }

    static BaseForm()
    {
        TokenSource = new();
        Version = $"v{Assembly.GetEntryAssembly()?.GetName().Version}";
        Configuration = DependencyResolver.Current?.GetRequiredService<IConfigurationRoot>() ?? throw new InvalidOperationException("No service for type 'IConfigurationRoot' has been registered.");
        DefaultClient = DependencyResolver.Current?.GetRequiredService<IHttpClientFactory>()?.CreateClient("default") ?? throw new InvalidOperationException("No service for type 'IHttpClientFactory' has been registered.");
        Db = DependencyResolver.Current?.GetRequiredService<ISqlSugarClient>() ?? throw new InvalidOperationException("No service for type 'ISqlSugarClient' has been registered.");
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected BaseForm()
    {
        InitializeComponent();
        InitializeControl();

        HostWindow = this;
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

    async void DoWork()
    {
        using (await _async.WaitAsync())
        {
            try
            {
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
        }
    }

    /// <summary>
    /// 窗体加载事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Form_Load(object? sender, EventArgs e)
    {
        Task.Run(() =>
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
        });
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

        if (HostWindow!.InvokeRequired)
        {
            HostWindow!.Invoke(new System.Windows.Forms.MethodInvoker(action));
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

        if (HostWindow!.InvokeRequired)
        {
            return HostWindow!.Invoke(method, args);
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

        if (HostWindow!.InvokeRequired)
        {
            return (T?)HostWindow!.Invoke(method, args);
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

        if (HostWindow!.InvokeRequired)
        {
            return HostWindow!.Invoke(method);
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
            if (HostWindow.Controls.Find("Message", false).FirstOrDefault() is not Label label)
            {
                var titleHeight = HostWindow.Height - HostWindow.ClientRectangle.Height;
                label = new Label
                {
                    Name = "Message",
                    ForeColor = Color.Gray,
                    Width = HostWindow.Width,
                    Height = HostWindow.Height - titleHeight,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                HostWindow.Controls.Add(label);
            }
            label.Text = text;
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
            if (HostWindow.Controls.Find("Box", false).FirstOrDefault() is not RichTextBox box)
            {
                var offset = 0;
                if (HostWindow.Controls.Find("Menu", false).FirstOrDefault() is MenuStrip menu)
                {
                    offset = menu.Height;
                }
                var titleHeight = HostWindow.Height - HostWindow.ClientRectangle.Height;
                box = new RichTextBox
                {
                    Name = "Box",
                    Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left,
                    Width = HostWindow.Width,
                    Height = HostWindow.Height - offset - titleHeight,
                    ReadOnly = true,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    Location = new Point(0, offset),
                };
                HostWindow.Controls.Add(box);
            }
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color ?? Color.Black;
            box.AppendText($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}：{text}\r\n");
            box.SelectionColor = box.ForeColor;
            box.ScrollToCaret();
            box.Focus();
        });
    }

    #endregion

    #region 二维码输出

    /// <summary>
    /// 二维码输出
    /// </summary>
    /// <param name="url"></param>
    protected void AppendQRCode(string? url)
    {
        if (HostWindow == null || HostWindow.IsDisposed) return;

        InvokeOnUIThread(() =>
        {
            HostWindow.Controls.Clear();
            HostWindow.Controls.Add(new PictureBox
            {
                Name = "QRCode",
                ImageLocation = url,
                Width = 300,
                Height = 300,
                BackColor = Color.White,
                Location = new Point(50, 25),
                SizeMode = PictureBoxSizeMode.StretchImage
            });
            HostWindow.Controls.Add(new Label
            {
                Name = "Message",
                Text = "用 [ 微信 ] 扫一扫",
                Font = new Font(FontFamily.GenericSansSerif, 10),
                ForeColor = Color.Gray,
                Width = HostWindow.Width,
                Location = new Point(0, 345),
                TextAlign = ContentAlignment.BottomCenter
            });
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
        return (element as HtmlNode)?.GetAttributeValue("textContent", null) ?? string.Empty;
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
        return (element as HtmlNode)?.GetAttributeValue(attribute, null) ?? string.Empty;
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
