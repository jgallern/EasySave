using System.Diagnostics;
using Core.Model.Managers;
using System.IO;
using Core.Model.Services;
using System.ComponentModel.Design;

namespace Core.Model
{
	public class BackUpDifferential : IBackUpType
	{
		public IJobs job { get; set; }
        private ILogger _log;

        public BackUpDifferential(BackUpJob job)
		{
            this._log = Logger.Instance;
            this.job = job;
		}

		public void Execute()
		{
            job.Statement = Statement.Running;
            job.LastExecution = DateTime.Now;
            job.AlterJob();
            Stopwatch jobTimer = Stopwatch.StartNew();
            string message;
            try
            {
                CheckAndCreateDirectories();

                var test = Directory.GetFiles(job.dirSource, "*.*", SearchOption.AllDirectories);
                // Compare et copie les fichiers modifiés ou nouveaux
                foreach (string sourceFile in Directory.GetFiles(job.dirSource, "*.*", SearchOption.AllDirectories))
                {
                    string targetFile = sourceFile.Replace(job.dirSource, job.dirTarget);

                    if (shouldCopy(targetFile, sourceFile))
                    {
                        Stopwatch watch = Stopwatch.StartNew();
                        string fileTarget = sourceFile.Replace(job.dirSource, job.dirTarget);
                        string fileExtensionsToEncrypty = AppConfigManager.Instance.GetAppConfigParameter("EncryptionExtensions");
                        String[] LstFileExtensionsToEncrypt = fileExtensionsToEncrypty.Split(",");
                        double encryptionTime = 0;

                        if (shouldEncrypt(sourceFile))
                        {
                            encryptionTime = EncryptAndCopy(sourceFile, targetFile);
                        }
                        else
                        {
                            File.Copy(sourceFile, fileTarget, true);
                        }
                        watch.Stop();
                        double elapsedMs = watch.ElapsedMilliseconds;
                        try
                        {
                            WriteDailyLog(sourceFile, fileTarget, elapsedMs, encryptionTime);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }

                }
                jobTimer.Stop();
                message = "Job Succeed!";
                WriteStatusLog(jobTimer.ElapsedMilliseconds, message);
                Thread.Sleep(3000);
                job.Statement = Statement.Done;
                job.AlterJob();
            }
            catch (Exception ex)
            {
                jobTimer.Stop();
                message = "Erreur pendant le backup différentiel : " + ex.Message.ToString();
                WriteStatusLog(jobTimer.ElapsedMilliseconds, message);
                throw new Exception(message, ex);
            }

        }

        public double EncryptAndCopy(string sourceFile, string fileTarget)
        {
            Stopwatch EncryptTimer = Stopwatch.StartNew();

            try
            {
                fileTarget += ".xor";
                CryptoManager.EncryptFileToTarget(sourceFile, fileTarget);
            }
            catch 
            {
                return -1;
            }
            EncryptTimer.Stop();
            return EncryptTimer.Elapsed.Milliseconds;
        }

        public bool shouldCopy(string targetFile, string sourceFile)
        {
            bool shouldCopy = false;

            if ((!job.Encryption && !File.Exists(targetFile)) | (job.Encryption && !File.Exists(targetFile+".xor")))
            {
                shouldCopy = true; // Nouveau fichier
            }
            else
            {
                DateTime sourceLastWrite, targetLastWrite;
                if (!job.Encryption)
                {
                    sourceLastWrite = File.GetLastWriteTimeUtc(sourceFile);
                    targetLastWrite = File.GetLastWriteTimeUtc(targetFile);
                }
                else
                {
                    sourceLastWrite = File.GetLastWriteTimeUtc(sourceFile+".xor");
                    targetLastWrite = File.GetLastWriteTimeUtc(targetFile);
                }

                if (sourceLastWrite > targetLastWrite)
                {
                    shouldCopy = true; // Modifié
                }
            }
            return shouldCopy;
        }
        public bool shouldEncrypt(string fileSource)
        {
            string fileExtensionsToEncrypt = AppConfigManager.Instance.GetAppConfigParameter("EncryptionExtensions");
            String[] LstFileExtensionsToEncrypt = fileExtensionsToEncrypt.Split(",");

            bool shouldEncrypt = false;
            if (job.Encryption)
            {
                shouldEncrypt = LstFileExtensionsToEncrypt.Any(ext => fileSource.EndsWith(ext.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            return shouldEncrypt;
        }

        public void CheckAndCreateDirectories()
        {
            // verifie if the subdirectories exists and create them if necessary
            if (!Directory.Exists(job.dirTarget))
            {
                Directory.CreateDirectory(job.dirTarget);
            }
            foreach (string dirPath in Directory.GetDirectories(job.dirSource, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(job.dirSource, job.dirTarget));
            }
        }

        private void WriteDailyLog(string sourceFile, string targetFile, double transfertTime, double encryptionTime)
        {
            FileInfo fileInfo = new FileInfo(sourceFile);
            Dictionary<string, object> logEntry = new Dictionary<string, object>
            {
                { "Name", job.Name },
                { "SourcePath", sourceFile },
                { "TargetPath", targetFile },
                { "FileSize", fileInfo.Length },
                { "FileTransferTime", transfertTime},
                { "TimeStamp", DateTime.Now.ToString("M/d/yyyy HH:mm:ss") },
                {"encryptionTime", encryptionTime }
            };
            _log.AddLogInfo(LogType.Daily, logEntry);
        }

        private void WriteStatusLog(double jobTimer, string message)
        {
            Dictionary<string, object> logJob = new Dictionary<string, object>
                {
                    { "Name", job.Name },
                    { "JobTime", jobTimer},
                    { "Result", message },
                    { "TimeStamp", DateTime.Now.ToString("M/d/yyyy HH:mm:ss") }
                };
            _log.AddLogInfo(LogType.Status, logJob);
        }
    }
}