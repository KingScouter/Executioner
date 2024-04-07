using System.Text.Json.Serialization;
using Executioner.Models;

namespace Executioner.UserInputParameters
{
    [JsonDerivedType(typeof(TextUserInputParameter), typeDiscriminator: "text")]
    [JsonDerivedType(typeof(NumberUserInputParameter), typeDiscriminator: "number")]
    [JsonDerivedType(typeof(CheckBoxUserInputParameter), typeDiscriminator: "checkbox")]
    public abstract class IBaseUserInputParameter
    {
        public string Keyword { get; set; }
        public string Name { get; set; }
        public ParameterType Type { get; set; }

        public IBaseUserInputParameter(string keyword, string name, ParameterType type)
        {
            Keyword = keyword;
            Name = name;
            Type = type;
        }

        public abstract BaseUserInputControl GetControl();
    }
}
