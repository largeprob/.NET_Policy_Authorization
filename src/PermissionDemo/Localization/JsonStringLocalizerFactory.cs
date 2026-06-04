using Microsoft.Extensions.Localization;

namespace PermissionDemo.Localization;

/// <summary>
///  JSON 本地化器工厂。统一指向 Localization/Resources 目录，
///  为每次请求创建 <see cref="JsonStringLocalizer"/> 实例。
/// </summary>
public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    /// <summary>JSON 资源文件根目录（基于内容根路径拼接）。</summary>
    private readonly string _resourcesPath;

    public JsonStringLocalizerFactory(IWebHostEnvironment env)
    {
        _resourcesPath = Path.Combine(env.ContentRootPath, "Localization", "Resources");
    }

    /// <summary>
    ///  按资源类型创建本地化器（本实现忽略具体类型，统一读取同一资源目录）。
    /// </summary>
    public IStringLocalizer Create(Type resourceSource)
    {
        return new JsonStringLocalizer(_resourcesPath);
    }

    /// <summary>
    ///  按基础名/位置创建本地化器（本实现忽略入参，统一读取同一资源目录）。
    /// </summary>
    public IStringLocalizer Create(string baseName, string location)
    {
        return new JsonStringLocalizer(_resourcesPath);
    }
}
