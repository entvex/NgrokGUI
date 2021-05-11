using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using NgrokSharp;

namespace NgrokGUI
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void btnMenuItemAddNew_OnClick(object? sender, RoutedEventArgs e)
        {
            var addNewTunnelWindow = new AddNewTunnelWindow();

            
            //var result = await addNewTunnelWindow.ShowDialog<string>(Application.Current.MainWindow);

            /*if (addNewTunnelWindow.DialogResult != null && addNewTunnelWindow.DialogResult.Value)
            {
                var startTunnelDto = new StartTunnelDTO
                {
                    name = addNewTunnelWindow.tbName.Text,
                    proto = addNewTunnelWindow.cobProtocol.SelectionBoxItem.ToString(),
                    addr = addNewTunnelWindow.tbLocalPort.Text,
                    bind_tls = "false"
                };

                // bind_tls http bind an HTTPS or HTTP endpoint or both true, false, or both
                if (startTunnelDto.proto == "https")
                {
                    startTunnelDto.proto = "http";
                    startTunnelDto.bind_tls = "true";
                }

                var httpResponseMessage = await _ngrokManager.StartTunnel(startTunnelDto);

                if ((int) httpResponseMessage.StatusCode == 201)
                {
                    var tunnelDetail =
                        JsonConvert.DeserializeObject<TunnelDetail>(
                            await httpResponseMessage.Content.ReadAsStringAsync());

                    var tunnel = new TunnelDescription
                    {
                        Name = addNewTunnelWindow.tbName.Text,
                        Protocol = addNewTunnelWindow.cobProtocol.SelectionBoxItem.ToString(),
                        Port = Convert.ToInt32(addNewTunnelWindow.tbLocalPort.Text),
                        Url = tunnelDetail.PublicUrl
                    };
                    _tunnelDescriptions.Add(tunnel);
                }
                else
                {
                    var tunnelError =
                        JsonConvert.DeserializeObject<TunnelError>(
                            await httpResponseMessage.Content.ReadAsStringAsync());
                    MessageBox.Show(tunnelError.Details.Err);
                }
            }*/
        }

        private void btnMenuItemExit_OnClick(object? sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void btnMenuItemRunFirstTimeWizard_OnClick(object? sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}