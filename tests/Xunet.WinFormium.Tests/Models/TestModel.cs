namespace Xunet.WinFormium.Tests.Models;

using SqlSugar;

[SugarTable("test")]
public class TestModel
{
    [SugarColumn(IsPrimaryKey = true)]
    public string? Id { get; set; }

    public string? UserName { get; set; }

    public DateTime? CreateTime { get; set; }
}
