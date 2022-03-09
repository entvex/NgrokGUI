using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace ngrokGUI
{
    /// <summary>
    ///     Interaction logic for AddNewTunnelWindow.xaml
    /// </summary>
    public partial class AddNewTunnelWindow : Window
    {
        private readonly List<TunnelDescription> _currentTunnelDescriptions;
        private static readonly Regex _regex = new("[^0-9.-]+");



        public AddNewTunnelWindow(List<TunnelDescription> currentTunnelDescriptions ,bool paidAccount = false)
        {
            _currentTunnelDescriptions = currentTunnelDescriptions;
            InitializeComponent();

            if (!paidAccount)
            {
                tbSubdomain.Visibility = Visibility.Hidden;
                tbCustomDomain.Visibility = Visibility.Hidden;

                llsubdomain.Visibility = Visibility.Hidden;
                llCustomDomain.Visibility = Visibility.Hidden;

                wdSubdomain.MaxHeight = 0;
                wdCustomdomain.MaxHeight = 0;
                
                Height = 170;
            }

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

            if (_currentTunnelDescriptions.Any(x => x.Name == tbName.Text) )
            {
                MessageBox.Show($"A tunnel with name: {tbName.Text} already exists");
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