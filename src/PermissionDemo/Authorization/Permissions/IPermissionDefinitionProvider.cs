namespace PermissionDemo.Authorization.Permissions;

/// <summary>
///  权限定义提供者接口。各业务模块通过实现该接口，在应用启动时向系统注册自己的权限定义，
///  从而实现权限的模块化、可扩展定义（类似 ABP 的 PermissionDefinitionProvider）。
/// </summary>
public interface IPermissionDefinitionProvider
{
    /// <summary>
    ///  定义权限。框架会传入权限定义上下文，在该方法内通过上下文添加权限分组与权限项。
    /// </summary>
    /// <param name="context">权限定义上下文</param>
    void Define(PermissionDefinitionContext context);
}
