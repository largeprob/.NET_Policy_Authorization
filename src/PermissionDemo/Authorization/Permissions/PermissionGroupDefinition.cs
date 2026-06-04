namespace PermissionDemo.Authorization.Permissions;

/// <summary>
///  权限分组定义，表示一组相关的权限。每个权限分组可以包含多个权限定义
/// </summary>
public class PermissionGroupDefinition
{
    /// <summary>
    ///  分组名称
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 本地化名称
    /// </summary>
    public LocalizableString? DisplayName { get; set; }

    /// <summary>
    /// 权限定义列表
    /// </summary>
    public List<PermissionDefinition> Permissions { get; } = [];


    internal PermissionGroupDefinition(string name, LocalizableString? displayName = null)
    {
        Name = name;
        DisplayName = displayName;
    }

    /// <summary>
    ///  向当前分组添加一个顶级权限。
    /// </summary>
    /// <param name="name">权限名称</param>
    /// <param name="displayName">权限本地化显示名称</param>
    /// <returns>新创建的权限定义</returns>
    public PermissionDefinition AddPermission(string name, LocalizableString? displayName = null)
    {
        var permission = new PermissionDefinition(name, displayName)
        {
            Group = this
        };
        Permissions.Add(permission);
        return permission;
    }
}
