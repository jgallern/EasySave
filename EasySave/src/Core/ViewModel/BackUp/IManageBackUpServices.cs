namespace Core.ViewModel
{
	public interface IManageBackUpServices
    {
        List<KeyValuePair<int, string>> GetAllJobs();
        void CreateJob(string name, string sourcePath, string destintionPath, bool isDifferential);
        Dictionary<string, object> GetJobById(int id);
        void UpdateJob(int Id, Dictionary<string, object> jobdata);
        void DeleteJob(int Id);
    }
}

