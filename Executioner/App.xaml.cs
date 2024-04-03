using Executioner.Models;
using System.Windows;

namespace Executioner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                string? filename = null;
                string? commandName = null;
                List<string> additionalArguments = [];

                int idx = -1;
                foreach (string item in e.Args)
                {
                    idx++;
                    switch (idx)
                    {
                        case 0:
                            filename = item;
                            break;
                        case 1:
                            commandName = item;
                            break;
                        default:
                            additionalArguments.Add(item);
                            break;
                    }
                }

                if (e.Args.Length <= 1)
                {
                    MainWindow wnd = new(filename);
                    wnd.Show();
                    return;
                }

                ShutdownMode = ShutdownMode.OnExplicitShutdown;
                ProjectManager manager = new(filename!, commandName!, additionalArguments);

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception occured: {ex.Message}");
            }
            Shutdown(0);
        }
    }

}
