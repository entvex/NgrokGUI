using System.Reactive;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace NgrokGUI.Views
{
    public class AddNewTunnelView : UserControl
    {
        public AddNewTunnelView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        public ReactiveCommand<Unit,string?> AddNewTunnelCommand { get; }
    }
}