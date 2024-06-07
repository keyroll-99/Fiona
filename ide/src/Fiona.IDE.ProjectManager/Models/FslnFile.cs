using Fiona.IDE.ProjectManager.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Fiona.IDE.ProjectManager.Models
{
    public sealed class FslnFile(string name, List<string> projectFilesPath, string path)
    {
        public string Name { get; private init; } = name;
        public string Path { get; private init; } = path;
        [NotMapped]
        public List<ProjectFile>? ProjectFiles { get; private set; } = [];

        public List<string> ProjectFilePath { get; private init; } = projectFilesPath;
        public static string Extension = ".fsln";

        internal static async Task<FslnFile> CreateAsync(string name, string pathToFolder)
        {
            FslnFile fslnFile = new(name, Enumerable.Empty<string>().ToList(), pathToFolder);
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
            FslnFile? fslnFile = await JsonSerializer.DeserializeAsync<FslnFile>(fs);
            if (fslnFile is null)
            {
                return null;
            }

            IEnumerable<Task<ProjectFile>> loadingTasks = fslnFile.ProjectFilePath.Select(ProjectFile.LoadAsync).ToList();
            await Task.WhenAll(loadingTasks);
            fslnFile.ProjectFiles = loadingTasks.Select(x => x.Result).ToList();
            return fslnFile;
        }

        internal async Task AddFile(string name, string path)
        {
            string filePath = $"{path}{System.IO.Path.DirectorySeparatorChar}{name}.{ProjectFile.Extension}";
            ProjectFiles!.Add(
                 await ProjectFile.CreateAsync(filePath));
            ProjectFilePath.Add(filePath);
            await SaveAsync();
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