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
        string LastFileBackUp { get; set; }
        string? LastError { get; }

        Task Run();
        void CreateJob();
        void DeleteJob();
        void AlterJob();
    }
}