using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NgrokSharp;

namespace NgrokGUI
{
    public class FirstTimeWizard : Window
    {
        private readonly INgrokManager _ngrokManager;
        private State state;

        
        private readonly TextBox txtbxAuthToken;
        private readonly Button btnNext;
        private readonly Process pbprogress;
        private readonly TextBox txtbkInstruction;
        
        public FirstTimeWizard(INgrokManager ngrokManager)
        {
            TextBox txtbkInstruction;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _ngrokManager = ngrokManager;
            _ngrokManager.DownloadAndUnZipDone += _ngrokManager_DownloadAndUnZipDone;
            state = State.Start;

            //Wire up controls

            btnNext = this.Find<Button>("btnNext");
            pbprogress = this.Find<Process>("pbprogress");
            txtbxAuthToken = this.FindControl<TextBox>("txtbxAuthToken");
            txtbkInstruction = this.FindControl<TextBox>("txtbkInstruction");
            
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
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
                _ngrokManager.RegisterAuthToken(txtbxAuthToken.Text);
                Close("true");
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
                pbprogress.
                pbprogress.Visibility = Visibility.Hidden;
                txtbkAuthToken.Visibility = Visibility.Visible;
                txtbxAuthToken.Visibility = Visibility.Visible;

                txtbkInstruction.Text =
                    "Please go to ngrok.com and find your Auth token. Paste it into the textbox below and click register";

                btnNext.Content = "Register";

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