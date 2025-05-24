using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Core.Model.Services;

namespace Core.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ILocalizer _localizer;

        private readonly RelayCommand _changeSettingsCommand;
        public ICommand ChangeSettingsCommand => _changeSettingsCommand;

        public SettingsViewModel(ILocalizer localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

            // Initialisation des valeurs
            EditedExtensions = _localizer.GetEncryptionExtensions();
            EditedSoftwarePackages = _localizer.GetSoftwarePackages();
            EditedKey = _localizer.GetEncryptionKey();
            CurrentLanguage = _localizer.GetCurrentLanguage();
            AvailableLanguages = _localizer.GetAvailableLanguages();

            // Commandes
            EditCommand = new RelayCommand(_ => IsEditing = true);

            CancelCommand = new RelayCommand(_ =>
            {
                // Restaure l’état antérieur
                EditedExtensions = _localizer.GetEncryptionExtensions();
                EditedSoftwarePackages = _localizer.GetSoftwarePackages();
                EditedKey = _localizer.GetEncryptionKey();
                IsEditing = false;
            });

            _changeSettingsCommand = new RelayCommand(_ => SaveAll(),
                                         _ => IsEditing);


            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }

        private void SaveAll()
        {
            _localizer.ChangeEncryptionExtensions(EditedExtensions);
            _localizer.ChangeSoftwarePackages(EditedSoftwarePackages);
            _localizer.ChangeEncryptionKey(EditedKey);

            // Reload the values to be sure that Localizer don't transform the string
            EditedExtensions = _localizer.GetEncryptionExtensions();
            EditedSoftwarePackages = _localizer.GetSoftwarePackages();
            EditedKey = _localizer.GetEncryptionKey();

            IsEditing = false;  // will trigger the RaiseCanExecuteChanged
        }


        // État édition
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

        // Commandes exposées à la vue
        public ICommand EditCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand CloseCommand { get; }

        public event Action RequestClose;



    }
}
