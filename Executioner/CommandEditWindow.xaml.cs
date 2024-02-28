using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Executioner
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CommandEditWindow : Window
    {
        private int commandId = 0;

        private ShellType SelectedType = ShellType.Cmd;
        public ShellType SelectedTypeProperty
        {
            get { return SelectedType; }
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
            SelectedType = inputData.Type;
        }

        public void OnOKButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public CommandData OutputData 
        { 
            get 
            {
                ShellType type = (ShellType)TypeComboBox.SelectedIndex;

                return new CommandData(commandId, NameInputTextBox.Text, DescInputTextBox.Text,
                    TemplateInputTextBox.Text, WaitForResultCheckBox.IsChecked == true, "",
                    type);
            } 
        }
    }
}
