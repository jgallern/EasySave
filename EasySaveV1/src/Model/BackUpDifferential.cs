using System.Diagnostics;

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
                if (!Directory.Exists(this.FileTarget))
                {
                    Directory.CreateDirectory(this.FileTarget);
                }

                // Crée les dossiers manquants dans la cible
                foreach (string dirPath in Directory.GetDirectories(FileSource, "*", SearchOption.AllDirectories))
                {
                    string targetDirPath = dirPath.Replace(FileSource, FileTarget);
                    if (!Directory.Exists(targetDirPath))
                    {
                        Directory.CreateDirectory(targetDirPath);
                    }
                }

                // Compare et copie les fichiers modifiés ou nouveaux
                foreach (string sourceFilePath in Directory.GetFiles(FileSource, "*.*", SearchOption.AllDirectories))
                {
                    string targetFilePath = sourceFilePath.Replace(FileSource, FileTarget);

                    bool shouldCopy = false;

                    if (!File.Exists(targetFilePath))
                    {
                        shouldCopy = true; // Nouveau fichier
                    }
                    else
                    {
                        DateTime sourceLastWrite = File.GetLastWriteTimeUtc(sourceFilePath);
                        DateTime targetLastWrite = File.GetLastWriteTimeUtc(targetFilePath);

                        if (sourceLastWrite > targetLastWrite)
                        {
                            shouldCopy = true; // Modifié
                        }
                    }

                    if (shouldCopy)
                    {
                        Stopwatch watch = Stopwatch.StartNew();
                        Console.WriteLine("ajout");
                        File.Copy(sourceFilePath, targetFilePath, true);
                        watch.Stop();
                        //ToLogFile(sourceFilePath, watch.ElapsedMilliseconds);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur pendant le backup différentiel : " + ex.Message);
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
    }
}