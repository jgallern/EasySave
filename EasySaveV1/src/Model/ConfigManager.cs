namespace Model
{
    public class ConfigManager
    {
        private const int MaxJobs = 5;
        private readonly string filePath;

        public Json(string filePath)
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

        public void UpdateJob(string nom, BackUpJob updatedJob)
        {
            var jobs = LoadJobs();
            var index = jobs.FindIndex(j => j.Nom == nom);
            if (index == -1)
                throw new ArgumentException($"Aucun job trouvé avec le nom '{nom}'.");

            jobs[index] = updatedJob;
            SaveJobs(jobs);
        }

        public void DeleteJob(string nom)
        {
            var jobs = LoadJobs();
            int removed = jobs.RemoveAll(j => j.Nom == nom);
            if (removed == 0)
                throw new ArgumentException($"Aucun job trouvé avec le nom '{nom}'.");

            SaveJobs(jobs);
        }

    }
}