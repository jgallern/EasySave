using Newtonsoft.Json;
using System;

namespace BackUp.Model
{
    public class ConfigManager
    {
        private const int MaxJobs = 5;
        private readonly string filePath;

        public ConfigManager(string filePath)
        {
            this.filePath = filePath;

            if (!File.Exists(filePath))
                SaveJobs(new List<BackUpJob>());
        }

        private List<BackUpJob> LoadJobs()
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<BackUpJob>>(json) ?? new List<BackUpJob>();
        }

        private void SaveJobs(List<BackUpJob> jobs)
        {
            string json = JsonConvert.SerializeObject(jobs, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public List<BackUpJob> GetAllJobs() => LoadJobs();

        public void AddJob(BackUpJob job)
        {
            List<BackUpJob> jobs = LoadJobs();
            if (jobs.Count >= MaxJobs)
                throw new InvalidOperationException("Le nombre maximum de jobs (5) a été atteint.");
            foreach (IJobs savedjob in jobs)
            {
                if (savedjob.Name  == job.Name | (savedjob.FileSource == job.FileSource && savedjob.FileTarget == job.FileTarget && savedjob.Type == job.Type))
                {
                    throw new Exception("Ce job est déja enregistré dans la config");
                }
            }

            jobs.Add(job);
            SaveJobs(jobs);
        }

        public void UpdateJob(int Id, BackUpJob updatedJob)
        {
            List<BackUpJob> jobs = LoadJobs();
            int index = jobs.FindIndex(j => j.Id == Id);
            if (index == -1)
                throw new ArgumentException($"Aucun job avec L'ID '{Id}'.");

            jobs[index] = updatedJob;
            SaveJobs(jobs);
        }

        public int GetAvailableID()
        {
             List<BackUpJob> jobs = LoadJobs();
            List<int> TakenID = jobs.Select(job => job.Id).ToList();
            int id = 1;
            while (TakenID.Contains(id))
            {
                id++;
            }
            if (id > 6)
            {
                id = 0;
            }
            return id;
        }
        
        public void ReorganiseIndex()
        {
            List<BackUpJob> jobs = LoadJobs();
        }

        public int FindJobId(BackUpJob jobtofind)
        {
            List<BackUpJob> jobs = LoadJobs();
            foreach (BackUpJob job in jobs)
            {
                if (job.Name == jobtofind.Name)
                {
                    return job.Id;
                }
            }
            throw new Exception($"Id non trouvé pour le job nommé {jobtofind.Name}");
        }

        public void DeleteJob(int Id)
        {
            List<BackUpJob> jobs = LoadJobs();
            int removed = jobs.RemoveAll(j => j.Id == Id);
            if (removed == 0)
                throw new ArgumentException($"Aucun job avec L'ID '{Id}'.");

            SaveJobs(jobs);
        }

    }
}