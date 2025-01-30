using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
    public partial class AdvancedTextInputBox : StackPanel
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

        // Exposed properties from the TextBox
        public string Text { get => InputTextBox.Text; set => InputTextBox.Text = value; }
        public TextWrapping TextWrapping { get => InputTextBox.TextWrapping; set => InputTextBox.TextWrapping = value; }
        public VerticalAlignment VerticalContentAlignment { get => InputTextBox.VerticalContentAlignment; set => InputTextBox.VerticalContentAlignment = value; }
        public double FontSize { get => InputTextBox.FontSize; 
            set 
            {
                InputTextBox.FontSize = value;
                PrefixLabel.FontSize = value;
            } 
        }
        public int MaxLines { get => InputTextBox.MaxLines; set => InputTextBox.MaxLines = value; }
        public Thickness BorderThickness { get => InputTextBox.BorderThickness; set => InputTextBox.BorderThickness = value; }

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

        private bool IsValid { get => isValid; set
            {
                if (isValid == value)
                    return;

                if (value)
                {
                    if (prefixFadeInStoryboard != null)
                    {
                        prefixFadeInStoryboard.Stop();
                        prefixFadeInStoryboard = null;
                    }
                    PrefixFadeOut();
                } else
                {
                    if (prefixFadeOutStoryboard != null)
                    {
                        prefixFadeOutStoryboard.Stop();
                        prefixFadeOutStoryboard = null;
                    }
                    PrefixFadeIn();
                }

                isValid = value;
            } 
        }

        private bool isValid = true;


        [GeneratedRegex(".+")]
        private static partial Regex AllRegex();

        [GeneratedRegex("\\w+")]
        private static partial Regex CharOnlyRegex();

        [GeneratedRegex("^(?:-(?:[1-9](?:\\d{0,2}(?:,\\d{3})+|\\d*))|(?:0|(?:[1-9](?:\\d{0,2}(?:,\\d{3})+|\\d*))))(?:.\\d+|)$")]
        private static partial Regex NumberOnlyRegex();

        private static readonly Brush errorColor = Brushes.Red;

        private TextInputBoxMode mode = TextInputBoxMode.All;
        private Regex formatRegex = AllRegex();

        private readonly Brush defaultForeground;

        private Storyboard? prefixFadeInStoryboard;
        private Storyboard? prefixFadeOutStoryboard;

        public AdvancedTextInputBox()
        {
            InitializeComponent();

            defaultForeground = InputTextBox.Foreground;
            PrefixLabel.Foreground = errorColor;
            PrefixLabel.Content = '!';
            InputTextBox.Loaded += AdvancedTextInputBox_Loaded;
        }

        private void AdvancedTextInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = IsTextAllowed(((TextBox)sender).Text);
            if (isValid)
                InputTextBox.Foreground = defaultForeground;
            else
                InputTextBox.Foreground = errorColor;
            IsValid = isValid;
        }

        private void AdvancedTextInputBox_Loaded(object sender, RoutedEventArgs e)
        {
            Mode = AdvancedTextInputBox.GetMode(this);
            InputTextBox.Loaded -= AdvancedTextInputBox_Loaded;
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

        private void StackPanel_GotFocus(object sender, RoutedEventArgs e)
        {
            InputTextBox.Focus();
        }

        private void StackPanel_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            InputTextBox.Focus();
        }

        /// <summary>
        /// Animated Fade-In for the prefix-label
        /// </summary>
        private void PrefixFadeIn()
        {
            if (PrefixLabel.Opacity != 0)
                return;

            DoubleAnimation anim = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(500)));
            prefixFadeInStoryboard = new Storyboard();
            prefixFadeInStoryboard.Children.Add(anim);

            Storyboard.SetTarget(anim, PrefixLabel);
            Storyboard.SetTargetProperty(anim, new PropertyPath(OpacityProperty));
            prefixFadeInStoryboard.Begin();
        }

        /// <summary>
        /// Animated Fade-Out for the prefix-label
        /// </summary>
        private void PrefixFadeOut()
        {
            if (PrefixLabel.Opacity != 1)
                return;

            DoubleAnimation anim = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(500)));
            prefixFadeOutStoryboard = new Storyboard();
            prefixFadeOutStoryboard.Children.Add(anim);

            Storyboard.SetTarget(anim, PrefixLabel);
            Storyboard.SetTargetProperty(anim, new PropertyPath(OpacityProperty));
            prefixFadeOutStoryboard.Begin();
        }
    }
}
