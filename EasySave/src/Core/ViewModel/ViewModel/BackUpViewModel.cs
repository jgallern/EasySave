using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Core.ViewModel;
using Core.Model.Services;
using Core.ViewModel.Services;
using Core.ViewModel.Commands;
using Microsoft.Extensions.Options;
using Core.Model;
using Core.ViewModel.Notifiers;

namespace Core.ViewModel
{
    public class BackUpViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly ILocalizer _localizer;

        private readonly INavigationService _navigation;

        private readonly IUIErrorNotifier _notifier;

        private IFileDialogService _fileDialogService;

        private ViewModelBase _currentViewModel;
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


        public ICommand ModifyCommand { get; }
        public ICommand DeleteCommand { get;} 
        public ICommand ValidateCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BrowseSourceCommand { get; }
        public ICommand BrowseTargetCommand { get; }



        private bool _isNameInvalid;
        public bool IsNameInvalid
        {
            get => _isNameInvalid;
            set => SetProperty(ref _isNameInvalid, value);
        }

        private bool _isDirSourceInvalid;
        public bool IsDirSourceInvalid
        {
            get => _isDirSourceInvalid;
            set => SetProperty(ref _isDirSourceInvalid, value);
        }

        private bool _isDirTargetInvalid;
        public bool IsDirTargetInvalid
        {
            get => _isDirTargetInvalid;
            set => SetProperty(ref _isDirTargetInvalid, value);
        }



        private IJobs _job;

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                ValidateLabel = this[_isEditMode ? "modify" : "create"];
                OnPropertyChanged();
                OnPropertyChanged(nameof(Validate)); // Pour rafraîchir si lié en XAML
            }
        }

        public string ValidateLabel { get; set; }

        public string JobIdName
        { get; set; }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _sourcePath;
        public string dirSource
        {
            get => _sourcePath;
            set => SetProperty(ref _sourcePath, value);
        }
        private string _targetPath;
        public string dirTarget
        {
            get => _targetPath;
            set => SetProperty(ref _targetPath, value);
        }
        private bool _isEncryption;
        public bool Encryption
        {
            get => _isEncryption;
            set => SetProperty(ref _isEncryption, value);
        }

        private bool _isDifferential;
        public bool Differential
        {
            get => _isDifferential;
            set => SetProperty(ref _isDifferential, value);
        }

        public BackUpViewModel(ILocalizer localizer, INavigationService navigation, IUIErrorNotifier notifier)
        {
            _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(localizer));
            BrowseSourceCommand = new RelayCommand(BrowseSource);
            BrowseTargetCommand = new RelayCommand(BrowseTarget);
            ValidateCommand = new RelayCommand(_ => Validate());
            CancelCommand = new RelayCommand(_ => Cancel());
        }
        public void InitializeDialogService(IFileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;
        }

        public void LoadFromExistingJob(BackUpJob job)
        {
            _job = job;
            Id = job.Id;
            Name = job.Name;
            dirSource = job.dirSource;
            dirTarget = job.dirTarget;
            Encryption = job.Encryption;
            Differential = job.Differential;
            JobIdName = $"{job.Id}. {job.Name}";
        }


        private void Cancel()
        {
            _navigation.NavigateToMenu();
            _navigation.CloseBackUp();
        }

        private bool IsValid()
        {
            IsNameInvalid = string.IsNullOrEmpty(Name);
            IsDirSourceInvalid = string.IsNullOrEmpty(dirSource);
            IsDirTargetInvalid = string.IsNullOrEmpty(dirTarget);

            return !IsNameInvalid && !IsDirSourceInvalid && !IsDirTargetInvalid;
        }


        private void Validate()
        {
            if (IsValid())
            {
                if (IsEditMode)
                    AlterJob();
                else
                    CreateJob();
            }
        }

        private void AlterJob()
        {
            if (IsValid())
            {
                try
                {
                    if (_job == null) return;

                    _job.Name = Name;
                    _job.dirSource = dirSource;
                    _job.dirTarget = dirTarget;
                    _job.Encryption = Encryption;
                    _job.Differential = Differential;

                    _job.AlterJob();
                    _notifier.ShowSuccess(this["successfully_modify"]);
                    Cancel();
                }
                catch (Exception ex)
                {
                    _notifier.ShowError(ex.ToString());
                }
            }
        }


        private void CreateJob()
        {
            if (IsValid())
            {
                try
                {
                    _job = new BackUpJob(Name, dirSource, dirTarget, Differential, Encryption);
                    _job.CreateJob();
                    _notifier.ShowSuccess(this["job_created"]);
                    Cancel();
                }
                catch (Exception ex)
                {
                    _notifier.ShowError(ex.ToString());
                }
                
            }
        }

        private void BrowseSource(object obj)
        {
            var path = _fileDialogService.SelectFolder();
            if (!string.IsNullOrEmpty(path))
            {
                dirSource = path;
            }
        }
        private void BrowseTarget(object obj)
        {
            var path = _fileDialogService.SelectFolder();
            if (!string.IsNullOrEmpty(path))
            {
                dirTarget = path;
            }
        }
        public string this[string key] => _localizer[key];
    }
}