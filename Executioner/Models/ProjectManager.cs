using System.IO;
using System.Text.Json;

namespace Executioner.Models
{
    public class ProjectManager
    {
        private ProjectData project;
        private string filename = "";

        public List<CommandData> Commands { get { return project.Commands; } }

        public string Filename { get { return filename; } }
        public string ProjectName {  get { return project.ProjectName; } }

        public ProjectManager(string? filename)
        {
            if (filename != null)
                project = LoadProject(filename);
            
            if (project == null)
            {
                project = new ProjectData();
                project.ProjectName = "New project";
            }
        }

        public ProjectManager(string filename, string commandName, List<string> additionalArguments)
        {
            project = LoadProject(filename);
            if (project == null)
                throw new ArgumentException($"Project {filename} could not be loaded!");

            CommandData? command = project.GetCommandByName(commandName);
            if (command == null)
                throw new ArgumentException($"Command {commandName} could not be found!");

            CommandExecutor.ExecuteCommand(command);
        }


        public ProjectData? LoadProject(string filename)
        {
            this.filename = filename;
            StreamReader sr = new(filename);
            string dataLine = sr.ReadToEnd();
            sr.Close();
            if (dataLine != null)
                return JsonSerializer.Deserialize<ProjectData>(dataLine);

            return null;
        }

        public void SaveProject(string? filename)
        {
            if (filename != null)
                this.filename = filename;

            StreamWriter sw = new StreamWriter(this.filename);
            sw.Write(JsonSerializer.Serialize(project));
            sw.Close();
        }


        public void AddCommand(CommandData command)
        {
            project.AddCommand(command);
        }

        public CommandData? GetCommand(int id)
        {
            return project.GetCommand(id);
        }

        public CommandData? GetCommandByName(string name)
        {
            return project.GetCommandByName(name);
        }

        public bool UpdateCommand(CommandData command)
        {
            return project.UpdateCommand(command);
        }

        public bool RemoveCommand(int id)
        {
            return project.RemoveCommand(id);
        }
    }
}
