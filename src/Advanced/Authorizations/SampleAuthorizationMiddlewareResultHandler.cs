using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace  Advanced.Authorizations
{
    /// <summary>
    /// 示例授权中间件结果处理程序，演示如何自定义授权失败时的响应。
    /// </summary>
    public class SampleAuthorizationMiddlewareResultHandler: IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

        public async Task HandleAsync(
             RequestDelegate next,
             HttpContext context,
             AuthorizationPolicy policy,
             PolicyAuthorizationResult authorizeResult)
        {

            // 验证失败且是 Forbidden（即用户已认证但没有权限访问资源）
            if (authorizeResult.Forbidden)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync("IAuthorizationMiddlewareResultHandler 未授权");
                return;
            }

            // Fall back to the default implementation.
            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }

    }
}
