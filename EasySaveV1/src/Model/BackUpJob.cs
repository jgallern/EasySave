namespace BackUp.Model
{
	public class BackUpJob : IJobs
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string FileSource { get; set; }
		public string FileTarget { get; set; }
		public BackupType Type { get; set; }

		public BackUpJob(string Name, string SourceDir, string TargetDir,BackupType Type)
		{
			this.Name = Name;
			this.FileSource = SourceDir;
			this.FileTarget= TargetDir;
			this.Type = Type;
		}

		public void Run()
		{
				
		}

		public void CreateJob(ConfigManager config)
		{
			Id = config.GetAvailableID();
	        config.AddJob(this);
		}

		public void DeleteJob(ConfigManager config)
		{
			config.DeleteJob(Id);
		}

		public void AlterJob(ConfigManager config)
		{
			config.UpdateJob(Id,this);	
		}
	}
}
