using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using NgrokSharp;

namespace ngrokGUI
{
    /// <summary>
    ///     Interaction logic for FirstTimeWizard.xaml
    /// </summary>
    public partial class FirstTimeWizard : Window
    {
        private List<ComboBoxTunnelDataTemplate> comboBoxTunnelDataTemplates = new List<ComboBoxTunnelDataTemplate>(6);
        private readonly INgrokManager _ngrokManager;

        public FirstTimeWizard(INgrokManager ngrokManager)
        {
            InitializeComponent();

            foreach (var tabItem in tabcl.Items)
            {
                ((TabItem)tabItem).IsEnabled = false;
            }

            comboBoxTunnelDataTemplates.Add(new ComboBoxTunnelDataTemplate() { Country = "Usa", Flag = "/Assets/Locations/icons8-usa-48.png", Region = NgrokManager.Region.UnitedStates});
            comboBoxTunnelDataTemplates.Add(new ComboBoxTunnelDataTemplate() { Country = "Germany", Flag = "/Assets/Locations/icons8-germany-48.png", Region = NgrokManager.Region.Europe});
            comboBoxTunnelDataTemplates.Add(new ComboBoxTunnelDataTemplate() { Country = "Singapore", Flag = "/Assets/Locations/icons8-singapore-48.png", Region = NgrokManager.Region.AsiaPacific});
            comboBoxTunnelDataTemplates.Add(new ComboBoxTunnelDataTemplate() { Country = "Australia", Flag = "/Assets/Locations/icons8-australia-48.png", Region = NgrokManager.Region.Australia});
            comboBoxTunnelDataTemplates.Add(new ComboBoxTunnelDataTemplate() { Country = "Brazil", Flag = "/Assets/Locations/icons8-brazil-48.png", Region = NgrokManager.Region.SouthAmerica});
            comboBoxTunnelDataTemplates.Add(new ComboBoxTunnelDataTemplate() { Country = "Japan", Flag = "/Assets/Locations/icons8-japan-48.png", Region = NgrokManager.Region.Japan});
            comboBoxTunnelDataTemplates.Add(new ComboBoxTunnelDataTemplate() { Country = "India", Flag = "/Assets/Locations/icons8-india-48.png", Region = NgrokManager.Region.India});

            cmbTunnelExit.ItemsSource = comboBoxTunnelDataTemplates;
            cmbTunnelExit.SelectedIndex = 0;

            _ngrokManager = ngrokManager;
        }

        private void _ngrokManager_DownloadAndUnZipDone(object sender, EventArgs e)
        {
            btnDownload.Content = "Next";
            txtbkDownloadInstruction.Text = "Download completed successfully! Please click next.";
            btnDownload.IsEnabled = true;
            pbprogress.IsIndeterminate = false;
            pbprogress.Value = 100;
        }

        private async void BtnDownload_OnClick(object sender, RoutedEventArgs e)
        {
            if ((string)btnDownload.Content == "Next")
            {
                tabcl.SelectedIndex = 1;
                return;
            }

            txtbkDownloadInstruction.Text = "Please wait while ngrok is downloading";
            pbprogress.IsIndeterminate = true;
            btnDownload.IsEnabled = false;
            await _ngrokManager.DownloadAndUnzipNgrokAsync();
        }

        private void BtnSelectDataCenter_OnClick(object sender, RoutedEventArgs e)
        {
            tabcl.SelectedIndex = 2;
        }

        private async void BtnAuth_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbxAuthToken.Text))
            {
                var dialogResult = MessageBox.Show("There is no authtoken, without one the sessions are limited to 2 hours. Are you okay with that?", "Do you want the 2 hour limit ?", MessageBoxButton.YesNo);

                if (dialogResult == MessageBoxResult.No)
                {
                    return;
                }
            }

            if ((bool)cbxPaidAccount.IsChecked && !string.IsNullOrWhiteSpace(txtbxAuthToken.Text))
            {
                var dialogResult = MessageBox.Show("Are you sure have a paid Ngrok account?", "Are you sure have a paid account?", MessageBoxButton.YesNo);

                if (dialogResult == MessageBoxResult.No)
                {
                    return;
                }
            }

            await _ngrokManager.RegisterAuthTokenAsync(txtbxAuthToken.Text);

            DialogResult = true;

            Close();
        }

        private void TxtbxAuthToken_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbxAuthToken.Text))
            {
                cbxPaidAccount.IsEnabled = false;
            }
            else
            {
                cbxPaidAccount.IsEnabled = true;
            }
        }
    }
}