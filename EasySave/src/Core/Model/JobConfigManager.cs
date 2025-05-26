using Newtonsoft.Json;
using System;
using System.Linq;
using System.IO;

namespace Core.Model
{
    public class JobConfigManager
    {
        private static JobConfigManager? _instance;
        private static readonly object _lock = new object();

        private readonly string filePath;

        private JobConfigManager()
        {
            filePath = Path.Join(AppContext.BaseDirectory, Path.Join("env", "jobconfig.json"));
            string? dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static JobConfigManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new JobConfigManager();
                    }
                }
                return _instance;
            }
        }

        private List<BackUpJob> LoadJobs()
        {
            try
            {
                string json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json)) return new List<BackUpJob>();
                List<BackUpJob> jobs = JsonConvert.DeserializeObject<List<BackUpJob>>(json) ?? new List<BackUpJob>();
                return jobs;
            }
            catch (Exception ex)
            {
                throw new FileLoadException($"Erreur lors du chargement des jobs : {ex.Message}");
                //return new List<BackUpJob>();
            }
        }


        private void SaveJobs(List<BackUpJob> jobs)
        {
            string json = JsonConvert.SerializeObject(jobs, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public List<BackUpJob> GetAllJobs() => LoadJobs();

        public BackUpJob GetJobById(int Id)
        {
            List<BackUpJob> jobs = LoadJobs();
            return jobs.FirstOrDefault(job => job.Id == Id);
        }

        public void AddJob(BackUpJob job)
        {
            List<BackUpJob> jobs = LoadJobs();
            foreach (IJobs savedjob in jobs)
            {
                if (savedjob.Name  == job.Name | (savedjob.dirSource == job.dirSource && savedjob.dirTarget == job.dirTarget && savedjob.Differential == job.Differential && savedjob.Encryption == job.Encryption))
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
            return id;
        }

        public void ReorganiseIndex()
        {
            List<BackUpJob> jobs = LoadJobs();
            int AvailableID = GetAvailableID();
            if (jobs.Count() >= AvailableID)
            {
                for (int i = 0; i < jobs.Count(); i++)
                {
                    jobs[i].Id = i + 1;
                }
                SaveJobs(jobs);
            }
            else
            {
                return;
            }
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
            ReorganiseIndex(); 
        }
    }
}