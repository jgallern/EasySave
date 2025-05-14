using BackUp.Model;
using System.ComponentModel;

namespace BackUp.ViewModel
{
    public class ManageBackUpServices
    {
        public ManageBackUpServices()
        {

        }
    }
    /*
    
    public class ManageBackUpServicesViewModel : IManageBackUpServices, INotifyPropertyChanged
    {
        // 1) Propriétés exposées (contrat de l’interface)
        public ICommand CreateBackUpCommand { get; }
        public ICommand AlterBackUpCommand { get; }
        public ICommand DeleteBackUpCommand { get; }
        public ICommand SelectBackUpCommand { get; }
        public ICommand SelectAllJobsCommand { get; }

        // 2) Champs privés pour stocker les RelayCommand (optionnel mais pratique pour RaiseCanExecuteChanged)
        private readonly RelayCommand _createCmd;
        private readonly RelayCommand _alterCmd;
        private readonly RelayCommand _deleteCmd;
        private readonly RelayCommand _selectCmd;
        private readonly RelayCommand _selectAllCmd;

        // Un service métier injecté pour réellement réaliser les backups
        private readonly IJobs _jobsModel;

        // Une propriété pour la sauvegarde sélectionnée
        private BackUpJob _selectedBackUp;
        public BackUpJob SelectedBackUp
        {
            get => _selectedBackUp;
            set
            {
                if (_selectedBackUp != value)
                {
                    _selectedBackUp = value;
                    OnPropertyChanged(nameof(SelectedBackUp));
                    // chaque fois qu’on change de sélection, on réévalue CanExecute
                    _alterCmd.RaiseCanExecuteChanged();
                    _deleteCmd.RaiseCanExecuteChanged();
                }
            }
        }

        public ManageBackUpServices(IJobs service)
        {
            _jobsModel = service;

            // 3) Création des RelayCommand en pointant sur les méthodes
            _createCmd = new RelayCommand(
                _ => CreateBackUp(),
                _ => CanCreateBackUp()
            );
            _alterCmd = new RelayCommand(
                _ => AlterBackUp(),
                _ => CanAlterBackUp()
            );
            _deleteCmd = new RelayCommand(
                _ => DeleteBackUp(),
                _ => CanDeleteBackUp()
            );
            _selectCmd = new RelayCommand(
                param => SelectBackUp((BackUpJob)param),
                param => param is BackUpJob
            );

            // 1) On affecte nos champs RelayCommand aux propriétés ICommand
            CreateBackUpCommand = _createCmd;
            AlterBackUpCommand = _alterCmd;
            DeleteBackUpCommand = _deleteCmd;
            SelectBackUpCommand = _selectCmd;
        }

        // 4) Méthodes appelées par Execute
        private void CreateBackUp()
        {
            var newBackup = _jobsModel.Create();
            // par ex. Actualiser une liste, etc.
        }
        private bool CanCreateBackUp() => true;  // toujours possible ici

        private void AlterBackUp()
        {
            if (SelectedBackUp != null)
                _jobsModel.Alter(SelectedBackUp);
        }
        private bool CanAlterBackUp() => SelectedBackUp != null;

        private void DeleteBackUp()
        {
            if (SelectedBackUp != null)
                _jobsModel.Delete(SelectedBackUp.Id);
        }
        private bool CanDeleteBackUp() => SelectedBackUp != null;

        private void SelectBackUp(BackUpJob backup)
        {
            SelectedBackUp = backup;
        }

        // INotifyPropertyChanged pour les bindings
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }*/
}
