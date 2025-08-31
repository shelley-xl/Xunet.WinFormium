namespace Xunet.WinFormium.Simples.Windows;

using SuperDriver;
using SuperDriver.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xunet.WinFormium.Windows;

/// <summary>
/// 主窗体
/// </summary>
public class QRCodeForm : BaseForm
{
    /// <summary>
    /// 标题
    /// </summary>
    protected override string BaseText => $"测试 - {Version}";

    /// <summary>
    /// 窗体边框样式
    /// </summary>
    protected override FormBorderStyle BaseFormBorderStyle => FormBorderStyle.FixedSingle;

    /// <summary>
    /// 是否启用最大化控件
    /// </summary>
    protected override bool BaseMaximizeBox => false;

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
    /// <exception cref="NotImplementedException"></exception>
    protected override async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        AppendText("正在初始化，请稍后 ...");

        var result = Bootstrap.Initialize();

        if (result.Success)
        {
            AppendText("正在加载二维码，请稍后 ...", ColorTranslator.FromHtml("#1296db"));

            var alipay = AlipayService.CreateDefaultService();

            var code = alipay.LoadQRCode();

            if (code.Success)
            {
                AppendQRCode(code.QRCodeBytes, text: "用 [ 支付宝 ] 扫一扫");

                while (!cancellationToken.IsCancellationRequested)
                {
                    var login = alipay.CheckLogin();

                    if (login.Success)
                    {
                        var order = alipay.GetOrder(new AlipayGetOrderRequest
                        {
                            FundFlow = FundFlowConst.In,
                        });
                        if (order.Success && !alipay.CheckSecurity())
                        {
                            //var orders = order.Data;
                        }
                        break;
                    }

                    await Task.Delay(2000, cancellationToken);
                }
            }
            else
            {
                AppendText(code.Message);
            }
        }
        else
        {
            AppendText(result.Message);
        }

        await Task.CompletedTask;
    }
}
