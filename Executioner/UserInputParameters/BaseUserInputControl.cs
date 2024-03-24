using System.Windows.Controls;

namespace Executioner.UserInputParameters
{
    public abstract partial class BaseUserInputControl : UserControl
    {
        public abstract string OutputData { get; }
    }
}
