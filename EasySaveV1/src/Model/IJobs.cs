namespace BackUp.Model
{
    public enum BackupType
    {
        Full,
        Differential
    }

    public interface IJobs
    {
		int Id { get; set; }
		string Name { get; set; }
		string FileSource { get; set; }
		string FileTarget { get; set; }
		BackupType Type { get; set; }



        void Run();
        void CreateJob(ConfigManager config);
        void DeleteJob(ConfigManager config);
        void AlterJob(ConfigManager config);
    }
}