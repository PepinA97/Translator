using GalaSoft.MvvmLight.Command;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace TranslatorWPF.Windows.Devices
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        View View;

        public bool Show(View view)
        {
            view.DataContext = this;
            View = view;

            return view.ShowDialog().GetValueOrDefault();
        }

        void FinishExecute()
        {
            View.DialogResult = true;

            View.Close();
        }

        bool FinishCanExecute()
        {
            if(SelectedInput == null)
            {
                return false;
            }

            if(SelectedOutput == null)
            {
                return false;
            }

            return true;
        }

        void RefreshExecute()
        {
            var enumerator = new MMDeviceEnumerator();

            Inputs = new ObservableCollection<MMDevice>();
            foreach (var endpoint in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
            {
                Inputs.Add(endpoint);
            }

            Outputs = new ObservableCollection<MMDevice>();
            foreach (var endpoint in enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                Outputs.Add(endpoint);
            }
        }

        public string InputID
        {
            get
            {
                return SelectedInput.ID;
            }
        }

        public string OutputID
        {
            get
            {
                return SelectedOutput.ID;
            }
        }

        public RelayCommand Finish { get; set; }
        public RelayCommand Refresh { get; set; }

        MMDevice _SelectedInput;
        public MMDevice SelectedInput
        {
            get
            {
                return _SelectedInput;
            }
            set
            {
                _SelectedInput = value;
                Finish.RaiseCanExecuteChanged();
            }
        }

        MMDevice _SelectedOutput;
        public MMDevice SelectedOutput
        {
            get
            {
                return _SelectedOutput;
            }
            set
            {
                _SelectedOutput = value;
                Finish.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<MMDevice> Inputs { get; set; }
        public ObservableCollection<MMDevice> Outputs { get; set; }

        public ViewModel()
        {
            Finish = new RelayCommand(FinishExecute, FinishCanExecute);
            Refresh = new RelayCommand(RefreshExecute);

            Refresh.Execute(null);
        }
    }
}
