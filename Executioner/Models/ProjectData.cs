using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Executioner.Models
{
    public  class ProjectData
    {
        private List<CommandData> commands = [];
        private string filename = "";
        private string projectName = "";
        private static int commandIdx = 0;

        public List<CommandData> Commands { get { return commands; } }

        [JsonIgnore]
        public string Filename { get { return filename; } }
        public string ProjectName {  get { return projectName; } }

        public ProjectData(string? filename)
        {
            if (filename != null)
                LoadProject(filename);
            else
                projectName = "New Project";
        }


        public void LoadProject(string filename)
        {
            commands.Clear();

            StreamReader sr = new StreamReader(filename);
            string dataLine = sr.ReadToEnd();
            if (dataLine != null)
            {
                List<CommandData>? parsedCommands = JsonSerializer.Deserialize<List<CommandData>>(dataLine);
                if (parsedCommands != null)
                {
                    commands.Clear();
                    commands.AddRange(parsedCommands);
                    //FillDataGrid();
                    //StatusBarTextBox.Text = $"Loaded project from {filename}";
                }
            }

            sr.Close();
        }

        public void SaveProject(string? filename)
        {
            if (filename != null)
                this.filename = filename;

            StreamWriter sw = new StreamWriter(this.filename);
            sw.Write(JsonSerializer.Serialize(this));
            sw.Close();
            //StatusBarTextBox.Text = $"Saved project to {saveFileDialog.FileName}";
        }

        public void AddCommand(CommandData command)
        {
            command.Id = commandIdx++;
            commands.Add(command);
        }

        public CommandData? GetCommand(int id)
        {
            return commands.Find(elem => elem.Id == id);
        }

        public bool UpdateCommand(CommandData command)
        {
            int commandIdx = commands.FindIndex(elem => elem.Id == command.Id);
            if (commandIdx != -1)
            {
                commands.RemoveAt(commandIdx);
                commands.Insert(commandIdx, command);
                return true;
            }

            return false;
        }

        public bool RemoveCommand(int id)
        {
            int commandIdx = commands.FindIndex(elem => elem.Id == id);
            if (commandIdx != -1)
            {
                commands.RemoveAt(commandIdx);
                return true;
            }

            return false;
        }
    }
}
