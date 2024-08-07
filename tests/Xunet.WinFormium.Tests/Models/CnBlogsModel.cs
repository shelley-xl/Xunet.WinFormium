namespace Xunet.WinFormium.Tests.Models;

using SqlSugar;

[SugarTable("cnblogs")]
public class CnBlogsModel
{
    [SugarColumn(IsPrimaryKey = true)]
    public string? Id { get; set; }

    public string? Title { get; set; }

    public string? Url { get; set; }

    public string? Summary { get; set; }

    public DateTime? CreateTime { get; set; }
}
