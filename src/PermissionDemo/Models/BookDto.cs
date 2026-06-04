namespace PermissionDemo.Models;

/// <summary>图书数据传输对象，用于演示受权限保护的 CRUD 接口。</summary>
public class BookDto
{
    /// <summary>图书主键。</summary>
    public int Id { get; set; }
    /// <summary>书名。</summary>
    public string Title { get; set; } = default!;
    /// <summary>作者。</summary>
    public string Author { get; set; } = default!;
    /// <summary>价格。</summary>
    public decimal Price { get; set; }
}

/// <summary>登录请求（演示用）。仅提供用户名与角色列表，无密码校验，用于签发 JWT。</summary>
public class LoginRequest
{
    /// <summary>用户名，将写入 Token 的 Name 声明。</summary>
    public string UserName { get; set; } = default!;
    /// <summary>角色列表，将逐个写入 Token 的 Role 声明，决定用户拥有的权限。</summary>
    public string[] Roles { get; set; } = [];
}

/// <summary>Token 签发响应。</summary>
public class TokenResponse
{
    /// <summary>签发的 JWT 字符串。</summary>
    public string Token { get; set; } = default!;
}
