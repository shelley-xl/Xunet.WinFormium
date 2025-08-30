namespace Xunet.WinFormium.Simples.Entities;

using SuperSpider;

/// <summary>
/// 微博热搜
/// </summary>
[SpiderSchema("weibo", "微博热搜")]
[SpiderIndex("unique_weibo_Keywords", nameof(Keywords), true, true)]
[EntitySelector(Expression = "//*[@id=\"pl_top_realtimehot\"]/table/tbody/tr")]
public class WeiboEntity : SpiderEntity
{
    /// <summary>
    /// 排行
    /// </summary>
    [SpiderColumn(ColumnDescription = "排行", IsNullable = true)]
    [RegexFormatter(Pattern = "[0-9]+")]
    [ValueSelector(Expression = "//td[1]")]
    public int? RankTop { get; set; }

    /// <summary>
    /// 关键词
    /// </summary>
    [SpiderColumn(ColumnDescription = "关键词", IsNullable = true)]
    [ValueSelector(Expression = "//td[2]/a")]
    public string? Keywords { get; set; }

    /// <summary>
    /// HotText
    /// </summary>
    [SpiderColumn(ColumnDescription = "热度文本", IsNullable = true)]
    [RegexFormatter(Pattern = "[\u4E00-\u9FA5]+")]
    [ValueSelector(Expression = "//td[2]/span")]
    public string? HotText { get; set; }

    /// <summary>
    /// 热度值
    /// </summary>
    [SpiderColumn(ColumnDescription = "热度值", IsNullable = true)]
    [RegexFormatter(Pattern = "[0-9]+")]
    [ValueSelector(Expression = "//td[2]/span")]
    public int? HotValue { get; set; }

    /// <summary>
    /// 热度标签
    /// </summary>
    [SpiderColumn(ColumnDescription = "热度标签", IsNullable = true)]
    [ValueSelector(Expression = "//td[3]/i")]
    public string? HotTag { get; set; }

    /// <summary>
    /// 链接
    /// </summary>
    [SpiderColumn(ColumnDescription = "链接", IsNullable = true)]
    [ValueSelector(Expression = "//td[2]/a/@href")]
    public string? Url { get; set; }
}
