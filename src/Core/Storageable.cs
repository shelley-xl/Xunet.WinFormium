namespace Xunet.WinFormium.Core;

/// <summary>
/// 数据存储
/// </summary>
public class Storageable
{
    /// <summary>
    /// 存储名称
    /// </summary>
    public string? StorageName { get; set; } = "Default";

    /// <summary>
    /// 实体表
    /// </summary>
    public Type[]? EntityTypes { get; set; } = [];
}
