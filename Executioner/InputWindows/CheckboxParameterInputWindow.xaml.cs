using Executioner.UserInputParameters;
using System.Windows;
using System.Windows.Input;

namespace Executioner.InputWindows
{
    /// <summary>
    /// Interaction logic for CheckboxParameterInputWindow.xaml
    /// </summary>
    public partial class CheckboxParameterInputWindow : BaseParameterInputWindow
    {
        public override string OutputValue { get => ParamCheckBox.IsChecked.ToString(); }
        public override UIElement FocusControl { get => ParamCheckBox; }
        public CheckboxParameterInputWindow(IBaseUserInputParameter param) : base(param)
        {
            InitializeComponent();
        }

        /// <summary>
        /// PreviewKeyDown handler. Catches the ENTER-key to prevent it from toggling the
        /// button and instead close the window.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event data</param>
        private void CheckboxParameterInputWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BaseParameterInputWindow_KeyDown(sender, e);
                return;
            }
        }
    }
}
