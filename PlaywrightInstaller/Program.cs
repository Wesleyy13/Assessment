using System;
using System.Diagnostics;
using System.Threading.Tasks;

internal class Program
{
    private static async Task<int> Main()
    {
        try
        {
            Console.WriteLine("Running 'playwright install' using the Playwright CLI...");

            // Ensure the Playwright CLI runs with the project directory as the working directory
            var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var projectDir = System.IO.Path.GetDirectoryName(assemblyLocation)!;
            // walk up 4 levels from output (net10.0 -> Debug -> bin -> solution root)
            for (var i = 0; i < 4; ++i)
            {
                var parent = System.IO.Directory.GetParent(projectDir);
                if (parent == null) break;
                projectDir = parent.FullName;
            }

            // ensure Playwright CLI runs in the PlaywrightInstaller project folder
            projectDir = System.IO.Path.Combine(projectDir, "PlaywrightInstaller");

            Console.WriteLine($"Computed projectDir: {projectDir}");
            Console.WriteLine($"CurrentDirectory: {Environment.CurrentDirectory}");

            var psi = new ProcessStartInfo
            {
                FileName = "playwright",
                Arguments = "install",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = projectDir,
            };

            using var proc = Process.Start(psi)!;
            if (proc == null)
            {
                Console.Error.WriteLine("Failed to start the Playwright CLI process.");
                return 1;
            }

            // Read output streams asynchronously
            var stdoutTask = proc.StandardOutput.ReadToEndAsync();
            var stderrTask = proc.StandardError.ReadToEndAsync();

            await Task.WhenAll(stdoutTask, stderrTask);

            var stdout = stdoutTask.Result;
            var stderr = stderrTask.Result;

            Console.WriteLine(stdout);
            if (!string.IsNullOrWhiteSpace(stderr))
                Console.Error.WriteLine(stderr);

            proc.WaitForExit();

            Console.WriteLine($"Playwright CLI exited with code {proc.ExitCode}");
            return proc.ExitCode;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Playwright install failed: {ex.Message}");
            return 1;
        }
    }
}
