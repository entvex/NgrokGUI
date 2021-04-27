using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NgrokGUI.Views
{
    public class AddNewTunnelWindow : Window
    {
        public AddNewTunnelWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}