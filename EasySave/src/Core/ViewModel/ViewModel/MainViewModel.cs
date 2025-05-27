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
using Core.ViewModel.Notifiers;
using Core.Model.Managers;

namespace Core.ViewModel
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IUIErrorNotifier _notifier;


        public ObservableCollection<BackUpJob> JobsList { get; }

        public ICommand SettingsCommand { get; }
        public ICommand ShowLogsCommand { get; }
        public ICommand CreateJobCommand { get; }
        public ICommand ModifyJobCommand { get; }
        public ICommand DeleteJobCommand { get; }
        public ICommand ExecuteSelectedJobsCommand { get; }


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
                        job.IsSelected = value;
                    OnPropertyChanged();
                }
            }
        }


        public MainViewModel(INavigationService navigation, IUIErrorNotifier notifier)
        {
            _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
            _navigationService = navigation;
            CurrentLanguage = _localizer.GetCurrentLanguage();
            AvailableLanguages = _localizer.GetAvailableLanguages();
            JobsList = new ObservableCollection<BackUpJob>(BackUpJob.GetAllJobsFromConfig());

            ExecuteSelectedJobsCommand = new RelayCommand(_ => ExecuteSelectedJobs());
            ShowLogsCommand = new RelayCommand(_ => ShowLogs());
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
        }

        private void ExecuteSelectedJobs()
        {
            var selectedJobs = GetSelectedJobs();
            
            foreach (var job in selectedJobs)
            {
                job.Statement = Statement.Waiting;
            }
            RunJobManager.ExecuteSelectedJobs(selectedJobs, _notifier);
        }


        public List<BackUpJob> GetSelectedJobs()
        {
            List<BackUpJob> selectedJobs = JobsList.Where(job => job.IsSelected).ToList();
            _notifier.ShowSuccess($"Jobs sélectionnés : {string.Join(", ", selectedJobs.Select(j => j.Id))}");

            return selectedJobs;


        }


        private void Settings()
        {
            _navigationService.NavigateToSettings();
            _navigationService.CloseMenu();
        }

        private void ShowLogs()
        {
            try
            {
                ILogger logger = Logger.Instance;
                logger.OpenLogs();
            }
            catch (Exception ex) 
            {
                _notifier.ShowError(ex.ToString());
            }
        }

        private void CreateJob()
        {
            BackUpViewModel viewModel = new BackUpViewModel(_navigationService, _notifier)
            {
                IsEditMode = false,
            };

            _navigationService.NavigateToBackUp(viewModel);
            _navigationService.CloseMenu();
        }

        private void AlterJob(BackUpJob selectedJob)
        {
            BackUpViewModel viewModel = new BackUpViewModel(_navigationService, _notifier)
            {
                IsEditMode = true
            };
            viewModel.LoadFromExistingJob(selectedJob);

            _navigationService.NavigateToBackUp(viewModel);
            _navigationService.CloseMenu();
        }

        private void DeleteJob(BackUpJob selectedJob)
        {
            
            if (_notifier.ShowWarning($"{this["delete_job"]}{selectedJob.Id}. {selectedJob.Name}"))
            {
                try
                {
                    selectedJob.DeleteJob();
                    JobsList.Remove(selectedJob);
                    //RefreshJobs();
                }
                catch (Exception ex)
                {
                    _notifier.ShowError(ex.Message);
                }
            }            

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

    }
}