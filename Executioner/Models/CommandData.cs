using Executioner.UserInputParameters;
using System.Text.Json.Serialization;

namespace Executioner.Models
{
    public class CommandData
    {
        public string UUID { get; set; }
        public string Keyword { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Template { 
            get 
            {
                return commandTemplate.ToString();
            }
            set
            {
                commandTemplate.ParseTemplate(value);
            }
        }
        public bool WaitForResult { get; set; }
        public bool RunAsAdmin { get; set; }
        public string WorkingDir { get; set; }
        public ShellType Type { get; set; }
        public Dictionary<string, IBaseUserInputParameter> Parameters { get; set; } = [];

        [JsonIgnore]
        public CommandTemplate CommandTemplate
        {
            get
            {
                return commandTemplate;
            }
        }

        private CommandTemplate commandTemplate = new("");

        public CommandData(string uuid, string keyword, string name, string description, string template,
            bool waitForResult, string workingDir, bool runAsAdmin, ShellType type,
            Dictionary<string, IBaseUserInputParameter> parameters)
        {
            UUID = uuid;
            Keyword = keyword;
            Name = name;
            Description = description;
            Template = template; ;
            WaitForResult = waitForResult;
            WorkingDir = workingDir;
            RunAsAdmin = runAsAdmin;
            Type = type;
            Parameters = parameters;
        }

        public void Validate()
        {
            var templateParams = commandTemplate.GetParamElements();
            foreach(var param in templateParams)
            {
                if (!Parameters.ContainsKey(param.keyword))
                    throw new ArgumentException($"Parameter with keyword {param.keyword} not found!");
            }
        }
    }
}
