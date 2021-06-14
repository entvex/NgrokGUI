using System;
using System.Windows;
using NgrokSharp;

namespace ngrokGUI
{
    /// <summary>
    ///     Interaction logic for FirstTimeWizard.xaml
    /// </summary>
    public partial class FirstTimeWizard : Window
    {
        private readonly INgrokManager _ngrokManager;
        private State state;

        public FirstTimeWizard(INgrokManager ngrokManager)
        {
            InitializeComponent();
            _ngrokManager = ngrokManager;
            _ngrokManager.DownloadAndUnZipDone += _ngrokManager_DownloadAndUnZipDone;
            state = State.Start;
        }

        private void _ngrokManager_DownloadAndUnZipDone(object sender, EventArgs e)
        {
            btnNext.Content = "Next";
            txtbkInstruction.Text = "Download completed successfully! Please click next.";
            btnNext.IsEnabled = true;
            pbprogress.IsIndeterminate = false;
            pbprogress.Value = 100;
            state = State.RegsiterAuthtoken;
        }

        private void BtnNext_OnClick(object sender, RoutedEventArgs e)
        {

            if (state == State.Done)
            {
                if (string.IsNullOrWhiteSpace(txtbxAuthToken.Text))
                {
                    var dialogResult = MessageBox.Show("There is no authtoken, without one the sessions are limited to 2 hours. Are you okay with that?", "Do you want the 2 hour limit ?", MessageBoxButton.YesNo);
                    
                if (dialogResult == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                _ngrokManager.RegisterAuthToken(txtbxAuthToken.Text);

                DialogResult = true;

                Close();
            }



            if (state == State.Start)
            {
                txtbkInstruction.Text = "Please wait while ngrok is downloading";
                pbprogress.IsIndeterminate = true;
                btnNext.IsEnabled = false;
                state = State.Downloading;
                _ngrokManager.DownloadNgrok();
            }

            if (state == State.RegsiterAuthtoken)
            {
                pbprogress.Visibility = Visibility.Hidden;
                txtbkAuthToken.Visibility = Visibility.Visible;
                txtbxAuthToken.Visibility = Visibility.Visible;

                txtbkInstruction.Text =
                    "Please go to ngrok.com and find your Auth token. Paste it into the textbox below and click register";

                btnNext.Content = "Register";

                //Hyperlink hyperlink = new Hyperlink();
                //Run run = new Run();

                //run.Text = "Go to Ngrok";
                //hyperlink.NavigateUri = new Uri("https://dashboard.ngrok.com/get-started/setup");

                //txtbkInstruction.Inlines.Add("Please go to ");
                //txtbkInstruction.Inlines.Add(run);
                //txtbkInstruction.Inlines.Add(hyperlink);
                //txtbkInstruction.Inlines.Add(" and get your Auth token. Paste it into the textbox below and click Register");

                state = State.Done;
            }
        }

        private enum State
        {
            Start,
            Downloading,
            RegsiterAuthtoken,
            Done
        }
    }
}