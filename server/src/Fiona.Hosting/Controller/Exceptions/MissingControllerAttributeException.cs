namespace Fiona.Hosting.Controller;

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