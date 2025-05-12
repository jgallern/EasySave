namespace BackUp.Model
{
    public enum BackupType
    {
        Full,
        Differential
    }

    public interface IJobs
    {
		int Id { get; }
		string? Name { get; set; }
		string? FileSource { get; set; }
		string? FileTarget { get; set; }
		BackupType Type { get; set; }



        void Run();
        void CreateJob();
        void DeleteJob();
        void AlterJob();
    }
}