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
                LoadProject(filename);
            
            if (project == null)
            {
                project = new ProjectData();
                project.ProjectName = "New project";
            }
        }

        public ProjectManager(string filename, string keyword, List<string> additionalArguments, bool dryRun)
        {
            LoadProject(filename);
            if (project == null)
                throw new ArgumentException($"Project {filename} could not be loaded!");

            CommandData? command = project.GetCommandByKeyword(keyword);
            if (command == null)
                throw new ArgumentException($"Command with keyword {keyword} could not be found!");

            CommandExecutor.ExecuteCommand(command, additionalArguments, dryRun);
        }


        public void LoadProject(string filename)
        {
            this.filename = filename;
            StreamReader sr = new(filename);
            string dataLine = sr.ReadToEnd();
            sr.Close();
            if (dataLine != null)
                project = JsonSerializer.Deserialize<ProjectData>(dataLine);
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

        public CommandData? GetCommand(string uuid)
        {
            return project.GetCommand(uuid);
        }

        public CommandData? GetCommandByKeyword(string name)
        {
            return project.GetCommandByKeyword(name);
        }

        public bool UpdateCommand(CommandData command)
        {
            return project.UpdateCommand(command);
        }

        public bool RemoveCommand(string uuid)
        {
            return project.RemoveCommand(uuid);
        }
    }
}
