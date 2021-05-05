using Avalonia;
using System;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using NgrokGUI.ViewModels;
using ReactiveUI;

namespace NgrokGUI.Views
{
    public class AddNewTunnelWindow : ReactiveWindow<AddNewTunnelViewModel>
    {
        public AddNewTunnelWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            this.WhenActivated(d => d(ViewModel.AddNewTunnelCommand.Subscribe(Close)));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}