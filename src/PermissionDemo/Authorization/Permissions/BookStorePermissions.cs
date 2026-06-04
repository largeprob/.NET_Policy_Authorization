namespace PermissionDemo.Authorization.Permissions;

/// <summary>
///  图书商店模块的权限名称常量定义。
///  使用点分隔的命名空间格式，集中管理权限名，避免在代码中散落硬编码的字符串。
/// </summary>
public static class BookStorePermissions
{
    /// <summary>
    ///  权限分组名称
    /// </summary>
    public const string GroupName = "BookStore";

    /// <summary>
    ///  图书相关权限
    /// </summary>
    public static class Books
    {
        /// <summary>查看图书（父权限，默认即读取权限）</summary>
        public const string Default = GroupName + ".Books";
        /// <summary>创建图书</summary>
        public const string Create = Default + ".Create";
        /// <summary>编辑图书</summary>
        public const string Edit = Default + ".Edit";
        /// <summary>删除图书</summary>
        public const string Delete = Default + ".Delete";
    }

    /// <summary>
    ///  作者相关权限
    /// </summary>
    public static class Authors
    {
        /// <summary>查看作者（父权限，默认即读取权限）</summary>
        public const string Default = GroupName + ".Authors";
        /// <summary>创建作者</summary>
        public const string Create = Default + ".Create";
        /// <summary>编辑作者</summary>
        public const string Edit = Default + ".Edit";
        /// <summary>删除作者</summary>
        public const string Delete = Default + ".Delete";
    }
}
