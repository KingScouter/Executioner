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

        private List<BaseUserInputParameter> mockParameters = [
            new TextUserInputParameter("test1", "test1"),
            new TextUserInputParameter("test2", "test2"),
            new TextUserInputParameter("test3", "test3"),
        ];

        public List<BaseUserInputParameter> MockParametersProperty 
        { 
            get { return mockParameters; } 
            set { ; }
        }

        public static Dictionary<ShellType, string> NameMapping 
        {
            get { return ShellTypeConverter.NameMapping; }
        }

        public CommandEditWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            ParameterGrid.Parameters = mockParameters;
        }

        public CommandEditWindow(CommandData inputData)
        {
            InitializeComponent();
            this.DataContext = this;

            commandId = inputData.Id;
            NameInputTextBox.Text = inputData.Name;
            DescInputTextBox.Text = inputData.Description;
            TemplateInputTextBox.Text = inputData.Template;
            WaitForResultCheckBox.IsChecked = inputData.WaitForResult;
            WorkingDirTextBox.Text = inputData.WorkingDir;
            selectedType = inputData.Type;

            ParameterGrid.Parameters = mockParameters;
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
                    type);
            } 
        }
    }
}
