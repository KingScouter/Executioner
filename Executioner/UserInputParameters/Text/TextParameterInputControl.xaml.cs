namespace Executioner.UserInputParameters
{
    /// <summary>
    /// Interaction logic for TextParameterInputControl.xaml
    /// </summary>
    public partial class TextParameterInputControl : BaseUserInputControl
    {
        public override string OutputData
        {
            get
            {
                return InputTextBox.Text;
            }
        }

        public TextParameterInputControl()
        {
            InitializeComponent();
        }
    }
}
