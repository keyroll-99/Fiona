namespace Fiona.Hosting.Cookie;

[AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
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