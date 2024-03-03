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
            string? filename = null;
            if (e.Args.Length == 1)
                filename = e.Args[0];

            MainWindow wnd = new(filename);

            wnd.Show();
        }
    }

}
