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
    }
}
