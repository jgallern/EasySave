namespace Core.Model
{

    public interface IJobs
    {
		int Id { get; }
		string Name { get; set; }
		string dirSource { get; set; }
		string dirTarget { get; set; }
		bool Differential { get; set; }

        void Run();
        void CreateJob();
        void DeleteJob();
        void AlterJob();
    }
}