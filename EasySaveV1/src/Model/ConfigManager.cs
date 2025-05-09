using Newtonsoft.Json;

namespace Model
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
            var jobs = LoadJobs();
            if (jobs.Count >= MaxJobs)
                throw new InvalidOperationException("Le nombre maximum de jobs (5) a été atteint.");

            jobs.Add(job);
            SaveJobs(jobs);
        }

        public void UpdateJob(int Id, BackUpJob updatedJob)
        {
            var jobs = LoadJobs();
            var index = jobs.FindIndex(j => j.Id == Id);
            if (index == -1)
                throw new ArgumentException($"Aucun job avec L'ID '{Id}'.");

            jobs[index] = updatedJob;
            SaveJobs(jobs);
        }

        public int GetAvailableID()
        {
            var jobs = LoadJobs();
            List<int> TakenID = jobs.Select(job => job.ID).ToList();
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
            
        }

        public void DeleteJob(int Id)
        {
            var jobs = LoadJobs();
            int removed = jobs.RemoveAll(j => j.Id == Id);
            if (removed == 0)
                throw new ArgumentException($"Aucun job avec L'ID '{Id}'.");

            SaveJobs(jobs);
        }

    }
}