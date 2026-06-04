using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using PermissionDemo.Authorization.Permissions;
using PermissionDemo.Authorization.Requirements;

namespace PermissionDemo.Authorization.PolicyProvider;

/// <summary>
///  权限策略提供者（仿 ABP 的核心机制）。
///  拦截形如 [Authorize("BookStore.Books.Create")] 的策略名，若该名称是已注册的权限，
///  则动态构建包含 <see cref="PermissionRequirement"/> 的授权策略；否则回退到默认提供者。
///  这样无需为每个权限手动调用 AddPolicy 预注册。
/// </summary>
public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    /// <summary>默认策略提供者，用于处理非权限类的常规策略。</summary>
    private readonly DefaultAuthorizationPolicyProvider _fallback;
    /// <summary>权限定义管理器，用于判断策略名是否为已注册权限。</summary>
    private readonly IPermissionDefinitionManager _permissionManager;

    public PermissionPolicyProvider(
        IOptions<AuthorizationOptions> options,
        IPermissionDefinitionManager permissionManager)
    {
        _fallback = new DefaultAuthorizationPolicyProvider(options);
        _permissionManager = permissionManager;
    }


    /// <summary>
    /// 动态创建策略
    /// </summary>
    /// <param name="policyName"></param>
    /// <returns></returns>
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var permission = _permissionManager.GetOrNull(policyName);
        if (permission != null)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return _fallback.GetPolicyAsync(policyName);
    }




    /// <summary>
    ///  获取默认策略（用于仅标注 [Authorize] 而未指定策略名的场景）。
    /// </summary>
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => _fallback.GetDefaultPolicyAsync();

    /// <summary>
    ///  获取回退策略。
    /// </summary>
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => _fallback.GetFallbackPolicyAsync();
}
