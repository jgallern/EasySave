using Core.Model.Services;
using Core.ViewModel.Services;
using Core.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Core.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        //private readonly ILocalizer _localizer;
        private readonly INavigationService _navigationService;

        private readonly RelayCommand _changeSettingsCommand;
        public ICommand ChangeSettingsCommand => _changeSettingsCommand;

        public ICommand ExitCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand CloseCommand { get; }

        public event Action RequestClose;

        public SettingsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Initialisation des valeurs
            EditedPriorityFiles = _localizer.GetPriorityFiles();
            EditedExtensions = _localizer.GetEncryptionExtensions();
            EditedSoftwarePackages = _localizer.GetSoftwarePackages();
            EditedKey = _localizer.GetEncryptionKey();
            CurrentLanguage = _localizer.GetCurrentLanguage();
            AvailableLanguages = _localizer.GetAvailableLanguages();

            // Commandes
            EditCommand = new RelayCommand(_ => IsEditing = true);

            CancelCommand = new RelayCommand(_ =>
            {
                EditedPriorityFiles = _localizer.GetPriorityFiles();
                EditedExtensions = _localizer.GetEncryptionExtensions();
                EditedSoftwarePackages = _localizer.GetSoftwarePackages();
                EditedKey = _localizer.GetEncryptionKey();
                IsEditing = false;
            });

            _changeSettingsCommand = new RelayCommand(_ => SaveAll(),
                                            _ => IsEditing);

            ExitCommand = new RelayCommand(_ => Exit());

        }

        private void Exit()
        {
            _navigationService.NavigateToMenu();
            _navigationService.CloseSettings();
        }


        private void SaveAll()
        {
            EditedPriorityFiles = _localizer.ChangePriorityFiles(EditedPriorityFiles);
            EditedExtensions = _localizer.ChangeEncryptionExtensions(EditedExtensions);
            EditedSoftwarePackages = _localizer.ChangeSoftwarePackages(EditedSoftwarePackages);
            EditedKey = _localizer.ChangeEncryptionKey(EditedKey);

            IsEditing = false;  // will trigger the RaiseCanExecuteChanged
        }


        // État édition ou création
        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (_isEditing == value) return;
                _isEditing = value;
                OnPropertyChanged();
                _changeSettingsCommand.RaiseCanExecuteChanged(); //Use to change the execution statement
            }

        }

        // Texte éditable pour les fichiers prioritaires
        private string _editedPriorityFiles;
        public string EditedPriorityFiles
        {
            get => _editedPriorityFiles;
            set { _editedPriorityFiles = value; OnPropertyChanged(); }
        }


        // Texte éditable pour les extensions
        private string _editedExtensions;
        public string EditedExtensions
        {
            get => _editedExtensions;
            set { _editedExtensions = value; OnPropertyChanged(); }
        }

        // Texte éditable pour les paquets logiciels
        private string _editedSoftwarePackages;
        public string EditedSoftwarePackages
        {
            get => _editedSoftwarePackages;
            set { _editedSoftwarePackages = value; OnPropertyChanged(); }
        }

        // Texte éditable pour la clé d'encryptage
        private string _editedKey;
        public string EditedKey
        {
            get => _editedKey;
            set { _editedKey = value; OnPropertyChanged(); }
        }

        // Langues disponibles et courante
        public IReadOnlyList<string> AvailableLanguages { get; }
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

        // Accès aux traductions par indexeur
        public string this[string key] => _localizer[key];
    }
}
