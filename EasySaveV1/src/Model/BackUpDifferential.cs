namespace Model
{
	public class BackUpDifferential
	{
		string Name { get; }
		string FileSource{ get; }
		string FileTarget {  get; }

		public BackUpFull(string Name, string FileSource, string FileTarget)
		{
			this.Name = Name;
			this.FileSource = FileSource;
			this.FileTarget = FileTarget;
		}

		public bool execute()
		{
			try
			{

			}
			catch
			{

			}
		}
		public bool FileHadChanged(File file)
		{

		}
		public bool IsFile(File file)
		{
			if file 
		}
	}
}