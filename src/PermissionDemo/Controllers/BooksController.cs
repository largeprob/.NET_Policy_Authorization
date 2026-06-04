using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionDemo.Authorization.Permissions;
using PermissionDemo.Models;

namespace PermissionDemo.Controllers;

/// <summary>
///  图书控制器。演示 ABP 风格的权限保护：每个动作用
///  <c>[Authorize("权限名")]</c> 标注所需权限，策略由 PermissionPolicyProvider 动态生成。
///  类级 [Authorize] 要求先通过认证，方法级特性再校验具体权限。数据用静态列表模拟存储。
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
    /// <summary>模拟图书数据存储（进程内静态列表，仅供演示）。</summary>
    private static readonly List<BookDto> Books =
    [
        new() { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", Price = 39.99m },
        new() { Id = 2, Title = "Domain-Driven Design", Author = "Eric Evans", Price = 54.99m },
        new() { Id = 3, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Price = 49.99m }
    ];

    /// <summary>查询全部图书，需要"查看图书"权限。</summary>
    [HttpGet]
    [Authorize(BookStorePermissions.Books.Default)]
    public IActionResult GetAll()
    {
        return Ok(Books);
    }

    /// <summary>按 Id 查询单本图书，需要"查看图书"权限。</summary>
    [HttpGet("{id}")]
    [Authorize(BookStorePermissions.Books.Default)]
    public IActionResult Get(int id)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        return book is null ? NotFound() : Ok(book);
    }

    /// <summary>创建图书，需要"创建图书"权限。</summary>
    [HttpPost]
    [Authorize(BookStorePermissions.Books.Create)]
    public IActionResult Create([FromBody] BookDto input)
    {
        input.Id = Books.Count > 0 ? Books.Max(b => b.Id) + 1 : 1;
        Books.Add(input);
        return CreatedAtAction(nameof(Get), new { id = input.Id }, input);
    }

    /// <summary>编辑图书，需要"编辑图书"权限。</summary>
    [HttpPut("{id}")]
    [Authorize(BookStorePermissions.Books.Edit)]
    public IActionResult Update(int id, [FromBody] BookDto input)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        if (book is null) return NotFound();

        book.Title = input.Title;
        book.Author = input.Author;
        book.Price = input.Price;
        return Ok(book);
    }

    /// <summary>删除图书，需要"删除图书"权限。</summary>
    [HttpDelete("{id}")]
    [Authorize(BookStorePermissions.Books.Delete)]
    public IActionResult Delete(int id)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        if (book is null) return NotFound();

        Books.Remove(book);
        return NoContent();
    }
}
