using System.Threading.Tasks;

namespace Fiona.IDE.Components.Pages.Project
{
    public interface ICommandRunner
    {
        Task RunCommandAsync(string command, string? workingDirectory = null);
    }
}