// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

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
