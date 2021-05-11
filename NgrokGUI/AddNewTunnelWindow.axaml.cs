using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace NgrokGUI
{
    public class AddNewTunnelWindow : Window
    {

        private ComboBox cobProtocol;
        private TextBox tbName;
        private NumericUpDown nudLocalPort;
        
        public AddNewTunnelWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            List<string> protocols = new List<string> {"https","http","tcp"};

            cobProtocol.Items = protocols;
            cobProtocol.SelectedIndex = 0;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            cobProtocol = this.FindControl<ComboBox>("cobProtocol");
            tbName = this.FindControl<TextBox>("tbName");
            nudLocalPort = this.FindControl<NumericUpDown>("nudLocalPort");
        }

        private void btnAddNewTunnel_OnClick(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbName.Text))
            {
                //TODO imp a MessageBox
                //MessageBox.Show("The tunnel must have a name");
                return;
            }

            if (string.IsNullOrWhiteSpace( nudLocalPort.Text ))
            {
                //TODO imp a MessageBox
                //MessageBox.Show("There must be a local Port");
                return;
            }
            
            if (nudLocalPort.Value < 1 || nudLocalPort.Value > 65353)
            {
                //TODO imp a MessageBox
                //MessageBox.Show("Local Port must be between 1 and 65353");
                return;
            }

            //TODO handle DialogResult
            //DialogResult = true;
            Close();
        }
    }
}