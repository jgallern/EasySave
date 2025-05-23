using Core.Model;
using System.Windows.Input;

namespace Core.ViewModel
{
    public class ManageBackUpServices : IManageBackUpServices
    {
        private AppController _app;

        public ICommand ExecuteBackUpCommand { get; }

        public ManageBackUpServices(AppController app)
        {
            _app = app;
        }
        public List<KeyValuePair<int, string>> GetAllJobs()
        {
            List<BackUpJob> jobs = BackUpJob.GetAllJobsFromConfig();
            List<KeyValuePair<int, string>> jobList = new List<KeyValuePair<int, string>>();

            foreach (BackUpJob job in jobs)
            {
                jobList.Add(new KeyValuePair<int, string>(job.Id, job.Name));
            }
            return jobList;
        }
        public void CreateJob(string name, string sourcePath, string destintionPath, bool isDifferential)
        {
            BackUpJob job = new BackUpJob(name, sourcePath, destintionPath, isDifferential);
            try
            {
                job.CreateJob();
            }
            catch (Exception ex)
            {
                new Exception("Erreur lors de la creation du Job");
            }
        }

        public Dictionary<string, object> GetJobById(int id)
        {
            BackUpJob job = BackUpJob.GetJobByID(id);
            return new Dictionary<string, object>
            {
                {"Id", job.Id},
                { "Name", job.Name  },
                { "SourcePath", job.dirSource},
                { "DestinationPath", job.dirTarget},
                { "IsDifferential", job.Differential}
            };
        }

        public void UpdateJob(int Id, Dictionary<string, object> jobdata)
        {
            BackUpJob updatedjob = new BackUpJob((string)jobdata["Name"], (string)jobdata["SourcePath"], (string)jobdata["DestinationPath"], (bool)jobdata["IsDifferential"]);
            updatedjob.Id = Id;
            updatedjob.AlterJob();
        }
        public void DeleteJob(int Id)
        {
            BackUpJob JobToDelete = BackUpJob.GetJobByID(Id);
            JobToDelete.DeleteJob();
        }
    }
}