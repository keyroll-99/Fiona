using System;

namespace Fiona.IDE.Components.Pages.Project.Exceptions
{
    internal class ProjectAlreadyExistsException(string? name) : Exception($"Project {name} already exists");

}