using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Executioner.Controls
{
    public enum TextInputBoxMode
    {
        All,
        CharOnly,
        NumberOnly
    }

    /// <summary>
    /// Interaction logic for AdvancedTextInputBox.xaml
    /// </summary>
    public partial class AdvancedTextInputBox : TextBox
    {
        public static readonly DependencyProperty ModeProperty =
        DependencyProperty.RegisterAttached("Mode", typeof(TextInputBoxMode), typeof(AdvancedTextInputBox), new PropertyMetadata(default(TextInputBoxMode)));

        public static void SetMode(UIElement element, TextInputBoxMode value)
        {
            element.SetValue(ModeProperty, value);
        }

        public static TextInputBoxMode GetMode(UIElement element)
        {
            return (TextInputBoxMode)element.GetValue(ModeProperty);
        }

        public bool AllowWhitespace { get; set; } = true;

        public TextInputBoxMode Mode
        {
            get { return mode; }
            set
            {
                switch (value)
                {
                    case TextInputBoxMode.CharOnly:
                        formatRegex = CharOnlyRegex();
                        break;
                    case TextInputBoxMode.NumberOnly:
                        formatRegex = NumberOnlyRegex();
                        AllowWhitespace = false;
                        break;
                    case TextInputBoxMode.All:
                    default:
                        formatRegex = AllRegex();
                        break;
                }

                mode = value;
            }
        }


        [GeneratedRegex(".+")]
        private static partial Regex AllRegex();

        [GeneratedRegex("\\w+")]
        private static partial Regex CharOnlyRegex();

        [GeneratedRegex("^(?:-(?:[1-9](?:\\d{0,2}(?:,\\d{3})+|\\d*))|(?:0|(?:[1-9](?:\\d{0,2}(?:,\\d{3})+|\\d*))))(?:.\\d+|)$")]
        private static partial Regex NumberOnlyRegex();

        private TextInputBoxMode mode = TextInputBoxMode.All;
        private Regex formatRegex = AllRegex();

        private Brush defaultForeground;

        public AdvancedTextInputBox()
        {
            InitializeComponent();

            Loaded += AdvancedTextInputBox_Loaded;
        }

        private void AdvancedTextInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = IsTextAllowed(((AdvancedTextInputBox)sender).Text);
            if (isValid)
                Foreground = defaultForeground;
            else
                Foreground = Brushes.Red;
        }

        private void AdvancedTextInputBox_Loaded(object sender, RoutedEventArgs e)
        {
            Mode = AdvancedTextInputBox.GetMode(this);
            defaultForeground = Foreground;
            Loaded -= AdvancedTextInputBox_Loaded;
        }

        
        private bool IsTextAllowed(string text)
        {
            return formatRegex.IsMatch(text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!AllowWhitespace && e.Key == Key.Space)
                e.Handled = true;
        }
    }
}
