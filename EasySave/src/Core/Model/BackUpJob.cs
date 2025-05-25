using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Model
{
    public enum Statement
    {
        Running,
        Paused,
		Stoped,
    }
    public class BackUpJob : IJobs
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string dirSource { get; set; }
		public string dirTarget { get; set; }
		public bool Differential{ get; set; }
        public bool Encryption { get; set; }
        public DateTime CreationDate { get; private set; }
        public DateTime ModificationDate { get; private set; }
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
				new BackUpDifferential(Name, dirSource, dirTarget) :
				new BackUpFull(Name, dirSource, dirTarget);

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
            this.Statement = Statement.Stoped;
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
