using Fiona.IDE.Project.Models;
using System.Text.Json;

namespace Fiona.IDE.Project
{
    public class ProjectManager : IProjectManager
    {

        private FslnFile? Project { get; set; }

        public async Task<string> CreateProject(string path, string name)
        {
            string fullPath = $"{path}/{name}.fsln";
            if (File.Exists(fullPath))
            {
                throw new ProjectAlreadyExistsException(fullPath);
            }

            FslnFile fslnFile = new(name, Enumerable.Empty<string>());
            await using Stream fslnFileStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(fslnFileStream, fslnFile);
            await using FileStream fileStream = File.Create(fullPath);

            fslnFileStream.Seek(0, SeekOrigin.Begin);
            await fslnFileStream.CopyToAsync(fileStream);
            return fullPath;
        }

        public async Task LoadProject(string path)
        {

            await using FileStream fs = new(path, FileMode.Open, FileAccess.Read);

            Project = await JsonSerializer.DeserializeAsync<FslnFile>(fs);
        }
    }
}