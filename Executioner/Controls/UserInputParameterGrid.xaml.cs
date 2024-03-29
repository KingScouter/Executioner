﻿using Executioner.Models;
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
                parameters.Add(editWindow.OutputData);
                RefreshGrid();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParametersGrid.SelectedItem is BaseUserInputParameter selectedItem)
            {
                UserInputParameterEditWindow editWindow = new UserInputParameterEditWindow(selectedItem);
                if (editWindow.ShowDialog() == true)
                {
                    BaseUserInputParameter data = editWindow.OutputData;
                    int dataIdx = parameters.FindIndex(elem => elem.Keyword == selectedItem.Keyword);
                    if (dataIdx != -1)
                    {
                        parameters.RemoveAt(dataIdx);
                        parameters.Insert(dataIdx, data);
                        RefreshGrid();
                    }
                }
            }
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
