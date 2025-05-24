using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Core.ViewModel;
using Core.Model.Services;

namespace Core.ViewModel
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        private ViewModelBase _currentViewModel;
        private readonly ILocalizer _localizer;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand RedirectMenuCommand { get; }
        public ICommand RedirectSettingsCommand { get; }
        public ICommand ExitCommand { get; }

        public MenuViewModel(ILocalizer localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

            //RedirectSettingsCommand = new RelayCommand(_ => CurrentViewModel = new SettingsViewModel(_localizer));
            //ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());

            // Vue initiale
           // CurrentViewModel = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _currentLanguage;
        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage == value) return;
                _currentLanguage = value;
                _localizer.ChangeLanguage(value);
                OnPropertyChanged();
                OnPropertyChanged("Item[]"); // Pour rafraîchir les bindings indexeurs
            }
        }
        public string this[string key] => _localizer[key];

    }
}