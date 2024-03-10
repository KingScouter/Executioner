using Executioner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Executioner
{
    internal class CommandExecutor
    {
        public static void ExecuteCommand(CommandData commandData)
        {
            ParseCommand(commandData);
            ExecuteCommandInternal(commandData);
        }

        static string ParseCommand(CommandData commandData)
        {
            List<string> templateElements = new List<string>();

            var matches = System.Text.RegularExpressions.Regex.Matches(commandData.Template, "(?:([^(?:\\$\\{\\{)]*)(\\$\\{\\{[\\s|\\d|\\w]*\\}\\})?)");
            foreach(System.Text.RegularExpressions.Match match in matches)
            {
                bool first = true;
                foreach(System.Text.RegularExpressions.Group group in match.Groups)
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }
                    templateElements.Add(group.Value);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach(string elem in templateElements)
            {
                string parsedElement = ParseTemplateElement(elem, commandData.Parameters);
                sb.AppendLine($"<{parsedElement}>");
            }

            MessageBox.Show(sb.ToString() );

            return "";
        }

        static string ParseTemplateElement(string templateElement, List<BaseUserInputParameter> parameters)
        {

            if (!templateElement.StartsWith("${{") || !templateElement.EndsWith("}}"))
            {
                return templateElement;
            }

            string paramKeyword = templateElement.Substring(3, templateElement.Length - 5).Trim();
            BaseUserInputParameter? param = parameters.FirstOrDefault(elem => elem.Keyword == paramKeyword, null);
            if (param == null)
            {
                MessageBox.Show($"Param {paramKeyword} is not defined");
                return templateElement;
            }

            MessageBox.Show($"Param {paramKeyword} exists as {param.Name} with type {param.Type}");
            return param.Name;
        }

        private static void ExecuteCommandInternal(CommandData data)
        {
            try
            {
                switch (data.Type)
                {
                    case ShellType.Cmd:
                        ExecuteCmdCommand(data.Template, data.WaitForResult, data.WorkingDir);
                        break;
                    case ShellType.Powershell:
                        ExecutePowershellCommand(data.Template, data.WaitForResult, data.WorkingDir);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private static void ExecuteCmdCommand(string commandTemplate, Boolean waitForResult, string workingDir)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "cmd.exe";

            if (waitForResult)
                commandTemplate += "& pause";

            if (workingDir != "")
                startInfo.WorkingDirectory = workingDir;

            startInfo.Arguments = $"/C {commandTemplate}";

            ExecuteProcess(startInfo);
        }

        private static void ExecutePowershellCommand(string commandTemplate, Boolean waitForResult, string workingDir)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "powershell.exe";

            if (waitForResult)
                commandTemplate += "; pause";

            if (workingDir != "")
                startInfo.WorkingDirectory = workingDir;

            startInfo.Arguments = $"-ExecutionPolicy Bypass \"{commandTemplate}\"";
            startInfo.UseShellExecute = false;

            ExecuteProcess(startInfo);
        }

        private static void ExecuteProcess(System.Diagnostics.ProcessStartInfo startInfo)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}
