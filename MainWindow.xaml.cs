using System;
using System.Collections.Generic;
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
        private readonly string _downloadFolder =
                $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar}NgrokSharp{Path.DirectorySeparatorChar}";
        private bool PaidAccount;

        public MainWindow()
        {
            InitializeComponent();

            _tunnelDescriptions = new ObservableCollection<TunnelDescription>();
            DataContext = _tunnelDescriptions;

            _ngrokManager = new NgrokManager();


            if (!File.Exists($"{_downloadFolder}Settings.json"))
            {
                File.WriteAllText($"{_downloadFolder}Settings.json", "{\r\n  \"firstTimeSetupDone\": false\r\n}");
            }

            Settings settings;
            try
            {
                //Load settings
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText($"{_downloadFolder}Settings.json"));

                if (settings.FirstTimeSetupDone == false)
                {
                    var firstTimeWizard = new FirstTimeWizard(_ngrokManager);
                    firstTimeWizard.ShowDialog();

                    if (firstTimeWizard.DialogResult == true)
                    {
                        settings.FirstTimeSetupDone = true;
                        settings.DataCenterRegion = firstTimeWizard.cmbTunnelExit.SelectedIndex;
                        settings.PaidAccount = (bool) firstTimeWizard.cbxPaidAccount.IsChecked;

                        File.WriteAllText($"{_downloadFolder}Settings.json", JsonConvert.SerializeObject(settings));
                    }
                }

                PaidAccount = settings.PaidAccount;
                sbStatus.Content = $"connected to {(NgrokManager.Region)settings.DataCenterRegion}";
                _ngrokManager.StartNgrok((NgrokManager.Region)settings.DataCenterRegion);

                if (File.Exists($"{_downloadFolder}SavedTunnels.json"))
                {
                    JsonConvert.DeserializeObject<List<TunnelDescription>>(File.ReadAllText(
                        $"{_downloadFolder}SavedTunnels.json"))?.ForEach( x => _tunnelDescriptions.Add(x));
                }

            }
            catch (Exception e)
            {
                MessageBox.Show($"Something went wrong while loading the settings: {e}");
                Close();
            }

            
        }

        private void btnMenuItemExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void btnMenuItemAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            var addNewTunnelWindow = new AddNewTunnelWindow(_tunnelDescriptions.ToList(), PaidAccount);

            addNewTunnelWindow.ShowDialog();

            if (addNewTunnelWindow.DialogResult != null && addNewTunnelWindow.DialogResult.Value)
            {
                var startTunnelDto = new StartTunnelDTO
                {
                    name = addNewTunnelWindow.tbName.Text,
                    proto = addNewTunnelWindow.cobProtocol.SelectionBoxItem.ToString(),
                    addr = addNewTunnelWindow.tbLocalPort.Text,
                    bind_tls = "false",
                    subdomain = addNewTunnelWindow.tbSubdomain.Text
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
                        JsonConvert.DeserializeObject<TunnelDetailDTO>(
                            await httpResponseMessage.Content.ReadAsStringAsync());

                    var tunnel = new TunnelDescription
                    {
                        Name = addNewTunnelWindow.tbName.Text,
                        Protocol = addNewTunnelWindow.cobProtocol.SelectionBoxItem.ToString(),
                        Port = Convert.ToInt32(addNewTunnelWindow.tbLocalPort.Text),
                        Url = tunnelDetail.PublicUrl,
                        Active = true,
                        SubDomain = addNewTunnelWindow.tbSubdomain.Text
                    };
                    _tunnelDescriptions.Add(tunnel);
                }
                else
                {
                    var tunnelError =
                        JsonConvert.DeserializeObject<TunnelErrorDTO>(
                            await httpResponseMessage.Content.ReadAsStringAsync());
                    MessageBox.Show(tunnelError.Details.Err);
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _ngrokManager.StopNgrok();

            foreach (var tunnelDescription in _tunnelDescriptions)
            {
                tunnelDescription.Active = false;

                if (string.IsNullOrWhiteSpace(tunnelDescription.SubDomain))
                {
                    tunnelDescription.Url = null;
                }
            }

            File.WriteAllText($"{_downloadFolder}SavedTunnels.json", JsonConvert.SerializeObject(_tunnelDescriptions));

        }

        private void btnMenuItemCopy_OnClick(object sender, RoutedEventArgs e)
        {
            if (lwTunnels.SelectedIndex == -1) return;

            if (_tunnelDescriptions[lwTunnels.SelectedIndex].Url == null) return;
            

            Clipboard.SetText(_tunnelDescriptions[lwTunnels.SelectedIndex].Url.ToString());
        }

        private async void btnMenuItemStopTunnel_OnClick(object sender, RoutedEventArgs e)
        {
            if (lwTunnels.SelectedIndex == -1) return;

            var result = await _ngrokManager.StopTunnel(_tunnelDescriptions[lwTunnels.SelectedIndex].Name);

            if (result == 204)
            {
                _tunnelDescriptions[lwTunnels.SelectedIndex].Active = false;

                if (string.IsNullOrWhiteSpace(_tunnelDescriptions[lwTunnels.SelectedIndex].SubDomain))
                {
                    _tunnelDescriptions[lwTunnels.SelectedIndex].Url = null;
                }
                
            }
        }

        private void BtnMenuItemRunFirstTimeWizard_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to close NgrokGUI in order to run the First Time Wizard, again?", "Are you sure?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Settings settings = new Settings {FirstTimeSetupDone = false};
                File.WriteAllText($"{_downloadFolder}Settings.json", JsonConvert.SerializeObject(settings));
                Close();
            }
        }

        private async void btnMenuItemStartTunnel_OnClick(object sender, RoutedEventArgs e)
        {
            if (lwTunnels.SelectedIndex == -1) return;

            if (_tunnelDescriptions[lwTunnels.SelectedIndex].Active) return;


            var startTunnelDto = new StartTunnelDTO
            {
                name = _tunnelDescriptions[lwTunnels.SelectedIndex].Name,
                proto = _tunnelDescriptions[lwTunnels.SelectedIndex].Protocol,
                addr = _tunnelDescriptions[lwTunnels.SelectedIndex].Port.ToString(),
                bind_tls = "false",
                subdomain = _tunnelDescriptions[lwTunnels.SelectedIndex].SubDomain
            };

            // bind_tls http bind an HTTPS or HTTP endpoint or both true, false, or both
            if (startTunnelDto.proto == "https")
            {
                startTunnelDto.proto = "http";
                startTunnelDto.bind_tls = "true";
            }

            var httpResponseMessage = await _ngrokManager.StartTunnel(startTunnelDto);

            if ((int)httpResponseMessage.StatusCode == 201)
            {
                var tunnelDetail =
                    JsonConvert.DeserializeObject<TunnelDetailDTO>(
                        await httpResponseMessage.Content.ReadAsStringAsync());

                _tunnelDescriptions[lwTunnels.SelectedIndex].Active = true;
                _tunnelDescriptions[lwTunnels.SelectedIndex].Url = tunnelDetail.PublicUrl;
            }
            else
            {
                var tunnelError =
                    JsonConvert.DeserializeObject<TunnelErrorDTO>(
                        await httpResponseMessage.Content.ReadAsStringAsync());
                MessageBox.Show(tunnelError.Details.Err);
            }

        }

        private async void btnMenuItemDeleteTunnel_OnClick(object sender, RoutedEventArgs e)
        {
            if (lwTunnels.SelectedIndex == -1) return;

            if (_tunnelDescriptions[lwTunnels.SelectedIndex].Active)
            {
                var result = await _ngrokManager.StopTunnel(_tunnelDescriptions[lwTunnels.SelectedIndex].Name);
            }

            var tunnel = _tunnelDescriptions.SingleOrDefault(x => x.Name == _tunnelDescriptions[lwTunnels.SelectedIndex].Name);
            _tunnelDescriptions.Remove(tunnel);

        }
    }
}