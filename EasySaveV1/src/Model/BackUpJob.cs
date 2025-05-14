using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BackUp.Model
{

	public class BackUpJob : IJobs
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string dirSource { get; set; }
		public string dirTarget { get; set; }
		public bool Differential{ get; set; }

		public BackUpJob(string Name, string FileSource, string FileTarget, bool Differential)
		{
            this.Name = Name;
			this.dirSource = FileSource;
			this.dirTarget= FileTarget;
            this.Differential= Differential;
        }

		public BackUpJob()
		{
			this.Name = "";
			this.FileSource = "";
			this.FileTarget= "";
            this.Differential= false;
		}

        public void Run()
        {
            IBackUpType backupType = Differential ?
				new BackUpDifferential(Name, dirSource, dirTarget) :
				new BackUpFull(Name, dirSource, dirTarget);

            backupType.Execute();
        }

        public void CreateJob()
		{
			Id = ConfigManager.Instance.GetAvailableID();
	        ConfigManager.Instance.AddJob(this);
		}

		public void DeleteJob()
		{
			Id = ConfigManager.Instance.FindJobId(this);
			ConfigManager.Instance.DeleteJob(Id);
		}

		public void AlterJob()
		{
			ConfigManager.Instance.UpdateJob(Id,this);	
		}

		public static List<BackUpJob> GetAllJobsFromConfig()
		{
			return ConfigManager.Instance.GetAllJobs();
		}
		public static BackUpJob GetJobByID(int Id)
		{
			return ConfigManager.Instance.GetJobById(Id);
		}
	}
}
