namespace BackUp.Model
{

    public interface IJobs
    {
		int Id { get; }
		string? Name { get; set; }
		string? FileSource { get; set; }
		string? FileTarget { get; set; }
		bool Differential { get; set; }



        void Run();
        void CreateJob();
        void DeleteJob();
        void AlterJob();
    }
}