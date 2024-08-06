namespace Xunet.WinFormium;

using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using FluentScheduler;
using SqlSugar;
using Xunet.WinFormium.Core;
using Yitter.IdGenerator;

/// <summary>
/// 窗体基类
/// </summary>
public abstract class BaseForm : Form
{
    #region 字段

    #region 私有

    /// <summary>
    /// 配置
    /// </summary>
    /// <returns></returns>
    IConfigurationRoot Configuration { get; } = DependencyResolver.Current?.GetRequiredService<IConfigurationRoot>() ?? throw new InvalidOperationException("IConfigurationRoot Get Failed.");

    #endregion

    #region 只读

    /// <summary>
    /// 版本号
    /// </summary>
    protected string Version { get; } = $"v{Assembly.GetExecutingAssembly().GetName().Version}";

    /// <summary>
    /// 默认请求客户端
    /// </summary>
    protected HttpClient DefaultClient { get; } = DependencyResolver.Current?.GetRequiredService<IHttpClientFactory>()?.CreateClient("default") ?? throw new InvalidOperationException("DefaultClient Create Failed.");

    /// <summary>
    /// 数据库访问
    /// </summary>
    protected ISqlSugarClient Db { get; } = DependencyResolver.Current?.GetRequiredService<ISqlSugarClient>() ?? throw new InvalidOperationException("DbContext Get Failed.");

    #endregion

    #region 读写

    /// <summary>
    /// 多线程TokenSource
    /// </summary>
    protected CancellationTokenSource TokenSource { get; set; } = new();

    #endregion

    #region 静态

    /// <summary>
    /// 雪花Id
    /// </summary>
    protected static string NextIdString => YitIdHelper.NextId().ToString();

    #endregion

    #region 抽象

    /// <summary>
    /// 标题
    /// </summary>
    protected abstract string BaseText { get; }

    #endregion

    #region 虚属性

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
    public BaseForm()
    {
        InitializeComponent();
        InitializeControl();
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

    /// <summary>
    /// 窗体加载事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void Form_Load(object? sender, EventArgs e)
    {
        if (BaseDoWorkInterval > 0)
        {
            JobManager.AddJob(() => DoWork(TokenSource.Token), schedule => schedule.ToRunNow().AndEvery(BaseDoWorkInterval).Seconds());
        }
        else
        {
            DoWork(TokenSource.Token);
        }
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
        Application.Exit();
    }

    #endregion

    #region 虚方法

    #region 工作区间

    /// <summary>
    /// 工作区间
    /// </summary>
    /// <param name="cancellationToken"></param>
    protected virtual void DoWork(CancellationToken cancellationToken)
    {
        // TODO
    }

    #endregion

    #region 纯文本输出

    /// <summary>
    /// 纯文本输出
    /// </summary>
    /// <param name="form"></param>
    /// <param name="text"></param>
    protected virtual void AppendText(Form form, string? text)
    {
        form.BeginInvoke(new Action(() =>
        {
            form.Controls.Clear();
            form.Controls.Add(new Label
            {
                Name = "Message",
                Text = text,
                ForeColor = Color.Gray,
                Width = Width,
                Height = Height - 30,
                TextAlign = ContentAlignment.MiddleCenter
            });
        }));
    }

    #endregion

    #region 列表日志输出

    /// <summary>
    /// 列表日志输出
    /// </summary>
    /// <param name="form"></param>
    /// <param name="text"></param>
    /// <param name="color"></param>
    protected virtual void AppendBox(Form form, string text, Color? color = null)
    {
        form.BeginInvoke(new Action(() =>
        {
            if (form.Controls.Find("Box", false).FirstOrDefault() is not RichTextBox box)
            {
                var offset = form.Controls.Find("Menu", false).FirstOrDefault()?.Height ?? 0;
                var titleHeight = form.Height - form.ClientRectangle.Height;
                box = new RichTextBox
                {
                    Name = "Box",
                    Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left,
                    Width = Width,
                    Height = Height - offset - titleHeight,
                    ReadOnly = true,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    Location = new Point(0, offset),
                };
                form.Controls.Add(box);
            }
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color ?? Color.Black;
            box.AppendText($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}：{text}\r\n");
            box.SelectionColor = box.ForeColor;
            box.ScrollToCaret();
            box.Focus();
        }));
    }

    #endregion

    #region 二维码输出

    /// <summary>
    /// 二维码输出
    /// </summary>
    /// <param name="form"></param>
    /// <param name="url"></param>
    protected virtual void AppendQRCode(Form form, string? url)
    {
        form.BeginInvoke(new Action(() =>
        {
            form.Controls.Clear();
            form.Controls.Add(new PictureBox
            {
                Name = "QRCode",
                ImageLocation = url,
                Width = 300,
                Height = 300,
                BackColor = Color.White,
                Location = new Point(50, 25),
                SizeMode = PictureBoxSizeMode.StretchImage
            });
            form.Controls.Add(new Label
            {
                Name = "Message",
                Text = "用 [ 微信 ] 扫一扫",
                Font = new Font(FontFamily.GenericSansSerif, 10),
                ForeColor = Color.Gray,
                Width = Width,
                Location = new Point(0, 345),
                TextAlign = ContentAlignment.BottomCenter
            });
        }));
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

    #endregion
}
