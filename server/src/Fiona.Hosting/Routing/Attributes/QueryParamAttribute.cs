namespace Fiona.Hosting.Routing.Attributes;

[AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
public class QueryParamAttribute : Attribute
{
    // TODO: In the future, parameters can have a different name than the property name
    internal string? Name { get; }

    public QueryParamAttribute()
    {
    }

    public QueryParamAttribute(string name)
    {
        Name = name;
    }
}