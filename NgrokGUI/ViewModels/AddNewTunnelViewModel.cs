using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using NgrokSharp;
using ReactiveUI;

namespace NgrokGUI.ViewModels
{
    public class AddNewTunnelViewModel : ViewModelBase
    {
        
        private string? _selectedProtocol;
        private string? _tunnelName;
        private int? _localPort = 8080;
        public ObservableCollection<string> ProtocolList { get; } = new();
        
        public AddNewTunnelViewModel()
        {
            ProtocolList.Add("https");
            ProtocolList.Add("http");
            ProtocolList.Add("tdc");

            _selectedProtocol = "https";
            
            var okEnabled = this.WhenAnyValue(
                x => x.TunnelName,
                x => !string.IsNullOrWhiteSpace(x));
            
            AddNewTunnelCommand = ReactiveCommand.Create(() =>
            {
                
                var startTunnelDto = new StartTunnelDTO
                {
                    name = TunnelName.Trim(),
                    proto = SelectedProtocol,
                    addr = LocalPort.ToString(),
                    bind_tls = "false"
                };
                
                // bind_tls http bind an HTTPS or HTTP endpoint or both true, false, or both
                if (startTunnelDto.proto == "https")
                {
                    startTunnelDto.proto = "http";
                    startTunnelDto.bind_tls = "true";
                }
                
                return startTunnelDto;
            },okEnabled);
        }

        public string? SelectedProtocol
        {
            get => _selectedProtocol;
            set => this.RaiseAndSetIfChanged(ref _selectedProtocol, value);
        }

        public string? TunnelName
        {
            get => _tunnelName;
            set => this.RaiseAndSetIfChanged(ref _tunnelName, value);
        }

        public int? LocalPort
        {
            get => _localPort;
            set => this.RaiseAndSetIfChanged(ref _localPort, value);
        }
        
        public ReactiveCommand<Unit, StartTunnelDTO> AddNewTunnelCommand { get; }
    }
}