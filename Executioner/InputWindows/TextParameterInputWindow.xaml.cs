using Executioner.UserInputParameters;
using System.Windows;
using System.Windows.Input;

namespace Executioner.InputWindows
{
    /// <summary>
    /// Interaction logic for TextParameterInputWindow.xaml
    /// </summary>
    public partial class TextParameterInputWindow : Window
    {
        private bool isClosing = false;

        public string InputLabel { get; set; } = "";
        public string OutputData { get; private set; } = "";


        public TextParameterInputWindow(IBaseUserInputParameter param)
        {
            InitializeComponent();

            DataContext = this;
            InputLabel = param.Name;

            ParamTextBox.Focus();
        }

        private void TextParameterInputWindow_Deactivated(object? sender, EventArgs e)
        {
            CloseWindow(false);
        }

        private void TextParameterInputWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                CloseWindow(false);
            else if (e.Key == Key.Enter)
            {
                OutputData = ParamTextBox.Text;
                CloseWindow(true);
            }
        }

        private void CloseWindow(bool result)
        {
            if (isClosing)
                return;

            isClosing = true;
            DialogResult = result;
            Close();
        }
    }
}
