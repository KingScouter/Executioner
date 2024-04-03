namespace Executioner.Models
{
    public class ProjectData
    {
        private static int commandIdx = 0;

        public List<CommandData> Commands { get; set; } = [];
        public string ProjectName { get; set; } = "";

        public void AddCommand(CommandData command)
        {
            command.Id = commandIdx++;
            Commands.Add(command);
        }

        public CommandData? GetCommand(int id)
        {
            return Commands.Find(elem => elem.Id == id);
        }

        public CommandData? GetCommandByName(string name)
        {
            return Commands.Find(elem => elem.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public bool UpdateCommand(CommandData command)
        {
            int commandIdx = Commands.FindIndex(elem => elem.Id == command.Id);
            if (commandIdx != -1)
            {
                Commands.RemoveAt(commandIdx);
                Commands.Insert(commandIdx, command);
                return true;
            }

            return false;
        }

        public bool RemoveCommand(int id)
        {
            int commandIdx = Commands.FindIndex(elem => elem.Id == id);
            if (commandIdx != -1)
            {
                Commands.RemoveAt(commandIdx);
                return true;
            }

            return false;
        }
    }
}
