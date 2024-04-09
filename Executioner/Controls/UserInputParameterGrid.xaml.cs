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

        private List<IBaseUserInputParameter> ParametersGridSource
        {
            get
            {
                return [.. parameters.Values];
            }
        }

        public Dictionary<string, IBaseUserInputParameter> Parameters
        {
            get { return parameters; }
            set
            {
                if (value == null)
                    parameters = [];
                else
                    parameters = value;
                ParametersGrid.ItemsSource = ParametersGridSource;
            }
        }

        public UserInputParameterGrid()
        {
            InitializeComponent();

            EnableEditButtons(false);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            IBaseUserInputParameter? data = null;
            while (true)
            {
                UserInputParameterEditWindow editWindow;
                if (data == null)
                    editWindow = new();
                else
                    editWindow = new(data);

                if (editWindow.ShowDialog() == true)
                {
                    data = editWindow.OutputData;

                    if (parameters.ContainsKey(data.Keyword))
                    {
                        MessageBox.Show($"Parameter with keyword {data.Keyword} already exists!");
                        continue;
                    }
                    parameters.Add(data.Keyword, data);
                    RefreshGrid();
                }
                break;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs? e)
        {
            if (ParametersGrid.SelectedItem is IBaseUserInputParameter selectedItem)
            {
                IBaseUserInputParameter data = selectedItem;
                while (true)
                {
                    UserInputParameterEditWindow editWindow = new UserInputParameterEditWindow(data);
                    if (editWindow.ShowDialog() == true)
                    {
                        data = editWindow.OutputData;
                        if (!data.Keyword.Equals(selectedItem.Keyword, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (parameters.ContainsKey(data.Keyword))
                            {
                                MessageBox.Show($"Parameter with keyword {data.Keyword} already exists!");
                                continue;
                            }
                            parameters.Remove(selectedItem.Keyword);
                            parameters.Add(data.Keyword, data);
                        } else
                        {
                            parameters[data.Keyword] = data;
                        }
                        RefreshGrid();
                    }
                    break;
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
            ParametersGrid.ItemsSource = null;
            ParametersGrid.ItemsSource = ParametersGridSource;
        }

        private void ParametersGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditButton_Click(sender, null);
        }

        private void ParametersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool elemSelected = ParametersGrid.SelectedItems.Count > 0;

            EnableEditButtons(elemSelected);
        }

        private void EnableEditButtons(bool enabled)
        {
            EditButton.IsEnabled = enabled;
            DeleteButton.IsEnabled = enabled;
            //CopyButton.IsEnabled = enabled;
        }
    }
}
