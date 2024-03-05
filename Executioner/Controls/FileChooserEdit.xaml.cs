using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Executioner.Controls
{
    /// <summary>
    /// Interaction logic for FileChooserEdit.xaml
    /// </summary>
    public partial class FileChooserEdit : UserControl
    {
        public string Text
        {
            get
            {
                return FileChooserTextBox.Text;
            }
            set
            {
                FileChooserTextBox.Text = value;
            }
        }
        public FileChooserEdit()
        {
            InitializeComponent();
        }

        public void OnFileChooserButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            if (Text.Length > 0)
                openFolderDialog.InitialDirectory = Text;

            if (openFolderDialog.ShowDialog() == true)
                Text = openFolderDialog.FolderName;
        }
    }
}
