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

        private string _sourcePath;
        public string SourcePath
        {
            get => _sourcePath;
            set => SetProperty(ref _sourcePath, value);
        }
        public BackUpViewModel(INavigationService navigation, IFileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;

            BrowseSourceCommand = new RelayCommand(BrowseSource);
        }


        private void BrowseSource(object obj)
        {
            var path = _fileDialogService.SelectFolder();
            if (!string.IsNullOrEmpty(path))
            {
                SourcePath = path;
            }
        }

    }
}