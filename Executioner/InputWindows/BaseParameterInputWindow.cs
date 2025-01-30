using Executioner.UserInputParameters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Executioner.InputWindows
{
    /// <summary>
    /// Interaction logic for BaseParameterInputWindow.xaml
    /// </summary>
    public abstract class BaseParameterInputWindow : Window
    {
        private bool isClosing = false;

        public string InputLabel { get; set; } = "";

        public abstract UIElement FocusControl { get; }
        public abstract string OutputValue { get; }

        static BaseParameterInputWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseParameterInputWindow), new FrameworkPropertyMetadata(typeof(BaseParameterInputWindow)));
        }

        public BaseParameterInputWindow(IBaseUserInputParameter param)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Deactivated += BaseParameterInputWindow_Deactivated;
            KeyDown += BaseParameterInputWindow_KeyDown;

            DataContext = this;

            InputLabel = param.Name;

            ContentRendered += BaseParameterInputWindow_ContentRendered;

        }

        /// <summary>
        /// Handler for the ContentRendered-event of the window.
        /// Sets the initial focus on the main control-element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseParameterInputWindow_ContentRendered(object? sender, EventArgs e)
        {
            FocusControl.Focus();
            ContentRendered -= BaseParameterInputWindow_ContentRendered;
        }

        /// <summary>
        /// Handler for the deactivated-event of the window. 
        /// Closes the input-window if the user focuses on anything else.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseParameterInputWindow_Deactivated(object? sender, EventArgs e)
        {
            CloseWindow(false);
        }

        /// <summary>
        /// Handler for the Keydown-event
        /// Closes the window on ESC. Confirms the data and closes the window on ENTER.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BaseParameterInputWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                e.Handled = true;
                bool result = false;

                if (e.Key == Key.Enter)
                {
                    result = true;
                    HandleEnter();
                }

                CloseWindow(result);
            }
        }

        /// <summary>
        /// Closes the window and sets the dialog-result accordingly.
        /// </summary>
        /// <param name="result">Dialog result</param>
        protected void CloseWindow(bool result)
        {
            if (isClosing)
                return;

            isClosing = true;
            DialogResult = result;
            Close();
        }

        /// <summary>
        /// Handler that triggers in case the user confirms the input using the ENTER key.
        /// </summary>
        /// <returns>True if everything is fine and the dialog can be confirmed, otherwise false.</returns>
        protected virtual bool HandleEnter()
        {
            return true;
        }
    }
}
