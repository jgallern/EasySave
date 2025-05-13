namespace Model
{
	public class BackUpFull
	{
		string Name { get; }
		string FileSource { get; }
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
				foreach (string dirPath in Directory.GetDirectories(FileSource, "*", SearchOption.AllDirectories))
				{
					Directory.CreateDirectory(dirPath.Replace(FileSource, FileTarget));
				}

				//Copy all the files & Replaces any files with the same name
				foreach (string newPath in Directory.GetFiles(FileSource, "*.*", SearchOption.AllDirectories))
				{
					File.Copy(newPath, newPath.Replace(FileSource, FileTarget), true);
				}
			}
			catch
			{

			}
		}

		public bool IsFile(File file)
		{
		}
	}
}