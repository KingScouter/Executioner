using System.Windows;
using System.Windows.Controls;

namespace Executioner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<CommandData> commands = [];
        private int commandIdx = 0;

        public MainWindow()
        {
            InitializeComponent();

            AddCommand("Title1", "Desc1", "echo Test", true, "", ShellType.Cmd);
            AddCommand("Title2", "Desc2", "echo Wait for me", true, "", ShellType.Cmd);
            AddCommand("Title3", "Desc3", "dir", true, "", ShellType.Cmd);
            AddCommand("Powershell test1", "Test mit PWS", "ls", true, "", ShellType.Powershell);

            CommandsDataGrid.ItemsSource = commands;

            FillDataGrid();
        }

        private void FillDataGrid()
        {
            CommandsDataGrid.Items.Refresh();
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

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            //AddCommand($"TestNeu{commandIdx}", "Another test", "echo hello", true, "", ShellType.Cmd);
            //FillDataGrid();
            CommandEditWindow editWindow = new CommandEditWindow();
            if (editWindow.ShowDialog() == true)
            {
                AddCommand(editWindow.OutputData);
                FillDataGrid();
            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (commands.Count > 0)
            {
                commands.RemoveAt(commands.Count - 1);
                FillDataGrid();
            }
        }

        private void AddCommand(string name, string desc, string template, bool waitForResult, string workingDir, ShellType type)
        {
            commands.Add(new CommandData(commandIdx++, name, desc, template, waitForResult, workingDir, type));
        }

        private void AddCommand(CommandData command)
        {
            command.Id = commandIdx++;
            commands.Add(command);
        }
    }
}