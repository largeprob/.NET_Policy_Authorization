using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Localization;

namespace PermissionDemo.Localization;

/// <summary>
///  轻量级 JSON 本地化器。从 Localization/Resources/{culture}.json 读取键值对，
///  按当前 UI 区域性返回对应文本。无第三方依赖，便于演示本地化原理。
/// </summary>
public class JsonStringLocalizer : IStringLocalizer
{
    /// <summary>JSON 资源文件所在目录。</summary>
    private readonly string _resourcesPath;
    /// <summary>区域性 -> 键值对 的内存缓存，避免重复读取文件。</summary>
    private readonly Dictionary<string, Dictionary<string, string>> _cache = new();

    public JsonStringLocalizer(string resourcesPath)
    {
        _resourcesPath = resourcesPath;
    }

    /// <summary>
    ///  按键获取本地化文本；若未找到则回退为键名本身，并标记 <see cref="LocalizedString.ResourceNotFound"/>。
    /// </summary>
    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value ?? name, value == null);
        }
    }

    /// <summary>
    ///  带参数的本地化文本，使用 <see cref="string.Format(string, object?[])"/> 进行占位符替换。
    /// </summary>
    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var format = GetString(name);
            var value = format != null ? string.Format(format, arguments) : name;
            return new LocalizedString(name, value, format == null);
        }
    }

    /// <summary>
    ///  返回当前 UI 区域性下的全部本地化字符串。
    /// </summary>
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        var strings = LoadCulture(culture);
        foreach (var kvp in strings)
        {
            yield return new LocalizedString(kvp.Key, kvp.Value, false);
        }
    }

    /// <summary>
    ///  按键查找文本，依次尝试：当前区域性 -> 父区域性（如 zh-Hans -> zh）-> 默认 en，
    ///  全部未命中返回 null。
    /// </summary>
    private string? GetString(string name)
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        var strings = LoadCulture(culture);
        if (strings.TryGetValue(name, out var value))
            return value;

        // 回退到父区域性
        if (culture.Contains('-'))
        {
            var parent = culture.Split('-')[0];
            strings = LoadCulture(parent);
            if (strings.TryGetValue(name, out value))
                return value;
        }

        // 回退到默认语言（en）
        strings = LoadCulture("en");
        return strings.GetValueOrDefault(name);
    }

    /// <summary>
    ///  加载指定区域性的 JSON 资源文件并缓存；文件不存在时缓存空字典，避免重复读盘。
    /// </summary>
    private Dictionary<string, string> LoadCulture(string culture)
    {
        if (_cache.TryGetValue(culture, out var cached))
            return cached;

        var filePath = Path.Combine(_resourcesPath, $"{culture}.json");
        if (!File.Exists(filePath))
        {
            _cache[culture] = new Dictionary<string, string>();
            return _cache[culture];
        }

        var json = File.ReadAllText(filePath);
        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];
        _cache[culture] = data;
        return data;
    }
}
