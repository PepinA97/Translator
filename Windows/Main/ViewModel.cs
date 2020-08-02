using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using TranslatorWPF.Models;
using TranslatorWPF.Windows.Main.Commands;

namespace TranslatorWPF.Windows.Main
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public object LogsLock;

        public string InputDeviceID;
        public string OutputDeviceID;

        public void SetDeviceIDs(string inputID, string outputID)
        {
            InputDeviceID = inputID;
            OutputDeviceID = outputID;
        }

        public TranslationRecognizer TranslationRecognizer { get; set; }

        public void AddLog(string log)
        {
            Logs += $"{DateTime.Now} - {log}\n";
        }

        public Language SelectedFromLanguage { get; set; }

        public Language SelectedToLanguage { get; set; }

        public ObservableCollection<Language> Languages { get; set; }

        string _Logs;
        public string Logs
        {
            get
            {
                return _Logs;
            }
            set
            {
                _Logs = value;
                OnPropertyChanged("Logs");
            }
        }

        public bool _IsTranslating;
        public bool IsTranslating
        {
            get
            {
                return _IsTranslating;
            }
            set
            {
                _IsTranslating = value;

                OnPropertyChanged("IsNotTranslating");
            }
        }

        public bool IsNotTranslating
        {
            get
            {
                return !_IsTranslating;
            }
        }

        public ICommand SaveLogs { get; set; }
        public ICommand SelectDevices { get; set; }
        public ICommand StartTranslating { get; set; }
        public ICommand StopTranslating { get; set; }

        public void Show(View view)
        {
            view.DataContext = this;

            view.ShowDialog();
        }

        public ViewModel()
        {
            IsTranslating = false;

            SaveLogs = new SaveLogs(this);
            SelectDevices = new SelectDevices(this);
            StartTranslating = new StartTranslating(this);
            StopTranslating = new StopTranslating(this);

            Logs = String.Empty;

            // Create locks for accessing across threads
            LogsLock = new object();
            BindingOperations.EnableCollectionSynchronization(Logs, LogsLock);

            Languages = new ObservableCollection<Language>
            {
                new Language("English", "en", "en-US", "en-US-ZiraRUS"),
                new Language("Arabic", "ar", "ar-SA", ""),
                new Language("Danish", "da", "da-DK", ""),
                new Language("German", "de", "de-DE", ""),
                new Language("Greek", "el", "el-EL", ""),
                new Language("Spanish", "es", "es-ES", ""),
                new Language("Finnish", "fi", "fi-FI", ""),
                new Language("Portuguese", "pt", "pt-BR", "pt-BR-HeloisaRUS"),
                new Language("French", "fr", "fr-FR", ""),
                new Language("Hindi", "hi", "hi-IN", ""),
                new Language("Hungarian", "hu", "hu-HU", ""),
                new Language("Italian", "it", "it-IT", ""),
                new Language("Hebrew", "he", "he-IL", ""),
                new Language("Russian", "ru", "ru-RU", "ru-RU-Irina-Apollo")
            };
        }
    }
}
