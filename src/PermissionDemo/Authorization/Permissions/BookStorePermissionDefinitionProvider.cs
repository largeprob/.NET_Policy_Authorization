using PermissionDemo.Authorization.PermissionChecker;
using PermissionDemo.Localization;

namespace PermissionDemo.Authorization.Permissions;

/// <summary>
///  图书商店模块的权限定义提供者。在应用启动时被框架调用，
///  向系统注册"书店"分组下的图书、作者两组权限及其子权限。
/// </summary>
public class BookStorePermissionDefinitionProvider : IPermissionDefinitionProvider
{
    /// <summary>
    ///  定义本模块的权限分组与权限项。
    /// </summary>
    public void Define(PermissionDefinitionContext context)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n加载图书管理权限");
        Console.ResetColor();


        var group = context.AddGroup(
            BookStorePermissions.GroupName,
            L("Permission:BookStore"));

        var booksPermission = group.AddPermission(
            BookStorePermissions.Books.Default,
            L("Permission:BookStore.Books"));
        booksPermission.AddChild(BookStorePermissions.Books.Create, L("Permission:BookStore.Books.Create"));
        booksPermission.AddChild(BookStorePermissions.Books.Edit, L("Permission:BookStore.Books.Edit"));
        booksPermission.AddChild(BookStorePermissions.Books.Delete, L("Permission:BookStore.Books.Delete"));

        var authorsPermission = group.AddPermission(
            BookStorePermissions.Authors.Default,
            L("Permission:BookStore.Authors"));
        authorsPermission.AddChild(BookStorePermissions.Authors.Create, L("Permission:BookStore.Authors.Create"));
        authorsPermission.AddChild(BookStorePermissions.Authors.Edit, L("Permission:BookStore.Authors.Edit"));
        authorsPermission.AddChild(BookStorePermissions.Authors.Delete, L("Permission:BookStore.Authors.Delete"));
    }

    /// <summary>
    ///  创建可本地化字符串的辅助方法，简化权限显示名称的定义。
    /// </summary>
    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PermissionResource>(name);
    }
}
