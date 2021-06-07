using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
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
        private readonly INgrokManager _ngrokManager;
        private readonly DataGrid dgTunnels;
        public ObservableCollection<TunnelDescription> _tunnelDescriptions { get; } = new();
        public MainWindow()
        {
            _ngrokManager = new NgrokManager();
            Initialized += OnInitialized;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            //Make sure to stop Ngrok, if the window is closing.
            Closing += (sender, args) => _ngrokManager.StopNgrok();
            
            dgTunnels = this.Find<DataGrid>("dgTunnels");

            dgTunnels.Items = _tunnelDescriptions;

            
            //FirstTimeSetUp().ContinueWith(task => {});
            TaskScheduler.FromCurrentSynchronizationContext();
        }

        private async void OnInitialized(object? sender, EventArgs e)
        {
            FirstTimeSetUp();
        }

        private async Task FirstTimeSetUp()
        {
            Settings settings;
            try
            {
                //Load settings
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("Settings.json"));

                if (settings.firstTimeSetupDone == false)
                {
                    var firstTimeWizard = new FirstTimeWizard(_ngrokManager);

                    var result = await firstTimeWizard.ShowDialog<string>(this);

                    if (result == "result")
                    {
                        settings.firstTimeSetupDone = true;
                        File.WriteAllText("Settings.json", JsonConvert.SerializeObject(settings));
                    }
                }
            }
            catch (Exception e)
            {
                //TODO MessageBox
                //MessageBox.Show($"Something went wrong while loading the settings: {e}");
                Close();
            }
            
            _ngrokManager.StartNgrok();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void btnMenuItemAddNew_OnClick(object? sender, RoutedEventArgs e)
        {
            var addNewTunnelWindow = new AddNewTunnelWindow();

            var result = await addNewTunnelWindow.ShowDialog<string>(this);

            
            if (result == "true")
            {
                var startTunnelDto = new StartTunnelDTO
                {
                    name = addNewTunnelWindow.FindControl<TextBox>("tbName").Text,
                    proto = addNewTunnelWindow.FindControl<ComboBox>("cobProtocol").SelectedItem?.ToString(),
                    addr = addNewTunnelWindow.FindControl<NumericUpDown>("nudLocalPort").Text,
                    //TODO add the URL
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
                        Name = addNewTunnelWindow.FindControl<TextBox>("tbName").Text,
                        Protocol = addNewTunnelWindow.FindControl<ComboBox>("cobProtocol").SelectedItem?.ToString(),
                        Port = Convert.ToInt32(addNewTunnelWindow.FindControl<NumericUpDown>("nudLocalPort").Value),
                        Url = tunnelDetail.PublicUrl
                    };
                    _tunnelDescriptions.Add(tunnel);
                }
                else
                {
                    var tunnelError =
                        JsonConvert.DeserializeObject<TunnelError>(
                            await httpResponseMessage.Content.ReadAsStringAsync());
                    //TODO MessageBox
                    //MessageBox.Show(tunnelError.Details.Err);
                }
            }
        }
        private void MenuItemExit_OnClick(object? sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuItemCopyLink_Click(object? sender, RoutedEventArgs e)
        {
            //TODO MessageBox, tell user to click on the item first
            if (dgTunnels.SelectedIndex == -1) return;

            Application.Current.Clipboard.SetTextAsync(_tunnelDescriptions[dgTunnels.SelectedIndex].Url.ToString());
        }

        private void MenuItem_OnClickFirstTimeWizard(object? sender, RoutedEventArgs e)
        {
            //TODO MessageBox yes, no
            //var result = MessageBox.Show("Are you sure you want to close NgrokGUI in order to run the First Time Wizard, again?", "Are you sure?", MessageBoxButton.YesNo);
            //if (result == MessageBoxResult.Yes)
            //{
                Settings settings = new Settings {firstTimeSetupDone = false};
                File.WriteAllText("Settings.json", JsonConvert.SerializeObject(settings));
                Environment.Exit(0);
            //}
        }
    }
}