using Executioner.UserInputParameters;
using System.Windows;
using System.Windows.Controls;

namespace Executioner.InputWindows
{
    /// <summary>
    /// Interaction logic for InputParameterWindow.xaml
    /// </summary>
    public partial class InputParameterWindow : Window
    {
        public string InputLabel { get; set; } = "";
        public string InputTitle { get; set; } = "";

        public string? OutputData
        {
            get
            {
                return activeControl.OutputData;
            }
        }

        private readonly BaseUserInputControl activeControl;
        private readonly IBaseUserInputParameter inputParameter;

        public InputParameterWindow(IBaseUserInputParameter param)
        {
            InitializeComponent();

            inputParameter = param;
            activeControl = param.GetControl();


            DataContext = this;

            InputLabel = param.Name;
            InputTitle = param.Name;

            InitializeControls();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void InitializeControls()
        {
            Grid.SetColumn(activeControl, 1);
            Grid.SetRow(activeControl, 1);

            MainGrid.Children.Add(activeControl);
            activeControl.FocusControl();
        }
    }
}
