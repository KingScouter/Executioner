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


        public ProjectData? LoadProject(string filename)
        {
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
