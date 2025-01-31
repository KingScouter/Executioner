using Executioner.Models;
using System.Windows;
using CommandLine;

namespace Executioner
{
    internal class Options
    {
        [Option('p', "project", Required = false, HelpText = "Project to open")]
        public string ProjectFile { get; set; } = "";

        [Option('c', "cmd", Required = false, HelpText = "Command to execute")]
        public string CommandKeyword { get; set; } = "";

        [Option('a', "args", Required = false, HelpText = "Arguments for the command execution")]
        public IEnumerable<string> AdditionalArguments { get; set; } = [];

        [Option('d', "dry-run", Required = false, HelpText = "Print the command without executing it")]
        public bool DryRun { get; set; } = false;

        [Value(0)]
        public IEnumerable<string> Misc { get; set; } = [];

    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Parser.Default.ParseArguments<Options>(e.Args)
                .WithParsed(RunOptions);
        }

        void RunOptions(Options opts)
        {
            try
            {
                if (opts.CommandKeyword == "" || opts.ProjectFile == "")
                {
                    MainWindow wnd = new(opts.ProjectFile);
                    wnd.Show();
                    return;
                }

                List<string> additionalArgs = [];
                if (opts.AdditionalArguments.Any()) 
                    additionalArgs.AddRange(opts.AdditionalArguments);
                else if (opts.Misc.Any())
                    additionalArgs.AddRange(opts.Misc);

                ShutdownMode = ShutdownMode.OnExplicitShutdown;
                ProjectManager manager = new(opts.ProjectFile, opts.CommandKeyword, additionalArgs, opts.DryRun);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured: {ex.Message}");
            }
            Shutdown(0);
        }
    }

}
