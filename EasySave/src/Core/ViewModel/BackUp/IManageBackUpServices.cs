using Core.Model;

namespace Core.ViewModel
{
	public interface IManageBackUpServices
    {
        List<BackUpJob> GetAllJobs();
        void CreateJob(string name, string sourcePath, string destintionPath, bool isDifferential, bool encryption);
        Dictionary<string, object> GetJobById(int id);
        void UpdateJob(int Id, Dictionary<string, object> jobdata);
        void DeleteJob(int Id);
    }
}

