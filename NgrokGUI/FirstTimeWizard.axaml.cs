using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NgrokGUI
{
    public class FirstTimeWizard : Window
    {
        public FirstTimeWizard()
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