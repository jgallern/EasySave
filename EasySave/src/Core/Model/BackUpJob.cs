using Core.Model.Managers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows;
using Core.ViewModel.Services;


namespace Core.Model
{
    public enum Statement
    {
        Running,
        Waiting,
        Paused,
		Canceled,
		Error,
        Done,
        NoStatement,
    }
    public class BackUpJob : IJobs, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static readonly object _lockStatement = new();

        private ManualResetEventSlim _pauseEventJob = new ManualResetEventSlim(true);

        private CancellationTokenSource _ctsJob = new();

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();

                    // Appel direct dans MainViewModel via un callback ou événement si besoin
                }
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }

		public string dirSource { get; set; }

        public string dirTarget { get; set; }

		public bool Differential{ get; set; }

        public bool ProcessEventPause { get; set; }
        public bool ManualEventPause { get; set; }

        public bool Encryption { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        private Statement _statement;
        public Statement Statement
        {
            get
            {
                lock (_lockStatement)
                {
                    return _statement;
                }
            }
            set
            {
                lock (_lockStatement)
                {
                    if (_statement != value)
                    {
                        _statement = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        private int _currentFile;
        public int CurrentFile
        {
            get => _currentFile;
            set
            {
                if (_currentFile != value)
                {
                    _currentFile = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _totalFiles;
        public int TotalFiles
        {
            get => _totalFiles;
            set
            {
                if (_totalFiles != value)
                {
                    _totalFiles = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _progress;
        public string Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _lastExecution;
        public DateTime LastExecution
        {
            get => _lastExecution;
            set
            {
                if (_lastExecution != value)
                {
                    _lastExecution = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? LastError { get; private set; }



        public BackUpJob(string Name, string dirSource, string dirTarget, bool Differential, bool Encryption)
		{
            this.Name = Name;
			this.dirSource = dirSource;
			this.dirTarget= dirTarget;
            this.Differential= Differential;
			this.Encryption = Encryption;
        }

		public BackUpJob()
		{
			this.Name = "";
			this.dirSource = "";
			this.dirTarget = "";
            this.Differential= false;
		}

        public Task Run()
        {
            return Task.Run(() => RunJobInThread(), _ctsJob.Token);

        }


        //Currently use to continue using the user interface 
        public void RunJobInThread()
        {
            try
            {
                Thread.Sleep(1000);
                IBackUpType backupType = Differential ?
                new BackUpDifferential(this) :
                new BackUpFull(this);
                backupType.Execute(_ctsJob.Token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void Pause()
        {
            if (ManualEventPause || ProcessEventPause)
            {
                _pauseEventJob.Reset(); // bloque
                Statement = Statement.Paused;
                ChangeStatement();
            }
        }

        public void Resume()
        {
            if (!ManualEventPause && !ProcessEventPause)
            { 
                if (Statement == Statement.Paused)
                {   
                    _pauseEventJob.Set(); // débloque
                    Statement = Statement.Running;
                    ChangeStatement();
                }
                else if (Statement == Statement.Waiting || Statement == Statement.Running)
                {
                    return;
                }
            }
        }

        public void Reset()
        {
            if (!ManualEventPause && !ProcessEventPause)
            {
                _ctsJob = new CancellationTokenSource();
            }
        }

        public void Stop()
        {
            Statement = Statement.Canceled;
            ChangeStatement();
            _ctsJob.Cancel();
        }

        public void WaitingPause()
        {
            _pauseEventJob.Wait();
        }

        public void ChangeStatement()
        {
            JobConfigManager.Instance.UpdateJob(Id, this);
        }

        public void CreateJob()
		{
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            Statement = Statement.NoStatement;
            Id = JobConfigManager.Instance.GetAvailableID();
            JobConfigManager.Instance.AddJob(this);
		}

		public void DeleteJob()
		{
			Id = JobConfigManager.Instance.FindJobId(this);
            JobConfigManager.Instance.DeleteJob(Id);
		}

		public void AlterJob()
		{
            ModificationDate = DateTime.Now;
            JobConfigManager.Instance.UpdateJob(Id, this);
		}

        public static List<BackUpJob> GetAllJobsFromConfig()
		{
			return JobConfigManager.Instance.GetAllJobs();
		}
		public static BackUpJob GetJobByID(int Id)
		{
			return JobConfigManager.Instance.GetJobById(Id);
		}
	}
}
