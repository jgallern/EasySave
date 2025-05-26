using Core.Model.Managers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Diagnostics;


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
            get => _statement;
            set
            {
                if (_statement != value)
                {
                    _statement = value;
                    OnPropertyChanged();
                }
            }
        }


        public string LastFileBackUp { get; set; }

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

        public void Run()
        {
            var runningProcesses = Process.GetProcesses();
            string blockedProcesses = AppConfigManager.Instance.GetAppConfigParameter("SoftwarePackages");
            List<string> blockedProcessesList = blockedProcesses
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .ToList();

            foreach (var proc in runningProcesses){
                if (blockedProcessesList.Contains(proc.ProcessName, StringComparer.OrdinalIgnoreCase))
                {
                    this.Statement = Statement.Canceled;
                    throw new Exception($"Un processus bloquant est actif : {proc.ProcessName}. Exécution du job annulée.");
                }
            }
            try
			{
                this.Statement = Statement.Running;
                IBackUpType backupType = Differential ?
				new BackUpDifferential(Name, dirSource, dirTarget, Encryption) :
				new BackUpFull(Name, dirSource, dirTarget, Encryption);
                backupType.Execute();
                this.Statement = Statement.Done;
            }
			catch (Exception ex)
			{
                this.Statement = Statement.Error;
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
			this.ModificationDate = DateTime.Now;
            this.Statement = Statement.NoStatement;
            JobConfigManager.Instance.UpdateJob(Id,this);	
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
