using System.Windows;
using Executioner.Models;
using Microsoft.Win32;

namespace Executioner
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CommandEditWindow : Window
    {
        private int commandId = 0;

        private ShellType selectedType = ShellType.Cmd;
        public ShellType SelectedTypeProperty
        {
            get { return selectedType; }
            set { ; }
        }

        public static Dictionary<ShellType, string> NameMapping 
        {
            get { return ShellTypeConverter.NameMapping; }
        }

        public CommandEditWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public CommandEditWindow(CommandData inputData)
        {
            InitializeComponent();
            DataContext = this;

            commandId = inputData.Id;
            NameInputTextBox.Text = inputData.Name;
            DescInputTextBox.Text = inputData.Description;
            TemplateInputTextBox.Text = inputData.Template;
            WaitForResultCheckBox.IsChecked = inputData.WaitForResult;
            WorkingDirTextBox.Text = inputData.WorkingDir;
            selectedType = inputData.Type;

            ParameterGrid.Parameters = inputData.Parameters;
        }

        public void OnOKButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public void OnFileChooserButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            if (WorkingDirTextBox.Text.Length > 0)
                openFolderDialog.InitialDirectory = WorkingDirTextBox.Text;

            if (openFolderDialog.ShowDialog() == true)
                WorkingDirTextBox.Text = openFolderDialog.FolderName;
        }

        public CommandData OutputData 
        { 
            get 
            {
                ShellType type = (ShellType)TypeComboBox.SelectedIndex;

                return new CommandData(commandId, NameInputTextBox.Text, DescInputTextBox.Text,
                    TemplateInputTextBox.Text, WaitForResultCheckBox.IsChecked == true, WorkingDirTextBox.Text,
                    type, ParameterGrid.Parameters);
            } 
        }
    }
}
