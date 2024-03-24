using Executioner.Models;
using System.ComponentModel.Design;
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

                switch (type)
                {
                    case ParameterType.Text:
                        return new TextUserInputParameter(KeywordInputTextBox.Text, NameInputTextBox.Text);
                    case ParameterType.Number:
                        return new NumberUserInputParameter(KeywordInputTextBox.Text, NameInputTextBox.Text);
                    default:
                        return new TextUserInputParameter(KeywordInputTextBox.Text, NameInputTextBox.Text);
                };
            }
        }
    }
}
