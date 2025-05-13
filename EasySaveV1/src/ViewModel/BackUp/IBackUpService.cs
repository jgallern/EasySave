namespace BackUp.ViewModel
{
	public interface IBackupService
	{
		void ExecuteAll();
		void ExecuteList(IEnumerable<int> jobs);
		void ExecuteRange(int start, int end);
		void ExecuteSingle(int jobId);
		int MaxJobId { get; }
	}
}

