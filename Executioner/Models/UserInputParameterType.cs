using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Executioner.Models
{
    public enum UserInputParameterType
    {
        Text,
        Number,
        Checkbox,
        FileChooser,
        DirectoryChooser
    }

    public class UserInputParameterTypeConverter : IValueConverter
    {
        public static Dictionary<UserInputParameterType, string> NameMapping { get; } = new Dictionary<UserInputParameterType, string>()
        {
            { UserInputParameterType.Text, "Text" },
            { UserInputParameterType.Number, "Number" },
            { UserInputParameterType.Checkbox, "Checkbox" },
            { UserInputParameterType.FileChooser, "File" },
            { UserInputParameterType.DirectoryChooser, "Directory" }
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (NameMapping.ContainsKey((UserInputParameterType)value))
            {
                return NameMapping[(UserInputParameterType)value];
            }
            else
            {
                return "No type found";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            KeyValuePair<UserInputParameterType, string> convertedElement = NameMapping.First(elem => elem.Value == value.ToString());
            if (convertedElement.Value == null)
                return UserInputParameterType.Text;

            return convertedElement.Key;
        }
    }
}
