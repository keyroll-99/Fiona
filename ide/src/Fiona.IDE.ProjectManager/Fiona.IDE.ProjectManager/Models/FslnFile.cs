using Fiona.IDE.ProjectManager.Exceptions;
using System.Text.Json;

namespace Fiona.IDE.ProjectManager.Models
{
    public sealed class FslnFile(string name, List<ProjectFile> projectFileUrl, string path)
    {
        public string Name { get; private init; } = name;
        public string Path { get; private init; } = path;
        public List<ProjectFile>? ProjectFileUrl { get; private init; } = projectFileUrl;
        public static string Extension = ".fsln";

        internal static async Task<FslnFile> CreateAsync(string name, string pathToFolder)
        {
            FslnFile fslnFile = new(name, Enumerable.Empty<ProjectFile>().ToList(), pathToFolder);
            await fslnFile.SaveAsync();
            return fslnFile;
        }

        internal static async Task<FslnFile?> LoadAsync(string path)
        {
            string? filePath = Directory.GetFiles(path, $"*{Extension}").FirstOrDefault();
            if (filePath is null)
            {
                throw new ProjectNotFoundException(path);
            }

            await using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read);
            return await JsonSerializer.DeserializeAsync<FslnFile>(fs);
        }

        internal Task AddFile(string name, string path)
        {
            ProjectFileUrl!.Add(
                ProjectFile.Create($"{path}{System.IO.Path.DirectorySeparatorChar}{name}.{ProjectFile.Extension}"));
            return SaveAsync();
        }

        private async Task SaveAsync()
        {
            await using FileStream fs = new($"{Path}{System.IO.Path.DirectorySeparatorChar}{Name}{Extension}",
                FileMode.Create);
            await JsonSerializer.SerializeAsync(fs, this);
            fs.Close();
        }
    }
}