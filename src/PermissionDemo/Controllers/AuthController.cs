using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PermissionDemo.Models;

namespace PermissionDemo.Controllers;

/// <summary>
///  认证控制器（演示用）。根据传入的用户名和角色直接签发 JWT，不做密码验证。
///  真实项目应替换为校验凭据、查询用户角色后再签发的逻辑。
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    ///  签发 JWT：把用户名写入 Name 声明，把每个角色写入 Role 声明，
    ///  授权时即依据这些 Role 声明判断用户拥有哪些权限。Token 有效期 2 小时。
    /// </summary>
    [HttpPost("token")]
    public IActionResult GetToken([FromBody] LoginRequest request)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, request.UserName),
            new(ClaimTypes.Name, request.UserName)
        };

        foreach (var role in request.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return Ok(new TokenResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}

