using Core.Model.Managers;
using Core.Model.Services;
using Core.ViewModel.Services;
using System.Diagnostics;
using Core.Model.Interfaces;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.ComponentModel.Design;

namespace Core.Model
{
	public class BackUpFull : IBackUpType
	{
		public IJobs job {  get; set; }
		private ILogger _log;
        private ILocalizer _localizer = new Localizer();

        public BackUpFull(BackUpJob job)
		{
			this._log = Logger.Instance;
			this.job = job;
		}
		
		public async Task ExecuteAsync(CancellationToken cancellationToken)
		{
            job.Statement = Statement.Running;
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

                await RunBackupForFiles(GetFichiersPrio(job.dirSource), cancellationToken, maxSizeInBytes);
                await RunBackupForFiles(GetFichiersNonPrio(job.dirSource), cancellationToken, maxSizeInKo);
                
                jobTimer.Stop();
				message = "Job Succeed!";
				WriteStatusLog(jobTimer.ElapsedMilliseconds, message);
			}
			catch (Exception ex)
            {
                jobTimer.Stop();
                message = "Erreur pendant le backup complet : " + ex.Message.ToString();
                WriteStatusLog(jobTimer.ElapsedMilliseconds, message);
                throw new Exception(message, ex);
            }
        }

        public void BackUpFile(string fileSource, string fileTarget)
        {
            Stopwatch watch = Stopwatch.StartNew();
            double encryptionTime = 0;

            if (shouldEncrypt(fileSource))
            {
                encryptionTime = EncryptAndCopy(fileSource, fileTarget);
            }
            else
            {
                File.Copy(fileSource, fileTarget, true);
            }
            watch.Stop();
            double elapsedMs = watch.ElapsedMilliseconds;
            WriteDailyLog(fileSource, fileTarget, elapsedMs, encryptionTime);
        }

        public async Task RunBackupForFiles(IEnumerable<string> lstFichier, CancellationToken cancellationToken, long maxSizeInBytes)
        {
            try
            {
                foreach (string sourceFile in lstFichier)
                {
                    //Verify if we cancel 
                    cancellationToken.ThrowIfCancellationRequested();
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    job.WaitingPause(); // bloque si Reset()

                    job.Progress = $"{sourceFile}        {++job.CurrentFile}/{job.TotalFiles}";

                    string fileTarget = sourceFile.Replace(job.dirSource, job.dirTarget);

                    FileInfo fileInfo = new FileInfo(sourceFile);
                    if (fileInfo.Length > maxSizeInBytes)
                    {
                        await RunJobManager.LargeFileSemaphore.WaitAsync();

                        try
                        {
                            //Thread.Sleep(3000);
                            BackUpFile(sourceFile, fileTarget);
                        }
                        finally
                        {
                            RunJobManager.LargeFileSemaphore.Release();
                        }
                    }
                    else
                        BackUpFile(sourceFile, fileTarget);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    !extensionsFiltrees.Contains(Path.GetExtension(file).TrimStart('.').ToLower()));

            return fichiersFiltres;
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

        public void SetXorKey()
		{
            string xorKey = AppConfigManager.Instance.GetAppConfigParameter("CryptoSoftKey");
            if (xorKey == null | xorKey == "")
            {
                throw new Exception("la clé de Xor de la config est nulle");
            }
            CryptoManager.SetKey(xorKey);
        }

        public void WriteDailyLog(string fileSource, string fileTarget, double transfertTime, double encryptionTime)
        {
            FileInfo fileInfo = new FileInfo(fileSource);
            Dictionary<string, object> logEntry = new Dictionary<string, object>
			{
				{ "FileName", job.Name },
				{ "SourcePath", fileSource },
				{ "TargetPath", fileTarget },
				{ "FileSize", fileInfo.Length },
				{ "FileTransferTime", transfertTime},
				{ "TimeStamp", DateTime.Now.ToString("M/d/yyyy HH:mm:ss") },
				{"encryptionTime", encryptionTime }
			};
			_log.AddLogInfo(LogType.Daily, logEntry);
		}

        public void WriteStatusLog(double jobTimer, string message)
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