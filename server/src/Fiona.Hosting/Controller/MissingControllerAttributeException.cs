using Fiona.Hosting.Controller;

namespace Fiona.Hosting.Exceptions;

public sealed class MissingControllerAttributeException() : Exception("Cannot build router without controller attribute.")
{
    public static void ThrowIfNull(ControllerAttribute? controllerAttribute)
    {
        if (controllerAttribute is null)
        {
            throw new MissingControllerAttributeException();
        }
    }
}