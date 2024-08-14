// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium.Core;

/// <summary>
/// 属性管理器
/// </summary>
public class PropertyManager
{
    readonly Dictionary<string, Property> settings = [];

    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void SetValue(string name, object value)
    {
        settings[name] = new Property(value, value.GetType());
    }

    /// <summary>
    /// 设置值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void SetValue<T>(string name, T value) where T : notnull
    {
        settings[name] = new Property(value, typeof(T));
    }

    /// <summary>
    /// 获取值
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public object? GetValue(string name)
    {
        if (settings.TryGetValue(name, out var setting))
        {
            return setting.Value;
        }

        return null;
    }

    /// <summary>
    /// 获取值
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public Property GetProperty(string name)
    {
        if (settings.TryGetValue(name, out var setting))
        {
            return setting;
        }

        throw new KeyNotFoundException();
    }

    /// <summary>
    /// 获取值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T? GetValue<T>(string name)
    {
        if (settings.TryGetValue(name, out var setting))
        {
            return (T)setting.Value;
        }

        return default;
    }

    /// <summary>
    /// 移除值
    /// </summary>
    /// <param name="name"></param>
    public void Remove(string name)
    {
        settings.Remove(name);
    }

    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool Exists(string name)
    {
        return settings.ContainsKey(name);
    }
}

/// <summary>
/// 属性
/// </summary>
/// <param name="value"></param>
/// <param name="valueType"></param>
public sealed class Property(object value, Type valueType)
{
    /// <summary>
    /// 值
    /// </summary>
    public object Value { get; set; } = value;

    /// <summary>
    /// 值类型
    /// </summary>
    public Type ValueType { get; set; } = valueType;
}
