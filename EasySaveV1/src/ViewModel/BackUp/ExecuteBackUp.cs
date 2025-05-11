namespace BackUp.ViewModel
{
    public class BackupService : IBackupService
    {
        public int MaxJobId { get; } = 5; // ou lu depuis la config

        public void ExecuteAll() { /* … */ }
        public void ExecuteList(IEnumerable<int> jobs) { /* … */ }
        public void ExecuteRange(int start, int end) { /* … */ }
        public void ExecuteSingle(int jobId) { /* … */ }
    }
}