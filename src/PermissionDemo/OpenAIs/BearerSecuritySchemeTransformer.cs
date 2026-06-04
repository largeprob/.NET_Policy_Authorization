using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace PermissionDemo
{
    /// <summary>
    ///  OpenAPI 文档转换器。检测到启用了 JWT Bearer 认证方案后，
    ///  向文档注入名为 "Bearer" 的安全方案，使 Swagger UI 出现 Token 输入框。
    /// </summary>
    internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
        : IOpenApiDocumentTransformer
    {
        /// <summary>
        ///  若存在 JWT Bearer 认证方案，则向 OpenAPI 文档的 Components 添加 Bearer 安全方案定义。
        /// </summary>
        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

            if (authenticationSchemes.Any(authScheme => authScheme.Name == JwtBearerDefaults.AuthenticationScheme))
            {
                var securitySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                {
                    ["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",        
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "请输入 JWT Token，不需要加 'Bearer ' 前缀"
                    }
                };
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes = securitySchemes;
            }
        }
    }
}
