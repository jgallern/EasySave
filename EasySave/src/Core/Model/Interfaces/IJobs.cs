namespace Core.Model
{

    public interface IJobs
    {
        bool IsSelected { get; set; }
        int Id { get; }
		string Name { get; set; }
		string dirSource { get; set; }
		string dirTarget { get; set; }
		bool Differential { get; set; }
        bool Encryption { get; set; }
        DateTime CreationDate { get; }
        DateTime ModificationDate { get; }
        Statement Statement { get; set; }
        string Progress { get; set; }
        int CurrentFile { get; set; }
        int TotalFiles { get; set; }

        DateTime LastExecution { get; set; }
        string? LastError { get; }

        Task Run();
        void CreateJob();
        void DeleteJob();
        void AlterJob();
        void Pause();
        void Resume();
        void Stop();
        void Reset();
        void WaitingPause();
        void ChangeStatement();
    }
}