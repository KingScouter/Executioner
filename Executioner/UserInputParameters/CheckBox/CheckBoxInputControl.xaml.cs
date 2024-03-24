namespace Executioner.UserInputParameters
{
    /// <summary>
    /// Interaction logic for CheckBoxInputControl.xaml
    /// </summary>
    public partial class CheckBoxInputControl : BaseUserInputControl
    {
        public override string OutputData
        {
            get
            {
                return InputCheckBox.IsChecked == true ? "true" : "false";
            }
        }

        public CheckBoxInputControl()
        {
            InitializeComponent();
        }
    }
}
