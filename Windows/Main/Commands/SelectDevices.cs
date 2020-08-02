using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TranslatorWPF.Windows.Main.Commands
{
    class SelectDevices : ICommand
    {
        ViewModel ViewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return !ViewModel._IsTranslating;
        }

        public void Execute(object parameter)
        {
            Devices.ViewModel viewModel = new Devices.ViewModel();

            if(viewModel.Show(new Devices.View()))
            {
                ViewModel.SetDeviceIDs(viewModel.InputID, viewModel.OutputID);
            }
        }

        public SelectDevices(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
