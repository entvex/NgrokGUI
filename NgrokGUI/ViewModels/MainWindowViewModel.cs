using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using ReactiveUI;

namespace NgrokGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {

            ShowDialog = new Interaction<AddNewTunnelViewModel, FirstTimeWizardViewModel?>();
            
            NewTunnel = ReactiveCommand.CreateFromTask(async () =>
            {
                var addNewTunnel = new AddNewTunnelViewModel();

                var result = await ShowDialog.Handle(addNewTunnel);

                if (result != null)
                {
                    //TODO handle result
                }
                
            });
        }

        public ICommand NewTunnel { get; }
        
        public Interaction<AddNewTunnelViewModel,FirstTimeWizardViewModel?> ShowDialog { get; }
    }
    
    
}