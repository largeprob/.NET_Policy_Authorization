# ASP.NET Core 权限授权演示

这个仓库用三个 ASP.NET Core Web API 项目，按阶段演示从基础策略授权到类 ABP 权限体系的演进过程。

三个项目不是重复 Demo，而是对应不同学习阶段：

| 阶段 | 项目 | 重点 |
| --- | --- | --- |
| 第一阶段 | `BasicDemo` | 使用 ASP.NET Core 原生 Policy + Claim 完成基础权限控制 |
| 第二阶段 | `Advanced` | 封装权限特性与授权处理器，减少控制器里的硬编码授权写法 |
| 第三阶段 | `PermissionDemo` | 模拟 ABP 风格的权限定义、动态策略、权限检查和本地化显示 |

## 项目结构

```text
.
├── PermissionDemo.slnx
└── src
    ├── BasicDemo          # 第一阶段：基础策略授权
    ├── Advanced           # 第二阶段：自定义权限授权
    └── PermissionDemo     # 第三阶段：类 ABP 权限体系
```

## 第一阶段：BasicDemo

`BasicDemo` 用 ASP.NET Core 自带的授权能力完成最基础的权限控制。

核心思路：

1. 登录接口签发 JWT。
2. JWT 中写入 `Permission` Claim。
3. 在 `Program.cs` 中注册固定授权策略。
4. 控制器方法使用 `[Authorize(Policy = "权限名")]` 保护接口。

示例策略：

```csharp
options.AddPolicy("Customer.List",
    policy => policy.RequireClaim("Permission", "Customer.List"));
```

示例接口：

```csharp
[Authorize(Policy = "Customer.List")]
public ActionResult All()
```

适合学习：

- JWT 认证
- Claim 权限声明
- ASP.NET Core Policy 授权
- 401 / 403 的基础处理

这个阶段的特点是直观、容易理解，但当权限越来越多时，策略注册和控制器标注会变得比较重复。

## 第二阶段：Advanced

`Advanced` 在第一阶段基础上做了一层封装，引入自定义权限特性和授权处理器。

核心思路：

1. 继续使用 JWT + Permission Claim。
2. 用自定义 `PermissionAttribute` 表达接口需要的权限。
3. 通过 `PermissionHandler` 统一处理权限判断。
4. 控制器不再直接依赖原生 Policy 写法。

示例接口：

```csharp
[PermissionAttribute("Customer.List")]
public ActionResult All()
```

适合学习：

- 自定义授权 Requirement
- 自定义 AuthorizationHandler
- 自定义权限 Attribute
- 授权逻辑集中封装

这个阶段解决了第一阶段“到处写 Policy 名称”的问题，代码更接近真实业务项目里的权限封装方式。

## 第三阶段：PermissionDemo

`PermissionDemo` 是最终阶段，目标是模拟 ABP 风格的权限系统。

它不只是判断某个接口有没有权限，还加入了权限定义、权限分组、动态策略、权限存储、角色授权和本地化显示。

核心能力：

- 使用权限常量集中管理权限名
- 使用权限定义 Provider 注册权限树
- 支持权限分组和子权限
- 启动时模拟把权限定义加载到权限存储
- 根据角色判断用户拥有的权限
- 使用动态 Policy Provider 自动生成授权策略
- 支持权限显示名称本地化

示例接口：

```csharp
[Authorize(BookStorePermissions.Books.Default)]
public IActionResult GetAll()
```

示例权限：

```text
Permission:BookStore
Permission:BookStore.Books
Permission:BookStore.Books.Create
Permission:BookStore.Books.Edit
Permission:BookStore.Books.Delete
```

适合学习：

- ABP 权限系统的核心思想
- 权限定义和权限检查解耦
- 动态授权策略
- 角色与权限映射
- 权限管理界面需要的权限树数据
- 多语言权限显示名称

## 运行环境

项目基于 .NET Web API，运行前请确保本机已安装对应 .NET SDK。

可以先查看 SDK：

