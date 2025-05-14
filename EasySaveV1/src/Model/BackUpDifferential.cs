using System.Diagnostics;

namespace BackUp.Model
{
	public class BackUpDifferential : IBackUpType
	{
		public string Name { get; }
		public string dirSource{ get; }
		public string dirTarget {  get; }
        private ILogger _log;

        public BackUpDifferential(string Name, string dirSource, string dirTarget)
		{
            this._log = Logger.Instance;
            this.Name = Name;
			this.dirSource = dirSource;
			this.dirTarget = dirTarget;
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

                // Crée les dossiers manquants dans la cible
                foreach (string dirPath in Directory.GetDirectories(dirSource, "*", SearchOption.AllDirectories))
                {
                    string targetDirPath = dirPath.Replace(dirSource, dirTarget);
                    if (!Directory.Exists(targetDirPath))
                    {
                        Directory.CreateDirectory(targetDirPath);
                    }
                }

                // Compare et copie les fichiers modifiés ou nouveaux
                foreach (string sourceFile in Directory.GetFiles(dirSource, "*.*", SearchOption.AllDirectories))
                {
                    string targetFile = sourceFile.Replace(dirSource, dirTarget);

                    bool shouldCopy = false;

                    if (!File.Exists(targetFile))
                    {
                        shouldCopy = true; // Nouveau fichier
                    }
                    else
                    {
                        DateTime sourceLastWrite = File.GetLastWriteTimeUtc(sourceFile);
                        DateTime targetLastWrite = File.GetLastWriteTimeUtc(targetFile);

                        if (sourceLastWrite > targetLastWrite)
                        {
                            shouldCopy = true; // Modifié
                        }
                    }

                    if (shouldCopy)
                    {
                        Stopwatch watch = Stopwatch.StartNew();
                        File.Copy(sourceFile, targetFile, true);
                        watch.Stop();
                        WriteDailyLog(sourceFile, targetFile, watch.ElapsedMilliseconds.ToString());
                    }
                }
                jobTimer.Stop();
                message = "Job Succeed!";
            }
            catch (Exception ex)
            {
                jobTimer.Stop();
                Console.WriteLine("Erreur pendant le backup différentiel : " + ex.Message);
                message = "Erreur pendant le backup différentiel : " + ex.Message.ToString();
            }
            WriteStatusLog(jobTimer.ElapsedMilliseconds, message);
        }

        private void WriteDailyLog(string sourceFile, string targetFile, string transfertTime)
        {
            FileInfo fileInfo = new FileInfo(sourceFile);
            Dictionary<string, object> logEntry = new Dictionary<string, object>
            {
                { "Name", Name },
                { "SourcePath", sourceFile },
                { "TargetPath", targetFile },
                { "FileSize", fileInfo.Length },
                { "FileTransferTime", transfertTime},
                { "TimeStamp", DateTime.Now.ToString("M/d/yyyy HH:mm:ss") }
            };
            _log.AddLogInfo(LogType.Daily, logEntry);
        }

        private void WriteStatusLog(double jobTimer, string message)
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