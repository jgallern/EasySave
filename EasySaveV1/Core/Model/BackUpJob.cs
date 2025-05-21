using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Model
{

	public class BackUpJob : IJobs
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string dirSource { get; set; }
		public string dirTarget { get; set; }
		public bool Differential{ get; set; }

        public string? LastError { get; private set; }


        public BackUpJob(string Name, string dirSource, string dirTarget, bool Differential)
		{
            this.Name = Name;
			this.dirSource = dirSource;
			this.dirTarget= dirTarget;
            this.Differential= Differential;
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
