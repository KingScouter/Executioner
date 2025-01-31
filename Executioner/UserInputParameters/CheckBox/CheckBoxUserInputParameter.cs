using Executioner.Models;

namespace Executioner.UserInputParameters
{
    public class CheckBoxUserInputParameter : IBaseUserInputParameter
    {
        public CheckBoxUserInputParameter(string keyword, string name) : base(keyword, name, ParameterType.Checkbox)
        {
        }

        public override BaseUserInputControl GetControl()
        {
            return new CheckBoxInputControl();
        }
    }
}
