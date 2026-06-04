namespace PermissionDemo.Authorization.Permissions;

/// <summary>
///  权限定义，表示一个具体的权限项。权限定义可以有子权限，形成树状结构。
/// </summary>
public class PermissionDefinition
{

    /// <summary>
    /// 权限名称，必须唯一。通常使用点分隔的命名空间格式，例如 "Product.Create"、"Product.Edit" 等。
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 权限的本地化显示名称，用于在用户界面中显示。可以使用 LocalizableString 来支持多语言环境。
    /// </summary>
    public LocalizableString? DisplayName { get; set; }
    /// <summary>
    ///  是否启用。禁用后该权限不参与授权判断，常用于功能开关场景。
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    ///  父权限。顶级权限该值为 null，子权限指向其父节点，从而形成树状层级。
    /// </summary>
    public PermissionDefinition? Parent { get; internal set; }

    /// <summary>
    ///  所属权限分组。
    /// </summary>
    public PermissionGroupDefinition? Group { get; internal set; }

    /// <summary>
    ///  子权限列表。
    /// </summary>
    public List<PermissionDefinition> Children { get; } = [];

    internal PermissionDefinition(string name, LocalizableString? displayName = null)
    {
        Name = name;
        DisplayName = displayName;
    }

    /// <summary>
    ///  在当前权限下添加一个子权限，子权限自动继承父权限所属的分组。
    /// </summary>
    /// <param name="name">子权限名称</param>
    /// <param name="displayName">子权限本地化显示名称</param>
    /// <returns>新创建的子权限定义</returns>
    public PermissionDefinition AddChild(string name, LocalizableString? displayName = null)
    {
        var child = new PermissionDefinition(name, displayName)
        {
            Parent = this,
            Group = Group
        };
        Children.Add(child);
        return child;
    }
}
