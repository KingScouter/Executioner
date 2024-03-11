using System.Windows;

namespace Executioner.InputWindows
{
    /// <summary>
    /// Interaction logic for TextInputParameterWindow.xaml
    /// </summary>
    public partial class TextInputParameterWindow : Window
    {
        public string InputLabel { get; set; } = "";
        public string InputTitle { get; set; } = "";

        public string OutputData
        {
            get
            {
                return InputTextBox.Text;
            }
        }

        public TextInputParameterWindow(string inputLabel, string inputTitle)
        {
            InitializeComponent();

            DataContext = this;

            InputLabel = inputLabel;
            InputTitle = inputTitle;

            InputTextBox.Focus();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
