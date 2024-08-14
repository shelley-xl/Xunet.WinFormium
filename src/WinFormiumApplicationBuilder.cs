// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium;

using Microsoft.Extensions.DependencyInjection;
using System.Text;

/// <summary>
/// WinFormiumApplicationBuilder
/// </summary>
public sealed class WinFormiumApplicationBuilder
{
    /// <summary>
    /// Services
    /// </summary>
    public IServiceCollection Services { get; }

    internal WinFormiumApplicationBuilder()
    {
        Services = new ServiceCollection();

        // 设置未处理异常的模式为捕获并且 ThrowException
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        // 处理UI线程异常
        Application.ThreadException += Application_ThreadException;
        // 处理非UI线程异常
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        // ApplicationConfiguration.Initialize();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
    }

    /// <summary>
    /// Build
    /// </summary>
    /// <returns></returns>
    public WinFormiumApplication Build()
    {
        Services.AddSingleton(provider =>
        {
            return new WinFormiumApplication(Services);
        });

        return Services.BuildServiceProvider().GetRequiredService<WinFormiumApplication>();
    }

    static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
        MessageBox.Show(GetExceptionMsg(e.Exception, e.ToString()), "出错啦", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        MessageBox.Show(GetExceptionMsg(e.ExceptionObject as Exception, e.ToString()), "出错啦", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    static string GetExceptionMsg(Exception? ex, string? backStr)
    {
        var sb = new StringBuilder();
        sb.AppendLine("****************************异常文本****************************");
        sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
        if (ex != null)
        {
            sb.AppendLine("【异常类型】：" + ex.GetType().Name);
            sb.AppendLine("【异常信息】：" + ex.Message);
            sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
        }
        else
        {
            sb.AppendLine("【未处理异常】：" + backStr);
        }
        sb.AppendLine("***************************************************************");
        return sb.ToString();
    }
}
