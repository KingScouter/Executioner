using System.Windows;
using System.Windows.Controls;

namespace Executioner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<CommandData> commands = [
                new CommandData(1, "Title1", "Desc1", "echo Test", true, "", ShellType.Cmd),
                new CommandData(2, "Title2", "Desc2", "echo Wait for me", true, "", ShellType.Cmd),
                new CommandData(3, "Title3", "Desc3", "dir", true, "", ShellType.Cmd),
                new CommandData(4, "Powershell test1", "Test mit PWS", "ls", true, "", ShellType.Powershell)
            ];

            FillDataGrid(commands);
        }

        private void FillDataGrid(List<CommandData> commands)
        {
            CommandsDataGrid.ItemsSource = commands;
        }

        private void ExecuteCommand(object sender, RoutedEventArgs e)
        {
            object context = (sender as Button)!.DataContext;
            if (context is not null && context is CommandData)
            {
                CommandData data = (context as CommandData)!;
                try
                {
                    switch(data.Type)
                    {
                        case ShellType.Cmd:
                            ExecuteCmdCommand(data.Template, data.WaitForResult, data.WorkingDir);
                            break;
                        case ShellType.Powershell:
                            ExecutePowershellCommand(data.Template, data.WaitForResult, data.WorkingDir);
                            break;
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
                
            }
        }

        private void ExecuteCmdCommand(string commandTemplate, Boolean waitForResult, string workingDir)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "cmd.exe";

            if (waitForResult)
                commandTemplate += "& pause";

            if (workingDir != "")
                startInfo.WorkingDirectory = workingDir;

            startInfo.Arguments = $"/C {commandTemplate}";

            ExecuteProcess(startInfo);
        }

        private void ExecutePowershellCommand(string commandTemplate, Boolean waitForResult, string workingDir)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "powershell.exe";

            if (waitForResult)
                commandTemplate += "; pause";

            if (workingDir != "")
                startInfo.WorkingDirectory = workingDir;

            startInfo.Arguments = $"-ExecutionPolicy Bypass \"{commandTemplate}\"";
            startInfo.UseShellExecute = false;

            ExecuteProcess(startInfo);
        }

        private void ExecuteProcess(System.Diagnostics.ProcessStartInfo startInfo)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}