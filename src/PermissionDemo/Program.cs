using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using PermissionDemo;
using PermissionDemo.Authorization;
using PermissionDemo.Authorization.PermissionChecker;
using PermissionDemo.Authorization.Permissions;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi  API文档
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    options.AddOperationTransformer<AuthorizationOperationTransformer>();
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "API", Version = "v1" });
});


// JWT 身份认证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// 权限授权
builder.Services.AddPermissionAuthorization();

var app = builder.Build();

//  应用启动时，加载权限到数据库（此处内容模拟）
var store = app.Services.GetRequiredService<InMemoryPermissionStore>();
var manager = app.Services.GetRequiredService<IPermissionDefinitionManager>();
store.Seed(manager);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = [new CultureInfo("en"), new CultureInfo("zh-Hans")],
    SupportedUICultures = [new CultureInfo("en"), new CultureInfo("zh-Hans")]
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
