using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using NgrokSharp;

namespace ngrokGUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly INgrokManager _ngrokManager;
        private readonly ObservableCollection<TunnelDescription> _tunnelDescriptions;

        public MainWindow()
        {
            InitializeComponent();

            _tunnelDescriptions = new ObservableCollection<TunnelDescription>();
            DataContext = _tunnelDescriptions;

            _ngrokManager = new NgrokManager();

            Settings settings;
            try
            {
                //Load settings
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("Settings.json"));

                if (settings.firstTimeSetupDone == false)
                {
                    var firstTimeWizard = new FirstTimeWizard(_ngrokManager);
                    firstTimeWizard.ShowDialog();

                    if (firstTimeWizard.DialogResult == true)
                    {
                        settings.firstTimeSetupDone = true;
                        File.WriteAllText("Settings.json", JsonConvert.SerializeObject(settings));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Something went wrong while loading the settings: {e}");
                Close();
            }

            _ngrokManager.StartNgrok();
        }

        private void btnMenuItemExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void btnMenuItemAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            var addNewTunnelWindow = new AddNewTunnelWindow();

            addNewTunnelWindow.ShowDialog();

            if (addNewTunnelWindow.DialogResult != null && addNewTunnelWindow.DialogResult.Value)
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
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _ngrokManager.StopNgrok();
        }

        private void btnMenuItemCopy_OnClick(object sender, RoutedEventArgs e)
        {
            if (lwTunnels.SelectedIndex == -1) return;

            Clipboard.SetText(_tunnelDescriptions[lwTunnels.SelectedIndex].Url.ToString());
        }

        private async void btnMenuItemStopTunnel_OnClick(object sender, RoutedEventArgs e)
        {
            if (lwTunnels.SelectedIndex == -1) return;

            var result = await _ngrokManager.StopTunnel(_tunnelDescriptions[lwTunnels.SelectedIndex].Name);

            if (result == 204)
            {
                var tunnel =
                    _tunnelDescriptions.SingleOrDefault(
                        x => x.Name == _tunnelDescriptions[lwTunnels.SelectedIndex].Name);
                _tunnelDescriptions.Remove(tunnel);
            }
        }
    }
}