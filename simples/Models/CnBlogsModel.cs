namespace Xunet.WinFormium.Simples.Models;

using SqlSugar;
using System.ComponentModel;

/// <summary>
/// CnBlogsModel
/// </summary>
[SugarTable("cnblogs")]
public class CnBlogsModel
{
    /// <summary>
    /// Id
    /// </summary>
    [Description("编号")]
    [SugarColumn(IsPrimaryKey = true)]
    public string? Id { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    [Description("标题")]
    public string? Title { get; set; }

    /// <summary>
    /// Url
    /// </summary>
    [Description("地址")]
    public string? Url { get; set; }

    /// <summary>
    /// Summary
    /// </summary>
    [Description("摘要")]
    public string? Summary { get; set; }

    /// <summary>
    /// CreateTime
    /// </summary>
    [Description("创建时间")]
    public DateTime? CreateTime { get; set; }
}
