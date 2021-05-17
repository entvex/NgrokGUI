using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace NgrokGUI
{
    public class TunnelDescription : INotifyPropertyChanged
    {
        public int Port { get; set; }
        public string Protocol { get; set; }
        public string Name { get; set; }
        public Uri Url { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}