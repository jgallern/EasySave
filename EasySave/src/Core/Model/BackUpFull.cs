using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
using Core.Model.Managers;
using Core.Model.Services;

namespace Core.Model
{
	public class BackUpFull : IBackUpType
	{
		public IJobs job {  get; set; }
		private ILogger _log;

		public BackUpFull(BackUpJob job)
		{
			this._log = Logger.Instance;
			this.job = job;
		}
		
		public void Execute()
		{
            job.Statement = Statement.Running;
            job.ChangeStatement();
            Stopwatch jobTimer = Stopwatch.StartNew();
            string message;
			try
			{
				SetXorKey();
				CheckAndCreateDirectories();
                // Copie all the files to the target dir
                RunJobManager.PauseEvent.Wait(); // bloque si Reset()


                foreach (string fileSource in Directory.GetFiles(job.dirSource, "*.*", SearchOption.AllDirectories))
				{
                    RunJobManager.PauseEvent.Wait(); // bloque si Reset()

                    Stopwatch watch = Stopwatch.StartNew();
					string fileTarget = fileSource.Replace(job.dirSource, job.dirTarget);
                    double encryptionTime = 0;

                    if (shouldEncrypt(fileSource))
					{
                        encryptionTime = EncryptAndCopy(fileSource, fileTarget);
                    }
                    else {
						File.Copy(fileSource, fileTarget, true);
					}
					watch.Stop();
					double elapsedMs = watch.ElapsedMilliseconds;
                    WriteDailyLog(fileSource, fileTarget, elapsedMs, encryptionTime);
                }
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