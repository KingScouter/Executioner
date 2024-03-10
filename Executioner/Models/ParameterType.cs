using System.Windows.Data;

namespace Executioner.Models
{
    public enum ParameterType
    {
        Text,
        Number,
        Checkbox,
        FileChooser,
        DirectoryChooser
    }

    public class ParameterTypeConverter : IValueConverter
    {
        public static Dictionary<ParameterType, string> NameMapping { get; } = new Dictionary<ParameterType, string>()
        {
            { ParameterType.Text, "Text" },
            { ParameterType.Number, "Number" },
            { ParameterType.Checkbox, "Checkbox" },
            { ParameterType.FileChooser, "File" },
            { ParameterType.DirectoryChooser, "Directory" }
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (NameMapping.ContainsKey((ParameterType)value))
            {
                return NameMapping[(ParameterType)value];
            }
            else
            {
                return "No type found";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            KeyValuePair<ParameterType, string> convertedElement = NameMapping.First(elem => elem.Value == value.ToString());
            if (convertedElement.Value == null)
                return ParameterType.Text;

            return convertedElement.Key;
        }
    }
}
