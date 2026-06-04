using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using PermissionDemo.Authorization.PermissionChecker;
using PermissionDemo.Authorization.Permissions;
using PermissionDemo.Authorization.PolicyProvider;
using PermissionDemo.Authorization.Requirements;
using PermissionDemo.Localization;

namespace PermissionDemo.Authorization;

/// <summary>
///  权限授权服务注册扩展。集中注册权限系统所需的全部服务，
///  使 Program.cs 只需一行 AddPermissionAuthorization() 即可完成装配。
/// </summary>
public static class PermissionAuthorizationExtensions
{
    /// <summary>
    ///  注册权限授权相关服务：权限定义、权限检查、动态策略提供者、授权处理器及本地化工厂。
    /// </summary>
    public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services)
    {
        // 注册权限定义提供者
        services.AddSingleton<IPermissionDefinitionProvider, BookStorePermissionDefinitionProvider>();

        // 注册权限定义管理器
        services.AddSingleton<IPermissionDefinitionManager, PermissionDefinitionManager>();

        services.AddSingleton<InMemoryPermissionStore>();
        services.AddScoped<IPermissionChecker, PermissionChecker.PermissionChecker>();

        // 注册权限授权处理器和策略提供者
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();

        services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();


        services.AddSingleton<IAuthorizationMiddlewareResultHandler, SampleAuthorizationMiddlewareResultHandler>();
        return services;
    }
}
