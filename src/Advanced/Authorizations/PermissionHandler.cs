using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Advanced.Authorizations
{
    /// <summary>
    /// 处理具体的授权行为
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionAttribute>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAttribute requirement)
        {
            var permission = context.User.FindFirst(
            c => c.Type.ToString() == "Permission" && c.Value == requirement.Permission);

            if (permission is null)
            {
                context.Fail();
            }
            else {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
