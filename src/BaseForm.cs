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
/// �������
/// </summary>
public abstract class BaseForm : Form
{
    #region �ֶ�

    #region ˽��

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    IConfigurationRoot Configuration { get; } = DependencyResolver.Current?.GetRequiredService<IConfigurationRoot>() ?? throw new InvalidOperationException("IConfigurationRoot Get Failed.");

    #endregion

    #region ֻ��

    /// <summary>
    /// �汾��
    /// </summary>
    protected string Version { get; } = $"v{Assembly.GetExecutingAssembly().GetName().Version}";

    /// <summary>
    /// Ĭ������ͻ���
    /// </summary>
    protected HttpClient DefaultClient { get; } = DependencyResolver.Current?.GetRequiredService<IHttpClientFactory>()?.CreateClient("default") ?? throw new InvalidOperationException("DefaultClient Create Failed.");

    /// <summary>
    /// ���ݿ����
    /// </summary>
    protected ISqlSugarClient Db { get; } = DependencyResolver.Current?.GetRequiredService<ISqlSugarClient>() ?? throw new InvalidOperationException("DbContext Get Failed.");

    #endregion

    #region ��д

    /// <summary>
    /// ���߳�TokenSource
    /// </summary>
    protected CancellationTokenSource TokenSource { get; set; } = new();

    #endregion

    #region ��̬

    /// <summary>
    /// ѩ��Id
    /// </summary>
    protected static string NextIdString => YitIdHelper.NextId().ToString();

    #endregion

    #region ����

    /// <summary>
    /// ����
    /// </summary>
    protected abstract string BaseText { get; }

    #endregion

    #region ������

    /// <summary>
    /// �Ƿ�������󻯿ؼ�
    /// </summary>
    protected virtual bool BaseMaximizeBox { get; } = true;

    /// <summary>
    /// ���屳��ɫ
    /// </summary>
    protected virtual Color BaseBackColor { get; } = DefaultBackColor;

    /// <summary>
    /// ����߿���ʽ
    /// </summary>
    protected virtual FormBorderStyle BaseFormBorderStyle { get; } = FormBorderStyle.Sizable;

    /// <summary>
    /// �����С
    /// </summary>
    protected virtual Size BaseClientSize { get; } = new Size(400, 400);

    /// <summary>
    /// ��������Ƶ�ʣ���λ���룩������ 0 ʱ������һ��
    /// </summary>
    protected virtual int BaseDoWorkInterval { get; } = 0;

    #endregion

    #endregion

    #region �����

    /// <summary>
    ///  �������Ʊ�����
    /// </summary>
    readonly IContainer? components = null;

    /// <summary>
    ///  ������������ʹ�õ���Դ��
    /// </summary>
    /// <param name="disposing">���Ӧ�����й���Դ����Ϊ true������Ϊ false��</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows������������ɵĴ���

    /// <summary>
    ///  �����֧������ķ���-��Ҫ�޸�
    ///  ʹ�ô���༭���޸Ĵ˷��������ݡ�
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

    #region ���캯��

    /// <summary>
    /// ���캯��
    /// </summary>
    public BaseForm()
    {
        InitializeComponent();
        InitializeControl();
    }

    #endregion

    #region ��ʼ���ؼ�

    /// <summary>
    /// ��ʼ���ؼ�
    /// </summary>
    protected virtual void InitializeControl()
    {

    }

    #endregion

    #region ��������¼�

    /// <summary>
    /// ��������¼�
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

    #region ����ر��¼�

    /// <summary>
    /// ����ر��¼�
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

    #region �鷽��

    #region ��������

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="cancellationToken"></param>
    protected virtual void DoWork(CancellationToken cancellationToken)
    {
        // TODO
    }

    #endregion

    #region ���ı����

    /// <summary>
    /// ���ı����
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

    #region �б���־���

    /// <summary>
    /// �б���־���
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
            box.AppendText($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}��{text}\r\n");
            box.SelectionColor = box.ForeColor;
            box.ScrollToCaret();
            box.Focus();
        }));
    }

    #endregion

    #region ��ά�����

    /// <summary>
    /// ��ά�����
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
                Text = "�� [ ΢�� ] ɨһɨ",
                Font = new Font(FontFamily.GenericSansSerif, 10),
                ForeColor = Color.Gray,
                Width = Width,
                Location = new Point(0, 345),
                TextAlign = ContentAlignment.BottomCenter
            });
        }));
    }

    #endregion

    #region ���л�

    /// <summary>
    /// ���л�
    /// </summary>
    /// <param name="value">����ֵ</param>
    /// <param name="namingStrategy">�������ԣ�0 Ĭ�ϣ�1 С�շ壬2 ����</param>
    /// <param name="dateFormat">ʱ���ʽ��Ĭ�ϣ�yyyy-MM-dd HH:mm:ss</param>
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

    #region �����л�

    /// <summary>
    /// �����л�
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <param name="json">JSON�ַ���</param>
    /// <param name="namingStrategy">�������ԣ�0 Ĭ�ϣ�1 С�շ壬2 ����</param>
    /// <param name="dateFormat">ʱ���ʽ��Ĭ�ϣ�yyyy-MM-dd HH:mm:ss</param>
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

    #region ��ȡ����

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual T? GetConfigValue<T>(string key)
    {
        return Configuration.GetSection(key).Get<T>();
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual string? GetConfigValue(string key)
    {
        return GetConfigStringValue(key);
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual string? GetConfigStringValue(string key)
    {
        return Configuration[key];
    }

    #endregion

    #endregion
}
