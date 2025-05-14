using Newtonsoft.Json;

namespace BackUp.Model
{

	public class BackUpJob : IJobs
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string FileSource { get; set; }
		public string FileTarget { get; set; }
		public bool Differential{ get; set; }

		public BackUpJob(string Name, string SourceDir, string TargetDir, bool Differential)
		{
			this.Name = Name;
			this.FileSource = SourceDir;
			this.FileTarget= TargetDir;
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
				new BackUpDifferential(Name, FileSource, FileTarget) :
				new BackUpFull(Name, FileSource, FileTarget);

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
			Id = ConfigManager.Instance.FindJobId(this);
			ConfigManager.Instance.UpdateJob(Id,this);	
		}

		public static List<BackUpJob> GetAllJobsFromConfig()
		{
			return ConfigManager.Instance.GetAllJobs();
		}
	}
}
