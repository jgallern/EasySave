using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BackUp.ViewModel
{
    public class SettingsViewModel : ISettingsViewModel, INotifyPropertyChanged
    {
        private readonly ILocalizer _localizer;
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsViewModel(ILocalizer localizer)
        {
            _localizer = localizer;
            //For WPF
            ChangeLanguageCommand = new RelayCommand(obj =>
            {
                if (obj is string lang)
                {
                    _localizer.ChangeLanguage(lang);
                    OnPropertyChanged(nameof(CurrentLanguage));
                }
            });
        }

        public IReadOnlyList<string> AvailableLanguages => _localizer.GetAvailableLanguages();
        public string CurrentLanguage => _localizer.GetCurrentLanguage();
        public ICommand ChangeLanguageCommand { get; }
        public string this[string key] => _localizer[key];

        // For WPF 
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}