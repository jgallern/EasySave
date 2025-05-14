using System.Diagnostics;

namespace BackUp.Model
{
	public class BackUpFull : IBackUpType
	{
		public string Name { get; }
		public string dirSource { get; }
		public string dirTarget {  get; }
		private ILogger _log;
		public BackUpFull(string Name, string FileSource, string FileTarget)
		{
			this._log = Logger.Instance;
			this.Name = Name;
			this.dirSource = FileSource;
			this.dirTarget = FileTarget;
		}
		
		public void Execute()
		{
            Stopwatch jobTimer = Stopwatch.StartNew();
            string message;
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
                    double elapsedMs = watch.ElapsedMilliseconds;
                    WriteDailyLog(fileSource, fileTarget, elapsedMs);
				}
                jobTimer.Stop();
                message = "Job Succeed!";
            }
			catch (Exception ex)
            {
                jobTimer.Stop();
                Console.WriteLine("Erreur pendant le backup complet : " + ex.Message);
                message = "Erreur pendant le backup complet : " + ex.Message.ToString();
            }
            WriteStatusLog(jobTimer.ElapsedMilliseconds, message);
        }

        public void WriteDailyLog(string fileSource, string fileTarget, double transfertTime)
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

        public void WriteStatusLog(double jobTimer, string message)
        {
            Dictionary<string, object> logJob = new Dictionary<string, object>
                {
                    { "Name", Name },
                    { "JobTime", jobTimer},
                    { "Result", message },
                    { "TimeStamp", DateTime.Now.ToString("M/d/yyyy HH:mm:ss") }
                };
            _log.AddLogInfo(LogType.Status, logJob);
        }
    }
}