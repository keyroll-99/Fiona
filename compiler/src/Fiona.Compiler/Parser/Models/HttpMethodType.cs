namespace Fiona.Compiler.Parser.Models;

[Flags]
public enum HttpMethodType
{
    Get = 1 << 0,
    Post = 1 << 1,
    Put = 1 << 2,
    Patch = 1 << 3,
    Delete = 1 << 4
}

internal static class HttpMethodTypeExtensionMethods
{
    public static IEnumerable<HttpMethodType> GetMethodTypes(this HttpMethodType httpMethodTypes)
    {
        return Enum.GetValues<HttpMethodType>().Where(value => httpMethodTypes.HasFlag(value));
    }

    public static HttpMethodType? GetHttpMethodType(string method)
    {
        return method.ToUpper() switch
        {
            "GET" => HttpMethodType.Get,
            "POST" => HttpMethodType.Post,
            "PUT" => HttpMethodType.Put,
            "PATCH" => HttpMethodType.Patch,
            "DELETE" => HttpMethodType.Delete,
            _ => null
        };
    }

    public static string GetMethodTypesUsing(this HttpMethodType httpMethodType)
    {
        return httpMethodType switch
        {
            HttpMethodType.Get => "HttpMethodType.Get",
            HttpMethodType.Post => "HttpMethodType.Post",
            HttpMethodType.Put => "HttpMethodType.Put",
            HttpMethodType.Patch => "HttpMethodType.Patch",
            HttpMethodType.Delete => "HttpMethodType.Delete",
            _ => throw new ArgumentOutOfRangeException(nameof(httpMethodType), httpMethodType, null)
        };
    }

    public static bool HasBody(this HttpMethodType methodType) => methodType is HttpMethodType.Put or HttpMethodType.Post or HttpMethodType.Patch;
}