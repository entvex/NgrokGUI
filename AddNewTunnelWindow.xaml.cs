﻿using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ngrokGUI
{
    /// <summary>
    ///     Interaction logic for AddNewTunnelWindow.xaml
    /// </summary>
    public partial class AddNewTunnelWindow : Window
    {
        private static readonly Regex _regex = new("[^0-9.-]+");

        public AddNewTunnelWindow()
        {
            InitializeComponent();
            cobProtocol.Items.Add("https");
            cobProtocol.Items.Add("http");
            cobProtocol.Items.Add("tcp");
            cobProtocol.SelectedIndex = cobProtocol.Items.IndexOf("https");
        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void NumberValidationTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void BtnAddNewTunnel_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbName.Text))
            {
                MessageBox.Show("The tunnel must have a name");
                return;
            }

            if (string.IsNullOrWhiteSpace(tbLocalPort.Text))
            {
                MessageBox.Show("There must be a local Port");
                return;
            }

            if (int.TryParse(tbLocalPort.Text, out var i))
                if (i < 1 || i > 65353)
                {
                    MessageBox.Show("Local Port must be between 1 and 65353");
                    return;
                }

            DialogResult = true;
            Close();
        }

        private void TbLocalPort_OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string) e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text)) e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}