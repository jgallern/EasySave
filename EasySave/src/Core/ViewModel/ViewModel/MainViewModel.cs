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


        public ObservableCollection<BackUpJob> JobsList { get; }

        private IManageBackUpServices _backUpServices;
        public ICommand SettingsCommand { get; }
        public ICommand CreateJobCommand { get; }
        public ICommand ModifyJobCommand { get; }
        public ICommand DeleteJobCommand { get; }
        public ICommand ExecuteJobsCommand { get; }

        public MainViewModel(ILocalizer localizer, INavigationService navigation)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _navigationService = navigation;
            CurrentLanguage = _localizer.GetCurrentLanguage();
            AvailableLanguages = _localizer.GetAvailableLanguages();
            JobsList = new ObservableCollection<BackUpJob>(BackUpJob.GetAllJobsFromConfig());
            
            //ExecuteSelectedJobsCommand = new RelayCommand(_ => ExecuteSelectedJobs());
            SettingsCommand = new RelayCommand(_ => Settings());
            CreateJobCommand = new RelayCommand(_ => CreateJob());
            ModifyJobCommand = new RelayCommand(job =>
            {
                if (job is BackUpJob backupJob)
                    AlterJob(backupJob);
            });
            DeleteJobCommand = new RelayCommand(job =>
            {
                if (job is BackUpJob backupJob)
                    DeleteJob(backupJob);
            });

            ExecuteJobsCommand = new RelayCommand(_ => ExecuteSelectedJobs());
        }

        private void ExecuteSelectedJobs()
        {
            var selectedJobs = GetSelectedJobs();

            foreach (var job in selectedJobs)
            {
                job.Run();
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
            BackUpViewModel viewModel = new BackUpViewModel(_localizer, _navigationService)
            {
                IsEditMode = false,
            };

            _navigationService.NavigateToBackUp(viewModel);
            _navigationService.CloseMenu();
        }

        private void AlterJob(BackUpJob selectedJob)
        {
            BackUpViewModel viewModel = new BackUpViewModel(_localizer, _navigationService)
            {
                IsEditMode = true
            };
            viewModel.LoadFromExistingJob(selectedJob);

            _navigationService.NavigateToBackUp(viewModel);
            _navigationService.CloseMenu();
        }

        private void DeleteJob(BackUpJob selectedJob)
        {
            selectedJob.DeleteJob();
            JobsList.Remove(selectedJob);
            //RefreshJobs();
        }

        private void RefreshJobs()
        {
            JobsList.Clear();
            foreach (var job in BackUpJob.GetAllJobsFromConfig())
            {
                JobsList.Add(job);
            }
        }

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