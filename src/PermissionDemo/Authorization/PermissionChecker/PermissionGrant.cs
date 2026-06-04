namespace PermissionDemo.Authorization.PermissionChecker;

/// <summary>
///  权限授权记录，表示"某个提供者（角色或用户）被授予了某个权限"。
///  在真实系统中通常持久化到数据库，本示例存储在内存中。
/// </summary>
public class PermissionGrant
{
    /// <summary>
    ///  权限名称（与权限定义中的 Name 对应）。
    /// </summary>
    public string PermissionName { get; set; } = default!;

    /// <summary>
    ///  提供者类型，取值 "Role"（角色）或 "User"（用户）。
    /// </summary>
    public string ProviderName { get; set; } = default!;

    /// <summary>
    ///  提供者标识，角色名或用户 ID。
    /// </summary>
    public string ProviderKey { get; set; } = default!;
}
