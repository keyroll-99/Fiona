namespace Fiona.Hosting.Exceptions;

public class ServerAlreadyRunningException : Exception
{
    ServerAlreadyRunningException() : base("Server is already running.")
    {
    }
}