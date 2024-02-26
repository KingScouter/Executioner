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
        public CommandData OutputData { get; set; }

        public CommandEditWindow()
        {
            InitializeComponent();
        }

        public void OnOKButtonClick(object sender, RoutedEventArgs e)
        {
            ShellType type = (ShellType)TypeComboBox.SelectedIndex;

            OutputData = new CommandData(0, NameInputTextBox.Text, DescInputTextBox.Text, 
                TemplateInputTextBox.Text, WaitForResultCheckBox.IsChecked == true, "",
                type);
            DialogResult = true;
            Close();
        }

        public void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
