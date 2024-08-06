namespace Xunet.WinFormium.Core;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 依赖解析器
/// </summary>
public class DependencyResolver
{
    static DependencyResolver? _resolver;

    readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 私有构造函数
    /// </summary>
    /// <param name="serviceProvider"></param>
    DependencyResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 当前解析器
    /// </summary>
    public static DependencyResolver? Current => _resolver ?? throw new Exception("DependencyResolver not initialized. You should initialize it first.");

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="services"></param>
    public static void Initialize(IServiceProvider services)
    {
        _resolver = new DependencyResolver(services);
    }

    /// <summary>
    /// 获取Service
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    public object? GetRequiredService(Type serviceType)
    {
        return _serviceProvider.GetRequiredService(serviceType);
    }

    /// <summary>
    /// 获取Service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetRequiredService<T>() where T : notnull
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}
