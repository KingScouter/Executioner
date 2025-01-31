namespace Executioner.Models
{
    public class ProjectData
    {
        public List<CommandData> Commands { get; set; } = [];
        public string ProjectName { get; set; } = "";

        public void AddCommand(CommandData command)
        {
            command.UUID = Guid.NewGuid().ToString();
            Commands.Add(command);
        }

        public CommandData? GetCommand(string uuid)
        {
            return Commands.Find(elem => elem.UUID == uuid);
        }

        public CommandData? GetCommandByKeyword(string name)
        {
            return Commands.Find(elem => elem.Keyword.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public bool UpdateCommand(CommandData command)
        {
            int commandIdx = Commands.FindIndex(elem => elem.UUID == command.UUID);
            if (commandIdx != -1)
            {
                Commands.RemoveAt(commandIdx);
                Commands.Insert(commandIdx, command);
                return true;
            }

            return false;
        }

        public bool RemoveCommand(string uuid)
        {
            int commandIdx = Commands.FindIndex(elem => elem.UUID == uuid);
            if (commandIdx != -1)
            {
                Commands.RemoveAt(commandIdx);
                return true;
            }

            return false;
        }
    }
}
