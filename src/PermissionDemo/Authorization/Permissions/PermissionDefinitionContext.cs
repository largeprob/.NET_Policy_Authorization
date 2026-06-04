namespace PermissionDemo.Authorization.Permissions;

/// <summary>
///  权限定义上下文，用于在应用程序启动时定义权限结构。
/// </summary>
public class PermissionDefinitionContext
{
    private readonly List<PermissionGroupDefinition> _groups = [];

    /// <summary>
    ///  添加一个权限分组。
    /// </summary>
    /// <param name="name">分组名称</param>
    /// <param name="displayName">分组本地化显示名称</param>
    /// <returns>新创建的权限分组</returns>
    public PermissionGroupDefinition AddGroup(string name, LocalizableString? displayName = null)
    {
        var group = new PermissionGroupDefinition(name, displayName);
        _groups.Add(group);
        return group;
    }

    /// <summary>
    ///  获取已定义的所有权限分组。
    /// </summary>
    public IReadOnlyList<PermissionGroupDefinition> GetGroups() => _groups;
}
