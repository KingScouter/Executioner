using Executioner.Models;
using System.Windows;
using System.Windows.Controls;

namespace Executioner.Controls
{
    /// <summary>
    /// Interaction logic for UserInputParameterGrid.xaml
    /// </summary>
    public partial class UserInputParameterGrid : UserControl
    {
        private List<BaseUserInputParameter> parameters = [];

        public List<BaseUserInputParameter> Parameters
        {
            get { return parameters; }
            set
            {
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
            
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParametersGrid.SelectedItem is BaseUserInputParameter selectedItem)
            {
                parameters.Remove(selectedItem);
                RefreshGrid();
            }
        }

        private void RefreshGrid()
        {
            ParametersGrid.Items.Refresh();
        }
    }
}
