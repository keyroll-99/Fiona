using Fiona.Compiler.Tokenizer;

namespace Fiona.Compiler.ProjectManager.Models;

public sealed class Class
{
    public IReadOnlyCollection<Dependency> Dependencies => _dependencies.AsReadOnly();
    public string Namespace { get; set; }

    public string Route { get; set; }

    public IReadOnlyCollection<Endpoint> Endpoints => _endpoints.AsReadOnly();
    public IReadOnlyCollection<string> Usings => _usings.AsReadOnly();
    public string Name { get; set; }

    private List<Dependency> _dependencies;
    private List<Endpoint> _endpoints;
    private List<string> _usings;

    private Class(string name, string @namespace, List<Dependency> dependencies, string route, List<Endpoint> endpoints, List<string> usings)
    {
        Name = name;
        _dependencies = dependencies;
        Route = route;
        _endpoints = endpoints;
        _usings = usings;
        Namespace = @namespace;
    }

    public static async Task<Class> Load(string path)
    {
        await using FileStream file = File.Open(path, FileMode.Open);
        using StreamReader reader = new(file);
        IReadOnlyCollection<IToken> tokens = await Tokenizer.Tokenizer.GetTokensAsync(reader);

        List<Dependency> dependencies = [];
        List<Endpoint> endpoints = [];

        string @namespace = tokens.GetNamespaceToken()?.Value!;
        (List<IToken> usings, int indexOfUsingEndToken) = tokens.GetUsingTokens();
        (IToken classToken, int indexOfClassToken) = tokens.GetClassToken(indexOfUsingEndToken);
        (IToken? classDependency, int indexOfDependencyToken) = tokens.GetClassDependency(indexOfClassToken);
        (IToken? classRoute, int indexOfClassRouteToken) = tokens.GetClassRoute(indexOfClassToken);
        int indexOfStartEndpointsSearch = indexOfClassToken;
        if (classDependency is not null)
        {
            dependencies = Dependency.GetDependenciesFromToken(classDependency);
            indexOfStartEndpointsSearch = indexOfDependencyToken;
        }
        if (classRoute is not null && indexOfClassRouteToken > indexOfStartEndpointsSearch)
        {
            indexOfStartEndpointsSearch = indexOfClassRouteToken;
        }

        while (indexOfStartEndpointsSearch < tokens.Count)
        {
            (Endpoint? endpoint, int endOfSearch) = Endpoint.GetNextEndpoint(tokens, indexOfStartEndpointsSearch);
            if (endpoint is null)
            {
                break;
            }
            endpoints.Add(endpoint);
            indexOfStartEndpointsSearch = endOfSearch + 1;
        }


        return new Class(classToken.Value!, @namespace, dependencies, classRoute?.Value ?? string.Empty, endpoints, usings.Select(x => x.Value!).ToList());
    }

    public override string ToString()
        => $"class {Name}";
}