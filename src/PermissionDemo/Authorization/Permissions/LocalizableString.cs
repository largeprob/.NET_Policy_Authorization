using Microsoft.Extensions.Localization;

namespace PermissionDemo.Authorization.Permissions;

/// <summary>
///  可本地化字符串，存储资源类型和资源键，在需要显示时才解析为对应语言的文本（延迟本地化）。
///  这样权限定义阶段只需记录"资源键"，真正的多语言文本在渲染时根据当前区域文化动态解析。
/// </summary>
public class LocalizableString
{
    /// <summary>
    ///  资源类型，对应一组语言资源文件（例如 PermissionResource）。
    /// </summary>
    public Type ResourceType { get; }

    /// <summary>
    ///  资源键，即在语言资源文件（JSON）中查找文本时使用的键名。
    /// </summary>
    public string Name { get; }

    private LocalizableString(Type resourceType, string name)
    {
        ResourceType = resourceType;
        Name = name;
    }

    /// <summary>
    ///  创建一个可本地化字符串。
    /// </summary>
    /// <typeparam name="TResource">资源类型（标记类）</typeparam>
    /// <param name="name">资源键</param>
    public static LocalizableString Create<TResource>(string name)
    {
        return new LocalizableString(typeof(TResource), name);
    }

    /// <summary>
    ///  使用本地化工厂将当前资源键解析为对应当前区域文化的文本。
    /// </summary>
    /// <param name="factory">字符串本地化工厂</param>
    /// <returns>解析后的本地化文本</returns>
    public string Localize(IStringLocalizerFactory factory)
    {
        var localizer = factory.Create(ResourceType);
        return localizer[Name];
    }
}
