using Executioner.Models;

namespace Executioner.UserInputParameters
{
    public class NumberUserInputParameter : IBaseUserInputParameter
    {
        public NumberUserInputParameter(string keyword, string name) : base(keyword, name, ParameterType.Number)
        {
        }

        public override BaseUserInputControl GetControl()
        {
            return new TextParameterInputControl();
        }
    }
}
