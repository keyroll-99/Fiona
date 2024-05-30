using Fiona.IDE.Tokenizer;

namespace Fiona.IDE.ProjectManager.Models;

public sealed class Class
{
    public IReadOnlyCollection<Dependency> Dependencies => _dependencies.AsReadOnly();
    public string Namespace => _namespace;
    public string Route => _route;
    public IReadOnlyCollection<Endpoint> Endpoints => _endpoints.AsReadOnly();
    public IReadOnlyCollection<string> Usings => _usings.AsReadOnly();
    public string Name => _name;

    private List<Dependency> _dependencies;
    private string _route;
    private List<Endpoint> _endpoints;
    private List<string> _usings;
    private string _name;
    private string _namespace;

    private Class(string name, string @namespace, List<Dependency> dependencies, string route, List<Endpoint> endpoints, List<string> usings)
    {
        _name = name;
        _dependencies = dependencies;
        _route = route;
        _endpoints = endpoints;
        _usings = usings;
        _namespace = @namespace;
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
        if(classRoute is not null &&  indexOfClassRouteToken > indexOfStartEndpointsSearch)
        {
            indexOfStartEndpointsSearch = indexOfClassRouteToken;
        }
        
        
        return new Class(classToken.Value!, @namespace, dependencies, classRoute?.Value ?? string.Empty, endpoints, usings.Select(x => x.Value!).ToList());
    }
}