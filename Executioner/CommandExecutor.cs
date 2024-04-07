using Executioner.InputWindows;
using Executioner.Models;
using Executioner.UserInputParameters;
using System.Text;
using System.Windows;

namespace Executioner
{
    internal class CommandExecutor
    {
        public static void ExecuteCommand(CommandData commandData, List<string> additionalArguments)
        {
            string parsedCommand = ParseCommand(commandData, additionalArguments);
            ExecuteCommandInternal(parsedCommand, commandData);
        }

        static string ParseCommand(CommandData commandData, List<string> additionalArguments)
        {
            CommandTemplate commandTemplate = commandData.CommandTemplate;

            StringBuilder sb = new();
            foreach(var elem in commandTemplate.elements)
            {
                if (!elem.isParam)
                    sb.Append(elem.ToString());
                else
                    sb.Append(ParseTemplateElement(elem.keyword, commandData.Parameters, additionalArguments));

                sb.Append(' ');
            }

            return sb.ToString().Trim();
        }

        static string ParseTemplateElement(string templateElement, List<IBaseUserInputParameter> parameters, List<string> additionalArguments)
        {
            string preSetValue = additionalArguments.FirstOrDefault("");
            if (additionalArguments.Count > 0)
                additionalArguments.RemoveAt(0);

            IBaseUserInputParameter? param = parameters.FirstOrDefault(elem => elem.Keyword == templateElement, null);
            if (param == null)
                throw new ArgumentException($"Param {templateElement} is not defined");

            if (preSetValue != "")
                return preSetValue;

            return ExecuteParameter(param);
        }

        private static string ExecuteParameter(IBaseUserInputParameter param)
        {
            InputParameterWindow paramWindow = new(param);
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
            System.Diagnostics.ProcessStartInfo startInfo = new()
            {
                FileName = "cmd.exe"
            };

            if (waitForResult)
                commandTemplate += "& pause";

            if (workingDir != "")
                startInfo.WorkingDirectory = workingDir;

            startInfo.Arguments = $"/C {commandTemplate}";

            ExecuteProcess(startInfo);
        }

        private static void ExecutePowershellCommand(string commandTemplate, bool waitForResult, string workingDir)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new()
            {
                FileName = "powershell.exe"
            };

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
