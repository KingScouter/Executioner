using Executioner.Models;

namespace Executioner.UserInputParameters
{
    public class TextUserInputParameter : IBaseUserInputParameter
    {
        public TextUserInputParameter(string keyword, string name) : base(keyword, name, ParameterType.Text)
        {
        }

        public override BaseUserInputControl GetControl()
        {
            return new TextParameterInputControl();
        }
    }
}
