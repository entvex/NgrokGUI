using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using NgrokGUI.ViewModels;
using ReactiveUI;

namespace NgrokGUI.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            
            
            this.WhenActivated(d => d(ViewModel.ShowDialog.RegisterHandler(DoShowDialogAsync)));
        }
        
        private async Task DoShowDialogAsync(InteractionContext<AddNewTunnelViewModel, FirstTimeWizardViewModel?> interaction)
        {
            var dialog = new AddNewTunnelWindow();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<FirstTimeWizardViewModel?>(this);
            interaction.SetOutput(result);
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}