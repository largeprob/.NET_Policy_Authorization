using PermissionDemo.Authorization.Permissions;
using System.Drawing;
using System.Security;

namespace PermissionDemo.Authorization.PermissionChecker;

/// <summary>
///  内存权限存储（模拟数据库）。保存"角色/用户被授予哪些权限"的记录，
///  在应用启动时通过 <see cref="Seed"/> 灌入演示数据。真实项目中应替换为数据库存储。
/// </summary>
public class InMemoryPermissionStore
{
    private readonly List<PermissionGrant> _grants = [];

    /// <summary>
    ///  初始化演示授权数据：admin 拥有全部权限，editor 拥有图书的增删改查，viewer 仅可查看图书。
    /// </summary>
    /// <param name="manager">权限定义管理器，用于获取全部权限以授予 admin。</param>
    public void Seed(IPermissionDefinitionManager manager)
    {
        var allPermissions = manager.GetAll();

        // 添加系统的角色授权


        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n=== admin-角色授权 ===");
        foreach (var permission in allPermissions)
        {
            _grants.Add(new PermissionGrant
            {
                PermissionName = permission.Name,
                ProviderName = "Role",
                ProviderKey = "admin"
            });

            Console.WriteLine($"  - {permission.Name}");
        }
        Console.ResetColor();

        //  图书编辑：可以查看、创建、编辑和删除图书

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n=== 图书编辑-角色授权 ===");
        Console.WriteLine($"  - {BookStorePermissions.Books.Default}");
        Console.WriteLine($"  - {BookStorePermissions.Books.Create}");
        Console.WriteLine($"  - {BookStorePermissions.Books.Edit}");
        Console.WriteLine($"  - {BookStorePermissions.Books.Delete}");

        _grants.Add(new PermissionGrant { PermissionName = BookStorePermissions.Books.Default, ProviderName = "Role", ProviderKey = "editor" });
        _grants.Add(new PermissionGrant { PermissionName = BookStorePermissions.Books.Create, ProviderName = "Role", ProviderKey = "editor" });
        _grants.Add(new PermissionGrant { PermissionName = BookStorePermissions.Books.Edit, ProviderName = "Role", ProviderKey = "editor" });
        _grants.Add(new PermissionGrant { PermissionName = BookStorePermissions.Books.Delete, ProviderName = "Role", ProviderKey = "editor" });




        // 图书查看者：只能查看图书

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n=== 图书只读-角色授权 ===");
        Console.WriteLine($"  - {BookStorePermissions.Books.Default}");
        _grants.Add(new PermissionGrant { PermissionName = BookStorePermissions.Books.Default, ProviderName = "Role", ProviderKey = "viewer" });

    }

    /// <summary>
    ///  判断数据库中是否存在指定权限的授权记录，如果存在则返回true，否则返回false。
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="permissionName"></param>
    /// <returns></returns>
    public Task<bool> IsGrantedAsync(string providerName, string providerKey, string permissionName)
    {
        var granted = _grants.Any(g =>
            g.ProviderName == providerName &&
            g.ProviderKey == providerKey &&
            g.PermissionName == permissionName);
        return Task.FromResult(granted);
    }
}
