using System.Windows;
using System.Windows.Input;

namespace Executioner.InputWindows
{
    /// <summary>
    /// Interaction logic for BaseParameterInputWindow.xaml
    /// </summary>
    public abstract class BaseParameterInputWindow : Window
    {
        private bool isClosing = false;

        static BaseParameterInputWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseParameterInputWindow), new FrameworkPropertyMetadata(typeof(BaseParameterInputWindow)));
        }

        public BaseParameterInputWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Deactivated += BaseParameterInputWindow_Deactivated;
            KeyDown += BaseParameterInputWindow_KeyDown;
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
        protected abstract void HandleEnter();
    }
}
