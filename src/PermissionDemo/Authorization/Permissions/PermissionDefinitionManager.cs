namespace PermissionDemo.Authorization.Permissions;

/// <summary>
///  权限定义管理器接口。汇总所有权限定义提供者的定义结果，对外提供统一的权限查询能力。
/// </summary>
public interface IPermissionDefinitionManager
{
    /// <summary>获取所有权限分组（含层级结构）。</summary>
    IReadOnlyList<PermissionGroupDefinition> GetGroups();
    /// <summary>获取所有权限的扁平列表（递归展开子权限）。</summary>
    IReadOnlyList<PermissionDefinition> GetAll();
    /// <summary>按名称查找权限，不存在时返回 null。</summary>
    PermissionDefinition? GetOrNull(string name);
    /// <summary>按名称查找权限，不存在时抛出异常。</summary>
    PermissionDefinition Get(string name);
}

/// <summary>
///  权限定义管理器实现。通过依赖注入收集所有 <see cref="IPermissionDefinitionProvider"/>，
///  使用 <see cref="Lazy{T}"/> 在首次访问时构建权限注册表（权限定义为静态元数据，只需构建一次）。
/// </summary>
public class PermissionDefinitionManager : IPermissionDefinitionManager
{
    private readonly Lazy<PermissionDefinitionContext> _context;

    public PermissionDefinitionManager(IEnumerable<IPermissionDefinitionProvider> providers)
    {
        _context = new Lazy<PermissionDefinitionContext>(() =>
        {
            var context = new PermissionDefinitionContext();
            foreach (var provider in providers)
            {
                provider.Define(context);
            }
            return context;
        });
    }

    public IReadOnlyList<PermissionGroupDefinition> GetGroups() => _context.Value.GetGroups();

    /// <summary>
    ///  返回所有权限定义的列表，包括所有分组和子权限。
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<PermissionDefinition> GetAll()
    {
        var permissions = new List<PermissionDefinition>();
        foreach (var group in GetGroups())
        {
            CollectPermissions(group.Permissions, permissions);
        }
        return permissions;
    }

    /// <summary>
    ///  按名称查找权限，不存在时返回 null。
    /// </summary>
    public PermissionDefinition? GetOrNull(string name)
    {
        return GetAll().FirstOrDefault(p => p.Name == name);
    }

    /// <summary>
    ///  按名称查找权限，不存在时抛出异常。
    /// </summary>
    public PermissionDefinition Get(string name)
    {
        return GetOrNull(name) ?? throw new InvalidOperationException($"Permission '{name}' is not defined.");
    }

    /// <summary>
    ///  递归收集权限及其所有子权限，展开为扁平列表。
    /// </summary>
    private static void CollectPermissions(List<PermissionDefinition> source, List<PermissionDefinition> target)
    {
        foreach (var permission in source)
        {
            target.Add(permission);
            CollectPermissions(permission.Children, target);
        }
    }
}
