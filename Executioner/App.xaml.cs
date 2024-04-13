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
        public string CommandName { get; set; } = "";

        [Option('a', "args", Required = false, HelpText = "Arguments for the command execution")]
        public IEnumerable<string> AdditionalArguments { get; set; } = [];

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
                if (opts.CommandName == "" || opts.ProjectFile == "")
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
                ProjectManager manager = new(opts.ProjectFile, opts.CommandName, additionalArgs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured: {ex.Message}");
            }
            Shutdown(0);
        }
    }

}
