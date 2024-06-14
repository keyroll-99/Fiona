namespace Fiona.Compiler.ProjectManager.Exceptions;

public sealed class ProjectAlreadyExistsException(string? name) : Exception($"Project {name} already exists");