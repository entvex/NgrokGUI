using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using NgrokSharp;
using ReactiveUI;

namespace NgrokGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {

            ShowAddNewTunnelDialog = new Interaction<AddNewTunnelViewModel, StartTunnelDTO>();
            
            NewTunnel = ReactiveCommand.CreateFromTask(async () =>
            {
                var addNewTunnel = new AddNewTunnelViewModel();

                var result = await ShowAddNewTunnelDialog.Handle(addNewTunnel);

                if (result != null)
                {
                    //TODO handle result
                }
                
            });
        }

        public ICommand NewTunnel { get; }
        
        public Interaction<AddNewTunnelViewModel,StartTunnelDTO> ShowAddNewTunnelDialog { get; }
    }
    
    
}