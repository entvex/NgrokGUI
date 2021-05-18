using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using NgrokGUI.ViewModels;
using ReactiveUI;
using System;
using Avalonia.Interactivity;
using NgrokSharp;

namespace NgrokGUI.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        
        public static MainWindow? Instance { get; private set; }
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            
            
            this.WhenActivated(d => d(ViewModel.ShowAddNewTunnelDialog.RegisterHandler(DoShowDialogAsync)));
        }
        
        private async Task DoShowDialogAsync(InteractionContext<AddNewTunnelViewModel,StartTunnelDTO> interaction)
        {
            var dialog = new AddNewTunnelWindow();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<StartTunnelDTO?>(this);
            interaction.SetOutput(result);
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}