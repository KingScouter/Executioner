﻿using System.Text.RegularExpressions;
using System.Windows;
using Executioner.Models;
using Microsoft.Win32;

namespace Executioner
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CommandEditWindow : Window
    {
        string uuid = "";

        private ShellType selectedType = ShellType.Cmd;
        public ShellType SelectedTypeProperty
        {
            get { return selectedType; }
            set { ; }
        }

        public static Dictionary<ShellType, string> NameMapping 
        {
            get { return ShellTypeConverter.NameMapping; }
        }

        public CommandEditWindow()
        {
            InitializeComponent();
            DataContext = this;

            ParameterGrid.Parameters = [];
        }

        public CommandEditWindow(CommandData inputData)
        {
            InitializeComponent();
            DataContext = this;

            uuid = inputData.UUID;
            KeywordInputTextBox.Text = inputData.Keyword;
            NameInputTextBox.Text = inputData.Name;
            DescInputTextBox.Text = inputData.Description;
            TemplateInputTextBox.Text = inputData.Template;
            WaitForResultCheckBox.IsChecked = inputData.WaitForResult;
            RunAsAdminCheckBox.IsChecked = inputData.RunAsAdmin;
            WorkingDirTextBox.Text = inputData.WorkingDir;
            selectedType = inputData.Type;

            ParameterGrid.Parameters = inputData.Parameters;
        }

        public void OnOKButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OutputData.Validate();
                DialogResult = true;
            } catch (Exception ex)
            {
                MessageBox.Show($"Command invalid - Saving not possible: {ex.Message}");
            }
        }

        public void OnFileChooserButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            if (WorkingDirTextBox.Text.Length > 0)
                openFolderDialog.InitialDirectory = WorkingDirTextBox.Text;

            if (openFolderDialog.ShowDialog() == true)
                WorkingDirTextBox.Text = openFolderDialog.FolderName;
        }

        public CommandData OutputData 
        { 
            get 
            {
                ShellType type = (ShellType)TypeComboBox.SelectedIndex;

                return new CommandData(uuid, KeywordInputTextBox.Text, NameInputTextBox.Text, DescInputTextBox.Text,
                    TemplateInputTextBox.Text, WaitForResultCheckBox.IsChecked == true, WorkingDirTextBox.Text, RunAsAdminCheckBox.IsChecked == true,
                    type, ParameterGrid.Parameters);
            } 
        }

        private void KeywordInputTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static readonly Regex _regex = new Regex("[^a-zA-Z]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void ParameterGrid_InsertParamKeyword(object sender, Controls.InsertParamKeywordArgs e)
        {
            TemplateInputTextBox.AppendText($" {TemplateElement.ToParameterString(e.InsertKeyword)}");
        }
    }
}
