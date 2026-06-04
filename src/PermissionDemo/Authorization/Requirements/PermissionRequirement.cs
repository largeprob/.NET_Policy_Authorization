using Microsoft.AspNetCore.Authorization;

namespace PermissionDemo.Authorization.Requirements;

/// <summary>
///  权限授权要求，携带需要校验的权限名称进入 ASP.NET Core 授权管道。
///  由 <see cref="PermissionRequirementHandler"/> 负责处理该要求。
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    /// <summary>
    ///  需要校验的权限名称。
    /// </summary>
    public string PermissionName { get; }

    public PermissionRequirement(string permissionName)
    {
        PermissionName = permissionName;
    }
}
