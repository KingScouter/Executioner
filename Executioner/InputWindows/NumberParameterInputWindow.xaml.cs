using Executioner.UserInputParameters;
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
using System.Windows.Shapes;

namespace Executioner.InputWindows
{
    /// <summary>
    /// Interaction logic for NumberParameterInputWindow.xaml
    /// </summary>
    public partial class NumberParameterInputWindow : BaseParameterInputWindow
    {
        public override string OutputValue { get => ParamTextBox.Text; }
        public override UIElement FocusControl { get => ParamTextBox; }
        public NumberParameterInputWindow(IBaseUserInputParameter param): base(param)
        {
            InitializeComponent();
        }
    }
}
