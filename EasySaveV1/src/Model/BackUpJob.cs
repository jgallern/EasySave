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

		public BackUpJob(string Name, string SourceDir, string TargetDir, bool Differential)
		{
			this.Name = Name;
			this.dirSource = SourceDir;
			this.dirTarget= TargetDir;
            this.Differential= Differential;
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
			Id = ConfigManager.Instance.FindJobId(this);
			ConfigManager.Instance.UpdateJob(Id,this);	
		}

        public static List<BackUpJob> GetAllJobs()
        {
            List<BackUpJob> listJobs = ConfigManager.Instance.GetAllJobs();
			return listJobs;
        }
    }
}
