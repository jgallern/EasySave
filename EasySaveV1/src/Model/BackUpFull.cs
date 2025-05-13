using System.Diagnostics;

namespace BackUp.Model
{
	public class BackUpFull : IBackUpType
	{
		public string Name { get; }
		public string FileSource { get; }
		public string FileTarget {  get; }

		public BackUpFull(string Name, string FileSource, string FileTarget)
		{
			this.Name = Name;
			this.FileSource = FileSource;
			this.FileTarget = FileTarget;
		}
		
		public void Execute()
		{
			try
			{
				if (!Directory.Exists(this.FileTarget)) 
				{
					Directory.CreateDirectory(this.FileTarget);
				}
				foreach (string dirPath in Directory.GetDirectories(FileSource, "*", SearchOption.AllDirectories))
				{
					Console.WriteLine($"creation de la dir : {dirPath}");
					Directory.CreateDirectory(dirPath.Replace(FileSource, FileTarget));
				}

				foreach (string newPath in Directory.GetFiles(FileSource, "*.*", SearchOption.AllDirectories))
				{
					var watch = System.Diagnostics.Stopwatch.StartNew();
                    File.Copy(newPath, newPath.Replace(FileSource, FileTarget), true);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    ToLogFile(newPath, elapsedMs);
				}
			}
			catch
			{

            }
        }

        public void ToLogFile(string filePath, string transfertTime)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            Dictionary<string, object> logEntry = new Dictionary<string, object>
			{
			    { "SourcePath", FileSource },
			    { "TargetPath", FileTarget },
			    { "FileName", Name },
			    { "FileSize", fileInfo.Length },
			    { "FileTransferTime", transfertTime},
			    { "TimeStamp", DateTime.Now.ToString("M/d/yyyy HH:mm:ss") }
			};
			//LogMachinBidule.AddFileLog("Daily", logEntry);
		}

        public bool IsFile(/*File file*/)
		{
			return true;
		}
	}
}