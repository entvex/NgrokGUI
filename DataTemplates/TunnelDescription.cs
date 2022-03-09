using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ngrokGUI
{
    public class TunnelDescription : INotifyPropertyChanged
    {
        private bool active;
        private Uri url;
        public int Port { get; set; }
        public string Protocol { get; set; }
        public string Name { get; set; }

        public Uri Url
        {
            get => url;
            set
            {
                url = value;
                OnPropertyChanged();
            }
        }

        public bool Active
        {
            get => active;
            set
            {
                active = value;
                OnPropertyChanged();
            }
        }

        public string SubDomain { get; set; }
        public string CustomDomain { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}