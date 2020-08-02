using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace TranslatorWPF.Windows.Main.Commands
{
    class StartTranslating : ICommand
    {
        ViewModel ViewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if(ViewModel.SelectedFromLanguage == null)
            {
                return false;
            }

            if(ViewModel.SelectedToLanguage == null)
            {
                return false;
            }

            return !ViewModel._IsTranslating;
        }

        public async void Execute(object parameter)
        {
            var config = SpeechTranslationConfig.FromSubscription(SubscriptionKey.Key, SubscriptionKey.Region);

            config.SpeechRecognitionLanguage = ViewModel.SelectedFromLanguage.From;
            config.AddTargetLanguage(ViewModel.SelectedToLanguage.To);
            config.VoiceName = ViewModel.SelectedToLanguage.VoiceName;

            config.SetProfanity(ProfanityOption.Raw);

            AudioConfig audioConfig = AudioConfig.FromDefaultMicrophoneInput();

            if(ViewModel.InputDeviceID != null)
            {
                audioConfig = AudioConfig.FromMicrophoneInput(ViewModel.InputDeviceID);
            }

            ViewModel.TranslationRecognizer = new TranslationRecognizer(config, audioConfig);

            ViewModel.TranslationRecognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.TranslatedSpeech)
                {
                    foreach (var element in e.Result.Translations)
                    {
                        lock (ViewModel.LogsLock)
                        {
                            ViewModel.AddLog($"{element.Key}: {element.Value}");
                        }
                    }
                }
            };

            ViewModel.TranslationRecognizer.Synthesizing += (s, e) =>
            {
                if(ViewModel.SelectedToLanguage.VoiceName != String.Empty)
                {
                    byte[] audio = e.Result.GetAudio();

                    if (audio.Length != 0)
                    {
                        MMDevice output = null;

                        var enumerator = new MMDeviceEnumerator();
                        foreach (var endpoint in enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
                        {
                            output = endpoint;

                            if (endpoint.ID == ViewModel.OutputDeviceID)
                            {
                                break;
                            }
                        }

                        if (output != null)
                        {
                            var deviceOutput = new WasapiOut(output, AudioClientShareMode.Shared, false, 0);

                            deviceOutput.Init(new WaveFileReader(new MemoryStream(audio)));

                            deviceOutput.Play();
                        }
                    }
                }
            };

            ViewModel.TranslationRecognizer.Canceled += (s, e) =>
            {
                lock (ViewModel.LogsLock)
                {
                    ViewModel.AddLog($"Recognition canceled. Reason: {e.Reason};\nErrorDetails: {e.ErrorDetails}");
                }
            };

            await ViewModel.TranslationRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

            ViewModel.IsTranslating = true;
            lock (ViewModel.LogsLock)
            {
                ViewModel.AddLog($"Translating from {ViewModel.SelectedFromLanguage.Name} to {ViewModel.SelectedToLanguage.Name}...");
            }

            CommandManager.InvalidateRequerySuggested();
        }

        public StartTranslating(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
