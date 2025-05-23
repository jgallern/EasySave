using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading;
using System.Diagnostics;

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
                    Debug.WriteLine("language Changed ({_currentLanguage}) -> ({value})");
                    _currentLanguage = value;
                    _localizer.ChangeLanguage(value);
                    OnPropertyChanged("Item[]");
                }
            }
        }

        public string this[string key] => _localizer[key];

        public SettingsCommands(ILocalizer localizer)
        {
            Debug.WriteLine("Started");
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