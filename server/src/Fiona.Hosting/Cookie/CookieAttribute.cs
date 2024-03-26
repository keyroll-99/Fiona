namespace Fiona.Hosting.Cookie;

[AttributeUsage(AttributeTargets.Parameter)]
public class CookieAttribute : Attribute
{
    public string? Name { get; }

    public CookieAttribute(string name)
    {
        Name = name;
    }

    public CookieAttribute()
    {
        Name = null;
    }
}