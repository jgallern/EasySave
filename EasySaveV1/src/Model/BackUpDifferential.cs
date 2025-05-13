namespace BackUp.Model
{
	public class BackUpDifferential : IBackUpType
	{
		public string Name { get; }
		public string dirSource{ get; }
		public string dirTarget {  get; }

		public BackUpDifferential(string Name, string FileSource, string FileTarget)
		{
			this.Name = Name;
			this.dirSource = FileSource;
			this.dirTarget = FileTarget;
		}

		public void Execute()
		{
			try
			{

			}
			catch
			{

			}
		}
		public bool FileHadChanged(/*File file*/)
		{
			return true;

		}
		public bool IsFile(/*File file*/)
		{
			return true;
		}
	}
}