using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using Avalonia.Controls;
using NgrokGUI.Models;
using NgrokGUI.Views;
using NgrokSharp;
using ReactiveUI;

namespace NgrokGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        
        public ObservableCollection<TunnelDescription> TunnelDescriptions { get; } = new();
        public MainWindowViewModel()
        {

            ShowAddNewTunnelDialog = new Interaction<AddNewTunnelViewModel, StartTunnelDTO>();
            var dgTunnels = MainWindow.Instance.Find<DataGrid>("dgTunnels");

            NewTunnel = ReactiveCommand.CreateFromTask(async () =>
            {
                var addNewTunnel = new AddNewTunnelViewModel();

                var result = await ShowAddNewTunnelDialog.Handle(addNewTunnel);

                if (result != null)
                {

                    var meow = new TunnelDescription();
                    meow.Name = "nsa";
                    meow.Port = 8080;
                    meow.Protocol = "https";
                    
                    TunnelDescriptions.Add(meow);
                    
                    
                    //TODO handle result
                }
                
            });

            ExitApplicationCommand = ReactiveCommand.Create(() => { Environment.Exit(0); });
        }

        public ICommand NewTunnel { get; }
        
        public ICommand ExitApplicationCommand { get; }
        
        public ICommand CopylinkCommand { get; }
        
        public Interaction<AddNewTunnelViewModel,StartTunnelDTO> ShowAddNewTunnelDialog { get; }
    }
    
    
}