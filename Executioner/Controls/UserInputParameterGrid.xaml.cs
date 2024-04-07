using Executioner.UserInputParameters;
using System.Windows;
using System.Windows.Controls;

namespace Executioner.Controls
{
    /// <summary>
    /// Interaction logic for UserInputParameterGrid.xaml
    /// </summary>
    public partial class UserInputParameterGrid : UserControl
    {
        private Dictionary<string, IBaseUserInputParameter> parameters = [];

        public Dictionary<string, IBaseUserInputParameter> Parameters
        {
            get { return parameters; }
            set
            {
                if (value == null)
                    parameters = [];
                else
                    parameters = value;
                ParametersGrid.ItemsSource = parameters;
            }
        }

        public UserInputParameterGrid()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            UserInputParameterEditWindow editWindow = new UserInputParameterEditWindow();
            if (editWindow.ShowDialog() == true) 
            {
                parameters.Add(editWindow.OutputData.Keyword, editWindow.OutputData);
                RefreshGrid();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParametersGrid.SelectedItem is IBaseUserInputParameter selectedItem)
            {
                UserInputParameterEditWindow editWindow = new UserInputParameterEditWindow(selectedItem);
                if (editWindow.ShowDialog() == true)
                {
                    IBaseUserInputParameter data = editWindow.OutputData;
                    bool dataIdx = parameters.ContainsKey(selectedItem.Keyword);
                    if (dataIdx)
                    {
                        parameters[selectedItem.Keyword] = data;
                        RefreshGrid();
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParametersGrid.SelectedItem is IBaseUserInputParameter selectedItem)
            {
                parameters.Remove(selectedItem.Keyword);
                RefreshGrid();
            }
        }

        private void RefreshGrid()
        {
            ParametersGrid.Items.Refresh();
        }
    }
}
