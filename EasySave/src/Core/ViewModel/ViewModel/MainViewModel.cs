using Core.Model.Services;
using Core.ViewModel.Commands;
using Core.ViewModel;
using Core.ViewModel.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Core.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ViewModelBase _currentViewModel;
        private readonly ILocalizer _localizer;
        private readonly INavigationService _navigationService;

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
        public ICommand SettingsCommand { get; }

        public MainViewModel(ILocalizer localizer, INavigationService navigation)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _navigationService = navigation;
            SettingsCommand = new RelayCommand(_ => Settings());
        }

        private void Settings()
        {
            _navigationService.NavigateToSettings();
            _navigationService.CloseMenu();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /* //Si on met la combobox de selection de language dans le menu principal
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
        */
        public string this[string key] => _localizer[key];

    }
}