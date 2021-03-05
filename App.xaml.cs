using System.Threading;
using System.Windows;

namespace ngrokGUI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "Ngrok gui";
            bool createdNew;

            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
                //app is already running! Exiting the application  
                Current.Shutdown();

            base.OnStartup(e);
        }
    }
}