namespace Fiona.Hosting.Exceptions;

public sealed class ServerAlreadyRunningException : Exception
{
    ServerAlreadyRunningException() : base("Server is already running.")
    {
    }
}