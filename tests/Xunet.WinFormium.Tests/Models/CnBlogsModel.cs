namespace Xunet.WinFormium.Tests.Models;

using SqlSugar;

/// <summary>
/// CnBlogsModel
/// </summary>
[SugarTable("cnblogs")]
public class CnBlogsModel
{
    /// <summary>
    /// Id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true)]
    public string? Id { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Url
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Summary
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// CreateTime
    /// </summary>
    public DateTime? CreateTime { get; set; }
}
