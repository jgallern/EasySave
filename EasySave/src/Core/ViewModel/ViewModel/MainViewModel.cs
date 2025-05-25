using Core.Model.Services;
using Core.ViewModel;
using Core.ViewModel.Commands;
using Core.ViewModel.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Core.Model;

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

        private bool _areAllSelected;
        public bool AreAllSelected
        {
            get => _areAllSelected;
            set
            {
                if (_areAllSelected != value)
                {
                    _areAllSelected = value;
                    foreach (var job in JobsList)
                        job.isSelected = value;
                    OnPropertyChanged(nameof(AreAllSelected));
                }
            }
        }

        public ObservableCollection<BackUpJob> JobsList { get; }

        private IManageBackUpServices _backUpServices;
        public ICommand SettingsCommand { get; }
        public ICommand CreateJobCommand { get; }

        public MainViewModel(ILocalizer localizer, INavigationService navigation)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _navigationService = navigation;
            CurrentLanguage = _localizer.GetCurrentLanguage();
            AvailableLanguages = _localizer.GetAvailableLanguages();
            _backUpServices = new ManageBackUpServices();
            JobsList = new ObservableCollection<BackUpJob>(_backUpServices.GetAllJobs());
            
            //ExecuteSelectedJobsCommand = new RelayCommand(_ => ExecuteSelectedJobs());
            SettingsCommand = new RelayCommand(_ => Settings());
            CreateJobCommand = new RelayCommand(_ => CreateJob());
        }

        private void ExecuteSelectedJobs()
        {
            var selectedJobs = GetSelectedJobs();

            foreach (var job in selectedJobs)
            {
                // Call the execution with threading
            }
        }


        public IEnumerable<BackUpJob> GetSelectedJobs()
        {
            return JobsList.Where(job => job.isSelected);
        }


        private void Settings()
        {
            _navigationService.NavigateToSettings();
            _navigationService.CloseMenu();
        }
        private void CreateJob()
        {
            _navigationService.NavigateToBackUp();
            _navigationService.CloseMenu();
        }
        /*
        private void AlterJob(id)
        {
            ViewModel viewModel = new BackUpViewModel(...);
            viewModel.IsEditMode = true;

            // Préremplir les champs :
            viewModel.Id = selectedJob.Id;
            viewModel.Name = selectedJob.Name;
            viewModel.SourcePath = selectedJob.Source;
            viewModel.TargetPath = selectedJob.Target;
            viewModel.IsEncryption = selectedJob.Encryption;
            viewModel.IsDifferential = selectedJob.Differential;
            _navigationService.CloseMenu();
        }
        private void DeleteJob(id)
        {
            _navigationService.NavigateToBackUp("delete", id);
            _navigationService.CloseMenu();
        }
        */


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Si on met la combobox de selection de language dans le menu principal
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
        public string this[string key] => _localizer[key];

    }
}