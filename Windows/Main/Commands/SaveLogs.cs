using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TranslatorWPF.Windows.Main.Commands
{
    class SaveLogs : ICommand
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
            throw new NotImplementedException();
        }

        public SaveLogs(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
