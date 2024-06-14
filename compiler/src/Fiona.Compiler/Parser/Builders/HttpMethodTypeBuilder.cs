namespace Fiona.Compiler.Parser.Builders;

[Flags]
public enum HttpMethodTypeBuilder
{
    Get = 1 << 0,
    Post = 1 << 1,
    Put = 1 << 2,
    Patch = 1 << 3,
    Delete = 1 << 4
}

internal static class HttpMethodTypeExtensionMethods
{
    public static IEnumerable<HttpMethodTypeBuilder> GetMethodTypes(this HttpMethodTypeBuilder httpMethodTypesBuilder)
    {
        return Enum.GetValues<HttpMethodTypeBuilder>().Where(value => httpMethodTypesBuilder.HasFlag(value));
    }

    public static HttpMethodTypeBuilder? GetHttpMethodType(string method)
    {
        return method.ToUpper() switch
        {
            "GET" => HttpMethodTypeBuilder.Get,
            "POST" => HttpMethodTypeBuilder.Post,
            "PUT" => HttpMethodTypeBuilder.Put,
            "PATCH" => HttpMethodTypeBuilder.Patch,
            "DELETE" => HttpMethodTypeBuilder.Delete,
            _ => null
        };
    }

    public static string GetMethodTypesUsing(this HttpMethodTypeBuilder httpMethodTypeBuilder)
    {
        return httpMethodTypeBuilder switch
        {
            HttpMethodTypeBuilder.Get => "HttpMethodType.Get",
            HttpMethodTypeBuilder.Post => "HttpMethodType.Post",
            HttpMethodTypeBuilder.Put => "HttpMethodType.Put",
            HttpMethodTypeBuilder.Patch => "HttpMethodType.Patch",
            HttpMethodTypeBuilder.Delete => "HttpMethodType.Delete",
            _ => throw new ArgumentOutOfRangeException(nameof(httpMethodTypeBuilder), httpMethodTypeBuilder, null)
        };
    }

    public static bool HasBody(this HttpMethodTypeBuilder methodTypeBuilder) => methodTypeBuilder is HttpMethodTypeBuilder.Put or HttpMethodTypeBuilder.Post or HttpMethodTypeBuilder.Patch;
}