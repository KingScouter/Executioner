using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Executioner
{
    class CommandData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Template { get; set; }
        public Boolean WaitForResult { get; set; }
        public string WorkingDir { get; set; }
        public ShellType Type { get; set; }

        public CommandData(int id, string name, string description, string template,
            bool waitForResult, string workingDir, ShellType type)
        {
            Id = id;
            Name = name;
            Description = description;
            Template = template; ;
            WaitForResult = waitForResult;
            WorkingDir = workingDir;
            Type = type;
        }
    }
}
