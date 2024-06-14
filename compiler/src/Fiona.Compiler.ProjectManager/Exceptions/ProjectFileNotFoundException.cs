namespace Fiona.Compiler.ProjectManager.Exceptions;

public class ProjectFileNotFoundException(string @namespace) : Exception($"Project with namespace {@namespace} not found")
{

}