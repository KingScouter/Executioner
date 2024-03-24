using Executioner.Models;
using Executioner.UserInputParameters;
using System.Windows;

namespace Executioner
{
    /// <summary>
    /// Interaction logic for UserInputParameterEditWindow.xaml
    /// </summary>
    public partial class UserInputParameterEditWindow : Window
    {
        private ParameterType selectedType = ParameterType.Text;
        public ParameterType SelectedTypeProperty
        {
            get { return selectedType; }
            set {; }
        }

        public static Dictionary<ParameterType, string> NameMapping
        {
            get { return ParameterTypeConverter.NameMapping; }
        }

        public UserInputParameterEditWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public UserInputParameterEditWindow(IBaseUserInputParameter data)
        {
            InitializeComponent();
            DataContext = this;

            KeywordInputTextBox.Text = data.Keyword;
            NameInputTextBox.Text = data.Name;
            selectedType = data.Type;
        }

        public void OnOKButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public IBaseUserInputParameter OutputData
        {
            get
            {
                ParameterType type = (ParameterType)TypeComboBox.SelectedIndex;
                string keyword = KeywordInputTextBox.Text;
                string name = NameInputTextBox.Text;

                switch (type)
                {
                    case ParameterType.Text:
                        return new TextUserInputParameter(keyword, name);
                    case ParameterType.Number:
                        return new NumberUserInputParameter(keyword, name);
                    case ParameterType.Checkbox:
                        return new CheckBoxUserInputParameter(keyword, name);
                    default:
                        return new TextUserInputParameter(keyword, name);
                };
            }
        }
    }
}
