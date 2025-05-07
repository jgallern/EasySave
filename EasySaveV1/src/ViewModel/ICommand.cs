namespace ViewModel
{
	interface ICommand
	{
		void Execute();
		public ICommand CreateBackupJobCommand { get; }
		public ICommand DeleteBackupJobCommand { get; }
		public ICommand ExecuteBackupJobCommand { get; }

	}
}