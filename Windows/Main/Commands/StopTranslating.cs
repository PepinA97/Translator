using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TranslatorWPF.Windows.Main.Commands
{
    class StopTranslating : ICommand
    {
        ViewModel ViewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return ViewModel._IsTranslating;
        }

        public async void Execute(object parameter)
        {
            await ViewModel.TranslationRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

            ViewModel.IsTranslating = false;
            lock (ViewModel.LogsLock)
            {
                ViewModel.AddLog($"Translation stopped...");
            }

            CommandManager.InvalidateRequerySuggested();
        }

        public StopTranslating(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
