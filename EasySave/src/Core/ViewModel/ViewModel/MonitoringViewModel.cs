using Core.ViewModel.Notifiers;
using Core.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Core.ViewModel;
using Core.Model.Services;
using Core.ViewModel.Commands;
using Microsoft.Extensions.Options;
using Core.Model;
using Core.ViewModel.Notifiers;

namespace Core.ViewModel
{
    public class MonitoringViewModel: ViewModelBase, INotifyPropertyChanged
    {
        private readonly INavigationService _navigation;

        private readonly IUIErrorNotifier _notifier;

        private BackUpJob _job;
        public BackUpJob Job
        {
            get => _job;
            set
            {
                if (_job != null)
                    _job.PropertyChanged -= Job_PropertyChanged; //Passe jamais ici

                _job = value;
                _job.PropertyChanged += Job_PropertyChanged; //¨Passe ici au premier et au deuxième
                OnPropertyChanged(nameof(Job));
                OnPropertyChanged(nameof(Progress));    // pour forcer la première lecture
                OnPropertyChanged(nameof(Statement));
            }
        }

        private void Job_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BackUpJob.Progress)
             || e.PropertyName == nameof(BackUpJob.CurrentFile)
             || e.PropertyName == nameof(BackUpJob.TotalFiles))
            {
                OnPropertyChanged(nameof(Progress));
                OnPropertyChanged(nameof(ProgressValue)); 
            }
            else if (e.PropertyName == nameof(BackUpJob.Statement))
                OnPropertyChanged(nameof(Statement));
        }

        public int Id => Job.Id;
        public string Name => Job.Name;
        public string dirSource => Job.dirSource;
        public string dirTarget => Job.dirTarget;
        public string Progress => Job.Progress;
        public Statement Statement => Job.Statement;



        private double _progressValue;
        public double ProgressValue
        {
            get
            {
                if (Job?.TotalFiles > 0)
                    return Job.CurrentFile * 100.0 / Job.TotalFiles;
                return 0;
            }
        }




        public ICommand RunCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ExitCommand { get; }


        public MonitoringViewModel(INavigationService navigation, IUIErrorNotifier notifier)
        {
            _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            RunCommand = new RelayCommand(_ => Run());
            PauseCommand = new RelayCommand(_ => Pause());
            CancelCommand = new RelayCommand(_ => Cancel());
            ExitCommand = new RelayCommand(_ => Exit());
        }

        public void LoadFromExistingJob(BackUpJob job) // Copie de l'objet ? Objectif récupérer l'objet entrain d'etre modifier dans le thread
        {
            Job = job;
        }

        private void Exit()
        {
            _navigation.CloseMonitoring();
        }


        public void Run()
        {
            _job.Run();
        }

        public void Pause()
        {
            _job.Pause();
        }

        public void Cancel()
        {
            _job.Stop();
        }
    }
}
