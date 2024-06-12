using CompilerModel=Fiona.Compiler.Parser.Models;
using ProjectManagerModel=Fiona.IDE.ProjectManager.Models;

namespace Fiona.IDE.ProjectManager.Mappers;

public static class ProjectFileMapper
{
    public static CompilerModel.ProjectFile AsCompilerModel(this ProjectManagerModel.ProjectFile projectFile)
        => new(projectFile.Name, projectFile.Path);
}