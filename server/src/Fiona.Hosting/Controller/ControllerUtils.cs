using System.Reflection;
using Fiona.Hosting.Controller.Attributes;

namespace Fiona.Hosting.Controller;

internal static class ControllerUtils
{
    public static IEnumerable<Type> GetControllers(Assembly assembly)
    {
        var types = assembly.GetTypes();
        foreach (Type type in types)
            if (type.GetCustomAttributes(typeof(ControllerAttribute), true).Length > 0)
                yield return type;
    }
}