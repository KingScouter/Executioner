using Executioner.UserInputParameters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Executioner.InputWindows
{
    /// <summary>
    /// Interaction logic for TextParameterInputWindow.xaml
    /// </summary>
    public partial class TextParameterInputWindow : BaseParameterInputWindow
    {

        public override string OutputValue { get => ParamTextBox.Text; }

        public override Control FocusControl { get => ParamTextBox; }

        public TextParameterInputWindow(IBaseUserInputParameter param): base(param)
        {
            InitializeComponent();
        }
    }
}
