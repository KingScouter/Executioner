using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

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

        [GeneratedRegex("\\d+")]
        private static partial Regex NumberOnlyRegex();

        private TextInputBoxMode mode = TextInputBoxMode.All;
        private Regex formatRegex = AllRegex();

        public AdvancedTextInputBox()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
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
