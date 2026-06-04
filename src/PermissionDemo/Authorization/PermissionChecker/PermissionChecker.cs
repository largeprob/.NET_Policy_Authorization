using System.Security.Claims;

namespace PermissionDemo.Authorization.PermissionChecker;


/// <summary>
///  权限检查器接口，定义了检查用户是否具有特定权限的方法。
/// </summary>
public interface IPermissionChecker
{
    Task<bool> IsGrantedAsync(ClaimsPrincipal user, string permissionName);
}

/// <summary>
///  权限检查器实现类，使用内存中的权限存储来检查用户的权限。
/// </summary>
public class PermissionChecker : IPermissionChecker
{
    // 模拟数据库
    private readonly InMemoryPermissionStore _store;

    public PermissionChecker(InMemoryPermissionStore store)
    {
        _store = store;
    }

    public async Task<bool> IsGrantedAsync(ClaimsPrincipal user, string permissionName)
    {
        //  检查用户级别的授权
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userId))
        {
            if (await _store.IsGrantedAsync("User", userId, permissionName))
                return true;
        }

        //  检查角色级别的授权
        var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value);
        foreach (var role in roles)
        {
            if (await _store.IsGrantedAsync("Role", role, permissionName))
                return true;
        }

        return false;
    }
}
