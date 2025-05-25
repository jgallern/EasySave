using Core.Model;
using Core.ViewModel.Services;
using System.Windows.Input;

namespace Core.ViewModel
{
    public class ManageBackUpServices : IManageBackUpServices
    {
        public ICommand ExecuteBackUpCommand { get; }

        public ManageBackUpServices()
        {
        }
        public List<BackUpJob> GetAllJobs()
        {
            List<BackUpJob> jobs = BackUpJob.GetAllJobsFromConfig();
            
            return jobs;
        }

        public void UpdateJobState(int id, string newState)
        {
            // Change statement
        }

        public void CreateJob(string name, string sourcePath, string destintionPath, bool isDifferential, bool useEncryption)
        {
            BackUpJob job = new BackUpJob(name, sourcePath, destintionPath, isDifferential, useEncryption);
            try
            {
                job.CreateJob();
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la création du Job", ex);
            }
        }

        public Dictionary<string, object> GetJobById(int id)
        {
            try
            {
                BackUpJob job = BackUpJob.GetJobByID(id);
                return new Dictionary<string, object>
                {
                    {"Id", job.Id},
                    { "Name", job.Name  },
                    { "SourcePath", job.dirSource},
                    { "DestinationPath", job.dirTarget},
                    { "IsDifferential", job.Differential},
                    { "Encryption", job.Encryption},
                    { "CreationDate", job.CreationDate},
                    { "ModificationDate", job.ModificationDate},
                    { "Statement", job.Statement}
                };
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"Le job avec l'ID {id} est introuvable.");
            }
        }

        public void UpdateJob(int Id, Dictionary<string, object> jobdata)
        {
            BackUpJob updatedjob = new BackUpJob((string)jobdata["Name"], (string)jobdata["SourcePath"], (string)jobdata["DestinationPath"], (bool)jobdata["IsDifferential"], (bool)jobdata["Encryption"]);
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