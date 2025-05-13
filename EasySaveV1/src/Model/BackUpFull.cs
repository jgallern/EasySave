using System.Diagnostics;

namespace BackUp.Model
{
	public class BackUpFull : IBackUpType
	{
		public string Name { get; }
		public string dirSource { get; }
		public string dirTarget {  get; }
		private ILogger _log;
		public BackUpFull(string Name, string dirSource, string dirTarget)
		{
			this._log = Logger.Instance;
			this.Name = Name;
			this.dirSource = dirSource;
			this.dirTarget = dirTarget;
		}
		
		public void Execute()
		{
			try
			{
				if (!Directory.Exists(this.dirTarget)) 
				{
					Directory.CreateDirectory(this.dirTarget);
				}
				foreach (string dirPath in Directory.GetDirectories(dirSource, "*", SearchOption.AllDirectories))
				{
					Console.WriteLine($"creation de la dir : {dirPath}");
					Directory.CreateDirectory(dirPath.Replace(dirSource, dirTarget));
				}

				foreach (string fileSource in Directory.GetFiles(dirSource, "*.*", SearchOption.AllDirectories))
				{
					Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    string fileTarget = fileSource.Replace(dirSource, dirTarget);
                    File.Copy(fileSource, fileTarget, true);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    ToLogFile(fileSource, fileTarget, elapsedMs);
				}
			}
			catch
			{

            }
        }

        public void ToLogFile(string fileSource, string fileTarget, double transfertTime)
        {
            FileInfo fileInfo = new FileInfo(fileSource);
            Dictionary<string, object> logEntry = new Dictionary<string, object>
			{
                { "FileName", Name },
				{ "SourcePath", fileSource },
			    { "TargetPath", fileTarget },
			    { "FileSize", fileInfo.Length },
			    { "FileTransferTime", transfertTime},
			    { "TimeStamp", DateTime.Now.ToString("M/d/yyyy HH:mm:ss") }
			};
			_log.AddLogInfo(LogType.Daily, logEntry);
		}


		public string GetFileName(string filePath) 
		{
			string fileName = filePath;
			return fileName; 
		}

        public bool IsFile(/*File file*/)
		{
			return true;
		}
	}
}