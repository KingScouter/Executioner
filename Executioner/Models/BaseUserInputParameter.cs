
using System.Text.Json.Serialization;

namespace Executioner.Models
{
    //[JsonDerivedType(typeof(BaseUserGenericInputParameter<object>), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(TextUserInputParameter), typeDiscriminator: "text")]
    [JsonDerivedType(typeof(NumberUserInputParameter), typeDiscriminator: "number")]
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

        public abstract string Execute();
    }
}
