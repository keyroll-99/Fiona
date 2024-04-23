using System;

namespace Fiona.IDE.Components.Pages.Project.Exceptions
{
    public class ProjectNotFoundException(string path) : Exception($"Project {path} not found")
    {
        
    }
}