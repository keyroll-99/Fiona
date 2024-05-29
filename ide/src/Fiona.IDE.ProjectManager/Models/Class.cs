namespace Fiona.IDE.ProjectManager.Models;

public sealed class Class
{
    public IReadOnlyCollection<Dependency> Dependencies => _dependencies.AsReadOnly();
    
    private List<Dependency> _dependencies = new();
    private string _route;
    private List<Endpoint> _endpoints = new();


}