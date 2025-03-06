using Executioner.InputWindows;
using Executioner.Models;
using Executioner.UserInputParameters;
using System.Text;
using System.Windows;

namespace Executioner
{
    /// <summary>
    /// Executor to execute a given command (parse template, user-input, execution)
    /// </summary>
    internal class CommandExecutor
    {
        /// <summary>
        /// Execute a command
        /// </summary>
        /// <param name="commandData">Command data to execute</param>
        /// <param name="additionalArguments">Additional arguments to use instead of user-input</param>
        /// <param name="dryRun">True to only print the command, otherwise false to execute it</param>
        public static void ExecuteCommand(CommandData commandData, List<string> additionalArguments, bool dryRun)
        {
            string parsedCommand = ParseCommand(commandData, additionalArguments);

            if (dryRun)
            {
                MessageBox.Show(parsedCommand, "Dry Run");
                return;
            }
            ExecuteCommandInternal(parsedCommand, commandData);
        }

        /// <summary>
        /// Parse a command. Execute the user-input for the set parameters.
        /// </summary>
        /// <param name="commandData">Data of the command to parse</param>
        /// <param name="additionalArguments">List of arguments to use before any user-input</param>
        /// <returns>Parsed command as string</returns>
        static string ParseCommand(CommandData commandData, List<string> additionalArguments)
        {
            CommandTemplate commandTemplate = commandData.CommandTemplate;
            Dictionary<string, string> parsedValues = [];

            StringBuilder sb = new();
            foreach(var elem in commandTemplate.elements)
            {
                if (!elem.isParam)
                    sb.Append(elem.ToString());
                else
                {
                    if (parsedValues.ContainsKey(elem.keyword))
                        sb.Append(parsedValues[elem.keyword]);
                    else
                    {
                        string parsedValue = ParseTemplateElement(elem.keyword, commandData.Parameters, additionalArguments);
                        sb.Append(parsedValue);
                        parsedValues.Add(elem.keyword, parsedValue);
                    }
                }

                sb.Append(' ');
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Parse a single template-element. Check the parameter-data from the stored command and open the input-window.
        /// </summary>
        /// <param name="keyword">Keyword of the parameter</param>
        /// <param name="parameters">List of defined parameters for the command</param>
        /// <param name="additionalArguments">List of arguments to use instead of a user-input</param>
        /// <returns>Parsed value as a string for the parsed command</returns>
        /// <exception cref="ArgumentException"></exception>
        static string ParseTemplateElement(string keyword, Dictionary<string, IBaseUserInputParameter> parameters, List<string> additionalArguments)
        {
            string preSetValue = additionalArguments.FirstOrDefault("");
            if (additionalArguments.Count > 0)
                additionalArguments.RemoveAt(0);

            IBaseUserInputParameter? param = parameters[keyword];
            if (param == null)
                throw new ArgumentException($"Param {keyword} is not defined");

            if (preSetValue != "")
                return preSetValue;

            return ExecuteParameter(param);
        }

        /// <summary>
        /// Open the input-window for user-input parameters
        /// </summary>
        /// <param name="param">Parameter to let the user enter</param>
        /// <returns>Parsed value of the parameter for the parsed command</returns>
        /// <exception cref="ArgumentException">User cancel</exception>
        private static string ExecuteParameter(IBaseUserInputParameter param)
        {
            BaseParameterInputWindow? paramWindow = null;
            switch (param.Type)
            {
                case ParameterType.Text:
                    {
                        paramWindow = new TextParameterInputWindow(param);
                        break;
                    }
                case ParameterType.Number:
                    {
                        paramWindow = new NumberParameterInputWindow(param);
                        break;
                    }
                case ParameterType.Checkbox:
                    {
                        paramWindow = new CheckboxParameterInputWindow(param);
                        break;
                    }
            }

            if (paramWindow != null)
            {
                if (paramWindow.ShowDialog() != true)
                    throw new ArgumentException("User cancel");

                return paramWindow.OutputValue;
            }

            InputParameterWindow miscParamWindow = new(param);
            if (miscParamWindow.ShowDialog() != true)
                throw new ArgumentException($"User cancel");

            return miscParamWindow.OutputData!;
        }

        /// <summary>
        /// Determine the type of command and execute it accordingly
        /// </summary>
        /// <param name="parsedCommand">Command string to execute</param>
        /// <param name="data">Command data</param>
        private static void ExecuteCommandInternal(string parsedCommand, CommandData data)
        {
            try
            {
                switch (data.Type)
                {
                    case ShellType.Cmd:
                        ExecuteCmdCommand(parsedCommand, data.WaitForResult, data.WorkingDir, data.RunAsAdmin);
                        break;
                    case ShellType.WindowsPowershell:
                        ExecuteWindowsPowershellCommand(parsedCommand, data.WaitForResult, data.WorkingDir, data.RunAsAdmin);
                        break;
                    case ShellType.Powershell:
                        ExecutePowershellCommand(parsedCommand, data.WaitForResult, data.WorkingDir, data.RunAsAdmin);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        /// <summary>
        /// Execute a command in the windows command-line
        /// </summary>
        /// <param name="commandTemplate">Command template</param>
        /// <param name="waitForResult">Flag to determine if the CLI should remain open after the execution finished</param>
        /// <param name="workingDir">Working directory</param>
        /// <param name="runAsAdmin">Flag to check if the process should be started with admin privileges</param>
        private static void ExecuteCmdCommand(string commandTemplate, bool waitForResult, string workingDir, bool runAsAdmin)
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

            ExecuteProcess(startInfo, runAsAdmin);
        }

        /// <summary>
        /// Execute a command in the windows PowerShell
        /// </summary>
        /// <param name="commandTemplate">Command template</param>
        /// <param name="waitForResult">Flag to determine if the CLI should remain open after the execution finished</param>
        /// <param name="workingDir">Working directory</param>
        /// <param name="runAsAdmin">Flag to check if the process should be started with admin privileges</param>
        private static void ExecuteWindowsPowershellCommand(string commandTemplate, bool waitForResult, string workingDir, bool runAsAdmin)
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

            ExecuteProcess(startInfo, runAsAdmin);
        }

        /// <summary>
        /// Execute a command in the PowerShell
        /// </summary>
        /// <param name="commandTemplate">Command template</param>
        /// <param name="waitForResult">Flag to determine if the CLI should remain open after the execution finished</param>
        /// <param name="workingDir">Working directory</param>
        /// <param name="runAsAdmin">Flag to check if the process should be started with admin privileges</param>
        private static void ExecutePowershellCommand(string commandTemplate, bool waitForResult, string workingDir, bool runAsAdmin)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new()
            {
                FileName = "pwsh.exe"
            };

            if (waitForResult)
                commandTemplate += "; pause";

            if (workingDir != "")
                startInfo.WorkingDirectory = workingDir;

            startInfo.Arguments = $"-ExecutionPolicy Bypass -command \"{commandTemplate}\"";
            startInfo.UseShellExecute = false;

            ExecuteProcess(startInfo, runAsAdmin);
        }

        /// <summary>
        /// Internal method to startup and execute a process for the command
        /// </summary>
        /// <param name="startInfo">Process info for startup</param>
        /// <param name="runAsAdmin">Flag to check if the process should be started with admin privileges</param>
        private static void ExecuteProcess(System.Diagnostics.ProcessStartInfo startInfo, bool runAsAdmin = false)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();

            if (runAsAdmin)
            {
                startInfo.Verb = "runas";
                startInfo.UseShellExecute = true;
            }

            process.StartInfo = startInfo;
            process.Start();
        }
    }
}
