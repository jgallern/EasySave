using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Core.ViewModel
{
    public class SettingsCommands : ISettingsCommands, INotifyPropertyChanged
    {
        private readonly ILocalizer _localizer;
        public event PropertyChangedEventHandler PropertyChanged;

        public IReadOnlyList<string> AvailableLanguages => _localizer.GetAvailableLanguages();
        private string _currentLanguage;

        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value && AvailableLanguages.Contains(value))
                {
                    _currentLanguage = value;
                    _localizer.ChangeLanguage(value);
                    OnPropertyChanged("Item[]");
                }
            }
        }

        public ICommand ChangeLanguageCommand { get; }
        public string this[string key] => _localizer[key];

        public SettingsCommands(ILocalizer localizer)
        {
            _localizer = localizer;
            _currentLanguage = _localizer.GetCurrentLanguage();
        }

        // For WPF 
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}