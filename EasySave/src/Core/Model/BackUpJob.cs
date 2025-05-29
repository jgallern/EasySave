using Core.Model.Managers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;


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
            return Task.Run(() => { RunJobInThread(); });

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
                backupType.Execute();
            }
            catch (Exception ex)
            {
                this.Statement = Statement.Error;
                AlterJob();
                throw new Exception(ex.Message, ex);
            }
        }

        public void CreateJob()
		{
            this.CreationDate = DateTime.Now;
            this.ModificationDate = DateTime.Now;
            this.Statement = Statement.NoStatement;
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

        public void ChangeStatement()
        {
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
