namespace Fiona.Hosting.Routing.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class QueryParamAttribute : Attribute
{
    public QueryParamAttribute()
    {
    }

    public QueryParamAttribute(string name)
    {
        Name = name;
    }

    // TODO: In the future, parameters can have a different name than the property name
    internal string? Name { get; }
}