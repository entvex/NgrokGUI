using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace NgrokGUI
{
    public class MainWindow : Window
    {
        public MainWindow()
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

        private void btnMenuItemAddNew_OnClick(object? sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void btnMenuItemExit_OnClick(object? sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void btnMenuItemRunFirstTimeWizard_OnClick(object? sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}