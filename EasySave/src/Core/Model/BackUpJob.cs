using Core.Model.Managers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;


namespace Core.Model
{
    public enum Statement
    {
        Running,
        Paused,
		Canceled,
		Error,
        Done,
        NoStatement,
    }
    public class BackUpJob : IJobs, INotifyPropertyChanged
    {

        private bool _isSelected;

        public bool isSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(isSelected));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public int Id { get; set; }
        public string Name { get; set; }

		public string dirSource { get; set; }

        public string dirTarget { get; set; }

		public bool Differential{ get; set; }

        public bool Encryption { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public Statement Statement { get; set; }

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
			try
			{
				IBackUpType backupType = Differential ?
				new BackUpDifferential(Name, dirSource, dirTarget, Encryption) :
				new BackUpFull(Name, dirSource, dirTarget, Encryption);

				backupType.Execute();
			}
			catch (Exception ex)
			{
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
