using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Core.ViewModel;
using Core.Model.Services;
using Core.ViewModel.Services;
using Core.ViewModel.Commands;

namespace Core.ViewModel
{
    public class BackUpViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly ILocalizer _localizer;

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

        private readonly IFileDialogService _fileDialogService;

        public ICommand BrowseSourceCommand { get; }
        public ICommand BrowseTargetCommand { get; }

        private string _sourcePath;
        public string SourcePath
        {
            get => _sourcePath;
            set => SetProperty(ref _sourcePath, value);
        }
        private string _targetPath;
        public string TargetPath
        {
            get => _targetPath;
            set => SetProperty(ref _targetPath, value);
        }
        public bool IsEncryption { get; set; }
        public bool IsDifferential { get; set; }

        public BackUpViewModel(ILocalizer localizer, INavigationService navigation, IFileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

            BrowseSourceCommand = new RelayCommand(BrowseSource);
            BrowseTargetCommand = new RelayCommand(BrowseTarget);
        }


        private void BrowseSource(object obj)
        {
            var path = _fileDialogService.SelectFolder();
            if (!string.IsNullOrEmpty(path))
            {
                SourcePath = path;
            }
        }
        private void BrowseTarget(object obj)
        {
            var path = _fileDialogService.SelectFolder();
            if (!string.IsNullOrEmpty(path))
            {
                TargetPath = path;
            }
        }
        public string this[string key] => _localizer[key];
    }
}