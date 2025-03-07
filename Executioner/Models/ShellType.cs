﻿using System.Windows.Data;

namespace Executioner.Models
{
    public enum ShellType
    {
        Cmd,
        WindowsPowershell,
        Bash,
        Powershell
    }

    public class ShellTypeConverter : IValueConverter
    {
        public static Dictionary<ShellType, string> NameMapping { get; } = new Dictionary<ShellType, string>()
        {
            { ShellType.Cmd, "Windows Commandline" },
            { ShellType.WindowsPowershell, "Windows Powershell" },
            { ShellType.Bash, "Bash" },
            { ShellType.Powershell, "Powershell" }
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (NameMapping.ContainsKey((ShellType)value))
            {
                return NameMapping[(ShellType)value];
            }
            else
            {
                return "No type found";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            KeyValuePair<ShellType, string> convertedElement = NameMapping.First(elem => elem.Value == value.ToString());
            if (convertedElement.Value == null)
                return ShellType.Cmd;

            return convertedElement.Key;
        }
    }
}
