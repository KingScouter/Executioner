
namespace Executioner.Models
{
    public abstract class BaseUserInputParameter
    {
        public string Keyword { get; set; }
        public string Name { get; set; }
        public UserInputParameterType Type { get; set; }

        public BaseUserInputParameter(string keyword, string name, UserInputParameterType type)
        {
            Keyword = keyword;
            Name = name;
            Type = type;
        }
    }
}
