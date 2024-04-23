using Fiona.IDE.Components.Pages.Project;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Fiona.IDE.Platforms.Windows
{
    public class CommandRunner : ICommandRunner
    {
        public async Task RunCommandAsync(string command, string? workingDirectory = null)
        {

            ProcessStartInfo processStartInfo = new()
            {
                FileName = "powershell.exe",
                Arguments = $"-Command \"{command}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WorkingDirectory = workingDirectory
            };

            using Process process = new();
            process.StartInfo = processStartInfo;
            process.Start();
            await process.WaitForExitAsync();
        }

    }
}