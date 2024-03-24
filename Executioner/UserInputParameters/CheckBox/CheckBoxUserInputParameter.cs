using Executioner.Models;

namespace Executioner.UserInputParameters
{
    public class CheckBoxUserInputParameter : IBaseUserInputParameter
    {
        public CheckBoxUserInputParameter(string keyword, string name) : base(keyword, name, ParameterType.Number)
        {
        }

        public override BaseUserInputControl GetControl()
        {
            return new CheckBoxInputControl();
        }
    }
}
