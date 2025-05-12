namespace BackUp.Model
{

	public class BackUpJob : IJobs
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string FileSource { get; set; }
		public string FileTarget { get; set; }
		public BackupType Type { get; set; }
		public ConfigManager config {  get; set; }

		public BackUpJob(string Name, string SourceDir, string TargetDir,BackupType Type, ConfigManager config)
		{
			this.Name = Name;
			this.FileSource = SourceDir;
			this.FileTarget= TargetDir;
			this.Type = Type;
			this.config = config;

			InitializeId();
        }

		public void InitializeId()
		{
            try
            {
				Id = config.FindJobId(this);
			}
			catch
			{
				Id = config.GetAvailableID();
			}
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
			Id = config.FindJobId(this);
			config.DeleteJob(Id);
		}

		public void AlterJob(ConfigManager config)
		{
			config.UpdateJob(Id,this);	
		}
	}
}
