using System.Text.RegularExpressions;

namespace Fiona.Hosting.Routing;

internal sealed partial class Url(string url) : IEquatable<Url>, IEquatable<string>
{
    public string NormalizeUrl { get; } = NormalizeUrlRegex().Replace(url, "{param}");
    public string OriginalUrl { get; } = url;
    public string[] SplitUrl { get; } = url.Split('/');
    public HashSet<string> ParametersName { get; } = GetParameters(url); // string, Guid, primitive types

    private const string OpenParameter = "{";
    private const string CloseParameter = "}";

    public Url GetPartOfUrl(int howManyParts)
    {
        string[] parts = OriginalUrl.Split('/');
        return new Url(string.Join('/', parts[..howManyParts]));
    }

    public static implicit operator Url(string url) => new(url);

    [GeneratedRegex(@"\{[^{}]*\}")]
    private static partial Regex NormalizeUrlRegex();

    public bool Equals(Url? other)
    {
        if (other is null)
        {
            return false;
        }

        return NormalizeUrl == other.NormalizeUrl;
    }

    public bool Equals(string? other)
    {
        if (other is null)
        {
            return false;
        }

        return OriginalUrl == other;
    }

    public bool ContainsIn(Url url)
    {
        return NormalizeUrl.StartsWith(url.NormalizeUrl);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Url);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(OriginalUrl);
    }

    private static HashSet<string> GetParameters(string url)
    {
        HashSet<string> result = [];

        if (!url.Contains(OpenParameter))
        {
            return result;
        }

        int offset = 0;
        while (true)
        {
            int indexOfOpen = url.IndexOf(OpenParameter, offset, StringComparison.Ordinal);
            if (indexOfOpen == -1)
            {
                break;
            }

            int indexOfClose = url.IndexOf(CloseParameter, offset, StringComparison.Ordinal);
            string variableName = url.Substring(indexOfOpen + 1, indexOfClose - indexOfOpen - 1);
            if (!result.Add(variableName))
            {
                throw new ConflictNameOfRouteParameters(url);
            }

            offset = indexOfClose + 1;
        }

        return result;
    }
}