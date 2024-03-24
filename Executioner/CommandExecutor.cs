﻿using Executioner.InputWindows;
using Executioner.Models;
using Executioner.UserInputParameters;
using System.Text;
using System.Windows;

namespace Executioner
{
    internal class CommandExecutor
    {
        public static void ExecuteCommand(CommandData commandData)
        {
            string parsedCommand = ParseCommand(commandData);
            ExecuteCommandInternal(parsedCommand, commandData);
        }

        static string ParseCommand(CommandData commandData)
        {
            List<string> templateElements = [];

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
                sb.Append(parsedElement);
            }

            return sb.ToString();
        }

        static string ParseTemplateElement(string templateElement, List<IBaseUserInputParameter> parameters)
        {

            if (!templateElement.StartsWith("${{") || !templateElement.EndsWith("}}"))
            {
                return templateElement;
            }

            string paramKeyword = templateElement.Substring(3, templateElement.Length - 5).Trim();
            IBaseUserInputParameter? param = parameters.FirstOrDefault(elem => elem.Keyword == paramKeyword, null);
            if (param == null)
                throw new ArgumentException($"Param {paramKeyword} is not defined");

            return ExecuteParameter(param);
        }

        private static string ExecuteParameter(IBaseUserInputParameter param)
        {
            InputParameterWindow paramWindow = new InputParameterWindow(param);
            if (paramWindow.ShowDialog() != true)
                throw new ArgumentException($"User cancel");

            return paramWindow.OutputData!;
        }

        private static void ExecuteCommandInternal(string parsedCommand, CommandData data)
        {
            try
            {
                switch (data.Type)
                {
                    case ShellType.Cmd:
                        ExecuteCmdCommand(parsedCommand, data.WaitForResult, data.WorkingDir);
                        break;
                    case ShellType.Powershell:
                        ExecutePowershellCommand(parsedCommand, data.WaitForResult, data.WorkingDir);
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

        private static void ExecutePowershellCommand(string commandTemplate, bool waitForResult, string workingDir)
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
