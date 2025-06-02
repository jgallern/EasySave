using Core.Model.Managers;
using Core.Model.Services;
using Core.ViewModel.Services;
using System.ComponentModel.Design;
using System.Diagnostics;
using Core.Model.Interfaces;
using System.IO;
using System.Windows;

namespace Core.Model
{
	public class BackUpDifferential : IBackUpType
	{
		public IJobs job { get; set; }
        private ILogger _log;
        private ILocalizer _localizer = new Localizer();



        public BackUpDifferential(BackUpJob job)
		{
            this._log = Logger.Instance;
            this.job = job;
        }

		public async Task ExecuteAsync(CancellationToken cancellationToken)
		{
            job.Statement = Statement.Running;
            job.LastExecution = DateTime.Now;
            job.ChangeStatement();
            Stopwatch jobTimer = Stopwatch.StartNew();
            string message;
            try
            {
                CheckAndCreateDirectories();
                int maxSizeInKo = _localizer.GetMaxFileSize();
                long maxSizeInBytes = maxSizeInKo * 1024;

                job.TotalFiles = Directory.GetFiles(job.dirSource, "*.*", SearchOption.AllDirectories).Count();
                job.CurrentFile = 0;
                job.Progress = $"                {job.CurrentFile}/{job.TotalFiles}";

                job.WaitingPause(); // bloque si Reset()

                // run the backup for prio files, then for non prio
            RunBackupForFiles(GetFichiersPrio(job.dirSource), cancellationToken, maxSizeInBytes);
                RunBackupForFiles(GetFichiersNonPrio(job.dirSource), cancellationToken, maxSizeInBytes);
                jobTimer.Stop();
                message = "Job Succeed!";
                WriteStatusLog(jobTimer.ElapsedMilliseconds, message);
                //Thread.Sleep(3000);
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

        public async void RunBackupForFiles(IEnumerable<string> lstFichier, CancellationToken cancellationToken, long maxSizeInBytes)
        {
            foreach (string sourceFile in lstFichier)
            {
                //Verify if we cancel 
                cancellationToken.ThrowIfCancellationRequested();
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    //Verify if we paused
                    job.WaitingPause(); // bloque si Reset()
                    job.Progress = $"{sourceFile}        {++job.CurrentFile}/{job.TotalFiles}";
                    string targetFile = sourceFile.Replace(job.dirSource, job.dirTarget);

                    if (shouldCopy(targetFile, sourceFile))
                    {
                        FileInfo fileInfo = new FileInfo(sourceFile);
                        if (fileInfo.Length > maxSizeInBytes)
                        {
                            await RunJobManager.LargeFileSemaphore.WaitAsync();

                            try
                            {
                                Thread.Sleep(3000);
                                BackUpFile(sourceFile, targetFile);
                            }
                            finally
                            {
                                RunJobManager.LargeFileSemaphore.Release();
                            }
                        }
                        else
                            BackUpFile(sourceFile, targetFile);
                    }

                }

        }

        public static IEnumerable<string> GetFichiersPrio(string dossierSource)
        {
            string fileExtensionPrio = AppConfigManager.Instance.GetAppConfigParameter("PriorityFiles");
            string[] LstExtensionPrio = fileExtensionPrio.Split(",");


            List<string> extensionsFiltrees = LstExtensionPrio
                .Where((string ext) => !string.IsNullOrWhiteSpace(ext))
                .Select((string ext) => ext.Trim().TrimStart('.').ToLower())
                .ToList();

            IEnumerable<string> fichiersFiltres = Directory
                .GetFiles(dossierSource, "*.*", SearchOption.AllDirectories)
                .Where((string file) =>
                    extensionsFiltrees.Contains(Path.GetExtension(file).TrimStart('.').ToLower()));

            return fichiersFiltres;
        }

        public static IEnumerable<string> GetFichiersNonPrio(string dossierSource)
        {
            string fileExtensionPrio = AppConfigManager.Instance.GetAppConfigParameter("PriorityFiles");
            string[] LstExtensionPrio = fileExtensionPrio.Split(",");


            List<string> extensionsFiltrees = LstExtensionPrio
                .Where((string ext) => !string.IsNullOrWhiteSpace(ext))
                .Select((string ext) => ext.Trim().TrimStart('.').ToLower())
                .ToList();

            if (!extensionsFiltrees.Any())
            {
                return Directory.GetFiles(dossierSource, "*.*", SearchOption.AllDirectories);
            }

            IEnumerable<string> fichiersFiltres = Directory
                .GetFiles(dossierSource, "*.*", SearchOption.AllDirectories)
                .Where((string file) =>
                    extensionsFiltrees.Contains(Path.GetExtension(file).TrimStart('.').ToLower()));

            return fichiersFiltres;
        }


        public void BackUpFile(string sourceFile, string targetFile)
        {
            Stopwatch watch = Stopwatch.StartNew();
            string fileTarget = sourceFile.Replace(job.dirSource, job.dirTarget);
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