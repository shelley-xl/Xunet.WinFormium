namespace Xunet.WinFormium.Tests;

using Xunet.WinFormium.Tests.Models;

public class MainForm : BaseForm
{
    protected override string BaseText => $"测试 - {Version}";

    protected override Size BaseClientSize => new(600, 400);

    protected override int BaseDoWorkInterval => GetConfigValue<int>("DoWorkInterval");

    protected override void DoWork(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            try
            {
                AppendBox(this, "正在测试，请稍后 ...", ColorTranslator.FromHtml("#1296db"));

                foreach (var item in Enumerable.Range(0, 50))
                {
                    var model = new TestModel
                    {
                        Id = NextIdString,
                        UserName = "测试",
                        CreateTime = DateTime.Now
                    };

                    AppendBox(this, $"正在工作 {model.Id} ...");

                    await Db.Insertable(model).ExecuteCommandAsync();

                    await Task.Delay(new Random().Next(100, 500));
                }

                AppendBox(this, "测试完成！", ColorTranslator.FromHtml("#1296db"));
            }
            catch (Exception ex)
            {
                AppendBox(this, "系统错误！", Color.Red);
                AppendBox(this, ex.ToString(), Color.Red);
            }
        }, cancellationToken);
    }
}
