using Microsoft.AspNetCore.Authorization;
using PermissionDemo.Authorization.PermissionChecker;

namespace PermissionDemo.Authorization.Requirements;

/// <summary>
///  权限授权要求处理器。把授权管道中的 <see cref="PermissionRequirement"/>
///  桥接到 <see cref="IPermissionChecker"/>，由后者判断当前用户是否拥有该权限。
/// </summary>
public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionChecker _checker;

    public PermissionRequirementHandler(IPermissionChecker checker)
    {
        _checker = checker;
    }

    /// <summary>
    ///  处理权限要求：若用户被授予了所需权限，则标记该要求为通过。
    /// </summary>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (await _checker.IsGrantedAsync(context.User, requirement.PermissionName))
        {
            context.Succeed(requirement);
        }
    }
}
