namespace Model
{
    public enum BackupType
    {
        Full,
        Differential
    }

    public interface IJobs
    {
        string Name { get; set; }
        string SourceDir { get; set; }
        string TargetDir { get; set; }
        BackupType Type { get; set; }

        void Run();
        void CreateJob();
        void DeleteJob();
        void AlterJob();
    }
}