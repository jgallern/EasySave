namespace BackUp.Model
{
	public class BackUpDifferential : IBackUpType
	{
		public string Name { get; }
		public string FileSource{ get; }
		public string FileTarget {  get; }

		public BackUpDifferential(string Name, string FileSource, string FileTarget)
		{
			this.Name = Name;
			this.FileSource = FileSource;
			this.FileTarget = FileTarget;
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