```bash
dotnet --version
```

还原依赖：

```bash
dotnet restore PermissionDemo.slnx
```

## 运行项目

### 运行 BasicDemo

```bash
dotnet run --project src/BasicDemo/01BasicDemo.csproj
```

### 运行 Advanced

```bash
dotnet run --project src/Advanced/02Advanced.csproj
```

### 运行 PermissionDemo

```bash
dotnet run --project src/PermissionDemo/PermissionDemo.csproj
```

开发环境下可以打开 Swagger 页面测试接口。

## 接口测试流程

### 1. 获取 Token

`BasicDemo` 和 `Advanced`：

```http
POST /Auth/token
Content-Type: application/json

{
  "account": "admin"
}
```

`account` 为 `admin` 时，会签发带有权限 Claim 的 Token。

`PermissionDemo`：

```http
POST /api/auth/token
Content-Type: application/json

{
  "userName": "admin",
  "roles": ["Admin"]
}
```

### 2. 调用受保护接口

请求头加入：

```http
Authorization: Bearer {token}
```

`BasicDemo` / `Advanced` 示例：

```http
GET /Custome/All
POST /Custome/Add
POST /Custome/Update
DELETE /Custome/Delete
```

`PermissionDemo` 示例：

```http
GET /api/books
GET /api/books/1
POST /api/books
PUT /api/books/1
DELETE /api/books/1
```

### 3. 查看权限树

`PermissionDemo` 提供权限树查询接口：

```http
GET /api/permissions
```

这个接口会返回完整的权限分组、权限层级和本地化后的显示名称，适合前端权限管理页面使用。

## 本地化测试

`PermissionDemo` 当前支持：

- `en`
- `zh-Hans`

英文：

```http
GET /api/permissions?culture=en&ui-culture=en
```

中文：

```http
GET /api/permissions?culture=zh-Hans&ui-culture=zh-Hans
```

如果本地化生效，同一个权限会显示不同文本：

```text
Book Store
书店
```

本地化资源文件位于：

```text
src/PermissionDemo/Localization/Resources/en.json
src/PermissionDemo/Localization/Resources/zh-Hans.json
```

## 三个阶段的演进关系

### BasicDemo：能跑起来

先理解 ASP.NET Core 原生授权模型：认证负责“你是谁”，授权负责“你能做什么”。

这一阶段适合把 JWT、Claim、Policy、Authorize 这些基础概念串起来。

### Advanced：封装起来

当权限变多后，直接写 Policy 会变得重复。

这一阶段开始把权限判断封装成自定义特性和处理器，让业务代码更清爽。

### PermissionDemo：体系化

真实项目里，权限通常不只是写在接口上，还要能展示、分组、分配、存储和本地化。

这一阶段模拟 ABP 的权限设计，把权限从“接口上的字符串”升级成“可管理的权限体系”。

## 学习建议

建议按下面顺序阅读源码：

1. `src/BasicDemo/Program.cs`
2. `src/BasicDemo/Controllers/CustomeController.cs`
3. `src/Advanced/Authorizations`
4. `src/Advanced/Controllers/CustomeController.cs`
5. `src/PermissionDemo/Authorization/Permissions`
6. `src/PermissionDemo/Authorization/PolicyProvider`
7. `src/PermissionDemo/Authorization/PermissionChecker`
8. `src/PermissionDemo/Localization`
9. `src/PermissionDemo/Controllers`

## 适合的读者

这个仓库适合：

- 想理解 ASP.NET Core 授权机制的开发者
- 想从零实现权限系统的 .NET 开发者
- 想学习 ABP 权限设计思想的开发者
- 想做后台管理系统、RBAC、菜单权限、按钮权限的开发者

## 注意事项

这是一个教学演示项目，不是生产级权限系统。

当前 Demo 中的 Token 签发、用户校验、权限存储都做了简化处理。真实项目中应接入数据库、用户体系、角色体系、刷新 Token、密码校验和更完整的安全策略。
