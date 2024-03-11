using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;
using Executioner.Models;

namespace Executioner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<CommandData> commands = [];
        private int commandIdx = 0;

        public MainWindow(string? filename)
        {
            InitializeComponent();

            CommandsDataGrid.ItemsSource = commands;
            FillDataGrid();

            if (filename != null)
                LoadProject(filename);
        }

        private void FillDataGrid()
        {
            CommandsDataGrid.Items.Refresh();
        }

        private void ExecuteCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                object context = (sender as Button)!.DataContext;
                if (context is not null && context is CommandData)
                {
                    CommandData data = (context as CommandData)!;

                    CommandExecutor.ExecuteCommand(data);
                }
            }
            catch (Exception ex)
            {
                StatusBarTextBox.Text = $"Execution of command cancelled: {ex.Message}";
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Executioner Project (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
                LoadProject(openFileDialog.FileName);

        }

        private void LoadProject(string filename)
        {
            try
            {
                commands.Clear();

                StreamReader sr = new StreamReader(filename);
                string dataLine = sr.ReadToEnd();
                if (dataLine != null)
                {
                    List<CommandData>? parsedCommands = JsonSerializer.Deserialize<List<CommandData>>(dataLine);
                    if (parsedCommands != null)
                    {
                        commands.Clear();
                        commands.AddRange(parsedCommands);
                        FillDataGrid();
                        StatusBarTextBox.Text = $"Loaded project from {filename}";
                    }
                }

                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}