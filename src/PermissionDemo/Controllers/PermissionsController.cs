using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PermissionDemo.Authorization.Permissions;

namespace PermissionDemo.Controllers;

/// <summary>
///  权限查询控制器。返回完整的权限分组与层级树，并按当前请求区域性本地化显示名称，
///  常用于前端权限管理界面的渲染。
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionDefinitionManager _manager;
    private readonly IStringLocalizerFactory _localizerFactory;

    public PermissionsController(
        IPermissionDefinitionManager manager,
        IStringLocalizerFactory localizerFactory)
    {
        _manager = manager;
        _localizerFactory = localizerFactory;
    }

    /// <summary>
    ///  获取全部权限分组及其下的权限树，显示名称已本地化（随请求区域性变化）。
    /// </summary>
    [HttpGet]
    public IActionResult GetAll()
    {
        var groups = _manager.GetGroups().Select(g => new
        {
            g.Name,
            DisplayName = g.DisplayName?.Localize(_localizerFactory) ?? g.Name,
            Permissions = g.Permissions.Select(p => MapPermission(p)).ToList()
        });

        return Ok(new { groups });
    }

    /// <summary>
    ///  把单个权限定义递归映射为带本地化显示名称的匿名对象（含子权限）。
    /// </summary>
    private object MapPermission(PermissionDefinition permission)
    {
        return new
        {
            permission.Name,
            DisplayName = permission.DisplayName?.Localize(_localizerFactory) ?? permission.Name,
            Children = permission.Children.Select(c => MapPermission(c)).ToList()
        };
    }
}
