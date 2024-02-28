using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;
using System.Windows.Data;

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

        private void EditCommand(object sender, RoutedEventArgs e)
        {
            object context = (sender as Button)!.DataContext;
            if (context is not null && context is CommandData)
            {
                CommandData data = (context as CommandData)!;
                try
                {
                    CommandEditWindow editWindow = new CommandEditWindow(data);
                    if (editWindow.ShowDialog() == true)
                    {
                        CommandData newData = editWindow.OutputData!;
                        int commandIdx = commands.FindIndex(elem => elem.Id == data.Id);
                        if (commandIdx != -1)
                        {
                            commands.RemoveAt(commandIdx);
                            commands.Insert(commandIdx, newData);
                            StatusBarTextBox.Text = $"Updated command at index {commandIdx}";
                            FillDataGrid();
                        }
                    }
                }
                catch (Exception ex)
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

        private void CommonCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Executioner Project (*.json)|*.json";
                if (saveFileDialog.ShowDialog() == true)
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                    sw.Write(JsonSerializer.Serialize(commands));
                    sw.Close();
                    StatusBarTextBox.Text = $"Saved project to {saveFileDialog.FileName}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void LoadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Executioner Project (*.json)|*.json";
                if (openFileDialog.ShowDialog() == true)
                {
                    commands.Clear();

                    StreamReader sr = new StreamReader(openFileDialog.FileName);
                    string dataLine = sr.ReadToEnd();
                    if (dataLine != null)
                    {
                        List<CommandData>? parsedCommands = JsonSerializer.Deserialize<List<CommandData>>(dataLine);
                        if (parsedCommands != null)
                        {
                            commands.Clear();
                            commands.AddRange(parsedCommands);
                            FillDataGrid();
                            StatusBarTextBox.Text = $"Loaded project from {openFileDialog.FileName}";
                        }
                    }

                    sr.Close();
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}