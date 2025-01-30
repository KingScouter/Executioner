using Executioner.UserInputParameters;
using System.Windows;
using System.Windows.Input;

namespace Executioner.InputWindows
{
    /// <summary>
    /// Interaction logic for TextParameterInputWindow.xaml
    /// </summary>
    public partial class TextParameterInputWindow : BaseParameterInputWindow
    {

        public string InputLabel { get; set; } = "";
        public string OutputData { get; private set; } = "";


        public TextParameterInputWindow(IBaseUserInputParameter param)
        {
            InitializeComponent();

            DataContext = this;
            InputLabel = param.Name;

            ParamTextBox.Focus();
        }

        protected override void HandleEnter()
        {
            OutputData = ParamTextBox.Text;
        }
    }
}
