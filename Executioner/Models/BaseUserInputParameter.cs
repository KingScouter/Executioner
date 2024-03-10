
using System.Text.Json.Serialization;

namespace Executioner.Models
{
    [JsonDerivedType(typeof(BaseUserInputParameter), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(TextUserInputParameter), typeDiscriminator: "text")]
    public class BaseUserInputParameter
    {
        public string Keyword { get; set; }
        public string Name { get; set; }
        public ParameterType Type { get; set; }

        public BaseUserInputParameter(string keyword, string name, ParameterType type)
        {
            Keyword = keyword;
            Name = name;
            Type = type;
        }
    }
}
