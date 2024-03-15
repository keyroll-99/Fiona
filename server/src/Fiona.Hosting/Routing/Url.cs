using System.Text.RegularExpressions;

namespace Fiona.Hosting.Routing;

// TODO: 2 objects one fot keep it in routeNode second for request
internal sealed partial class Url: IEquatable<Url>, IEquatable<string>
{
    public string NormalizeUrl { get; }
    public string OriginalUrl { get; }
    public string[] SplitUrl { get; }
    public IEnumerable<int> IndexesOfParameters { get; }

    private const string OpenParameter = "{";
    private const string CloseParameter = "}";
    private const string ParamMark = "{param}";

    private Url(string url)
    {
        NormalizeUrl = NormalizeUrlRegex().Replace(url, ParamMark);
        OriginalUrl = url;
        SplitUrl = url.Split('/');
        IndexesOfParameters = GetIndexesOfParameters();
    }
    
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
        string[] splitUrl = (string[])SplitUrl.Clone();
        foreach (var parameter in other.IndexesOfParameters)
        {
            splitUrl[parameter] = ParamMark;
        }

        return other.NormalizeUrl ==  string.Join('/', splitUrl);
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
        // TODO: Url from api doesn't have {} at parameter, i have to looking for a parameter parent if not found exactly same url
        string[] splitUrl = (string[])SplitUrl.Clone();
        foreach (var parameter in url.IndexesOfParameters)
        {
            splitUrl[parameter] = ParamMark;
        }
        return string.Join('/', splitUrl).StartsWith(url.NormalizeUrl);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Url);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(OriginalUrl);
    }
    
    public HashSet<string> GetUrlParameters()
    {
        HashSet<string> result = [];

        if (!OriginalUrl.Contains(OpenParameter))
        {
            return result;
        }

        int offset = 0;
        while (true)
        {
            int indexOfOpen = OriginalUrl.IndexOf(OpenParameter, offset, StringComparison.Ordinal);
            if (indexOfOpen == -1)
            {
                break;
            }

            int indexOfClose = OriginalUrl.IndexOf(CloseParameter, offset, StringComparison.Ordinal);
            string variableName = OriginalUrl.Substring(indexOfOpen + 1, indexOfClose - indexOfOpen - 1);
            if (!result.Add(variableName))
            {
                throw new ConflictNameOfRouteParameters(OriginalUrl);
            }

            offset = indexOfClose + 1;
        }

        return result;
    }
    
    private IEnumerable<int> GetIndexesOfParameters()
    {
        List<int> result = [];
        List<string> splitNormalizedUrl = NormalizeUrl.Split("/").ToList();
        for (var index = 0; index < splitNormalizedUrl.Count; index++)
        {
            var url = splitNormalizedUrl[index];
            if (url == ParamMark)
            {
                result.Add(index);
            }
        }

        return result;
    }

}