using System.Text.RegularExpressions;

namespace Fiona.Hosting.Routing;

internal sealed partial class Url(string url)
{
    public string NormalizeUrl { get; } = NormalizeUrlRegex().Replace(url, "{param}");
    public string OriginalUrl { get; } = url;
    
    public string[] SplitUrl { get; } = url.Split('/');

    [GeneratedRegex(@"\{[^{}]*\}")]
    private static partial Regex NormalizeUrlRegex();
    
    public static implicit operator Url(string url) => new(url);
    
    public Url GetPartOfUrl(int howManyParts)
    {
        string[] parts = OriginalUrl.Split('/');
        return new Url(string.Join('/', parts[..howManyParts]));
    }
}