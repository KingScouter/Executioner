using Executioner.UserInputParameters;

namespace Executioner.Models
{
    public class CommandData
    {
        public string UUID { get; set; }
        public string Keyword { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Template { get; set; }
        public bool WaitForResult { get; set; }
        public string WorkingDir { get; set; }
        public ShellType Type { get; set; }
        public List<IBaseUserInputParameter> Parameters { get; set; } = [];

        public CommandData(string uuid, string keyword, string name, string description, string template,
            bool waitForResult, string workingDir, ShellType type,
            List<IBaseUserInputParameter> parameters)
        {
            UUID = uuid;
            Keyword = keyword;
            Name = name;
            Description = description;
            Template = template; ;
            WaitForResult = waitForResult;
            WorkingDir = workingDir;
            Type = type;
            Parameters = parameters;
        }
    }
}
