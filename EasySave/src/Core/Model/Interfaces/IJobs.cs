namespace Core.Model
{

    public interface IJobs
    {
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

        void Run();
        void CreateJob();
        void DeleteJob();
        void AlterJob();
    }
}