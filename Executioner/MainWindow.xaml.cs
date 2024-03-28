using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Executioner.Models;

namespace Executioner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProjectManager project;

        public MainWindow(string? filename)
        {
            InitializeComponent();

            InitializeProject(filename);

            CommandsDataGrid.ItemsSource = project!.Commands;
            FillDataGrid();
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

        private CommandData? GetCommandFromButtonContext(Button? sender)
        {
            if (sender == null)
                return null;

            object context = sender.DataContext;
            if (context is not null && context is CommandData)
            {
                CommandData data = (context as CommandData)!;
                return project.GetCommand(data.Id);
            }

            return null;
        }

        private void EditCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                CommandData? data = GetCommandFromButtonContext(sender as Button);
                if (data == null)
                    return;

                CommandData? commandData = project.GetCommand(data.Id);
                if (commandData == null)
                    return;

                CommandEditWindow editWindow = new CommandEditWindow(commandData);
                if (editWindow.ShowDialog() == true)
                {
                    CommandData newData = editWindow.OutputData!;
                    if (project.UpdateCommand(newData))
                    {
                        StatusBarTextBox.Text = $"Updated command";
                        FillDataGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
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
            CommandData? data = GetCommandFromButtonContext(sender as Button);
            if (data == null)
                return;

            if (project.RemoveCommand(data.Id))
                FillDataGrid();
            
        }

        private void AddCommand(CommandData command)
        {
            project.AddCommand(command);
        }

        private void CommonCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveProject(false);
        }

        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveProject(true);
        }

        private void SaveProject(bool saveAs)
        {
            try
            {
                string filename = project.Filename;

                if (filename == null)
                    saveAs = true;

                if (saveAs)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Executioner Project (*.json)|*.json";
                    if (saveFileDialog.ShowDialog() == true)
                        filename = saveFileDialog.FileName;
                }

                project.SaveProject(filename);
                StatusBarTextBox.Text = $"Saved project to {filename}";
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
                    project.LoadProject(openFileDialog.FileName);
                    StatusBarTextBox.Text = $"Project {openFileDialog.FileName} loaded successfully";
                }
            }
            catch (Exception ex) 
            {
                StatusBarTextBox.Text = $"Loading project failed: {ex.Message}";
            }
        }

        private void InitializeProject(string? filename)
        {
            try
            {
                project = new ProjectManager(filename);
                FillDataGrid();
                StatusBarTextBox.Text = $"Loaded project from {filename}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
}
}