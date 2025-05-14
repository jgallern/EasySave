using Newtonsoft.Json;
using System;
using System.Linq;

namespace BackUp.Model
{
    public class ConfigManager
    {
        private static ConfigManager? _instance;
        private static readonly object _lock = new object();

        private const int MaxJobs = 5;
        private readonly string filePath;

        private ConfigManager(string filePath)
        {
            this.filePath = filePath;
            if (!File.Exists(filePath))
                SaveJobs(new List<BackUpJob>());
        }

        public static ConfigManager Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("ConfigManager n'a pas encore été initialisé.");
                return _instance;
            }
        }

        public static void Initialize(string filePath)
        {
            lock (_lock)
            {
                if (_instance == null)
                    _instance = new ConfigManager(filePath);
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
                Console.WriteLine($"Erreur lors du chargement des jobs : {ex.Message}");
                return new List<BackUpJob>();
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
            if (jobs.Count >= MaxJobs)
                throw new InvalidOperationException("Le nombre maximum de jobs (5) a été atteint.");
            foreach (IJobs savedjob in jobs)
            {
                if (savedjob.Name  == job.Name | (savedjob.dirSource == job.dirSource && savedjob.dirTarget == job.dirTarget && savedjob.Differential == job.Differential))
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
        public BackUpJob GetJobById (int Id)
        {
            List<BackUpJob> jobs = LoadJobs();
            return jobs.FirstOrDefault(job => job.Id == Id);
        }
    }
}