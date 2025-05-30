using Core.Model.Interfaces;
using Core.Model.Managers;
using System.Globalization;

namespace Core.Model.Services
{
    public class Localizer : ILocalizer
    {
        // ------------------ Translation methods ----------------------------- 
        public string this[string key] => ResourceManager.Instance.Translate(key);

        public void ChangeLanguage(string languageCode)
        {
            AppConfigManager.Instance.ChangeAppConfigParameter("Language", languageCode);
            ResourceManager.Instance.LoadTranslations(languageCode);
        }

        public string GetCurrentLanguage()
        {
            return AppConfigManager.Instance.GetAppConfigParameter("Language");
        }

        public List<string> GetAvailableLanguages()
        {
            return ResourceManager.Instance.GetAvailableLanguages();
        }


        public int ChangeMaxFileSize(int maxFileSize)
        {
            string fileSize = maxFileSize.ToString();
            AppConfigManager.Instance.ChangeAppConfigParameter("MaxFileSize", fileSize);
            return maxFileSize;
        }
        public int GetMaxFileSize()
        {
            return int.Parse(AppConfigManager.Instance.GetAppConfigParameter("MaxFileSize"));
        }

        // ----------------------------- Priority Files methods ---------------------------
        public string ChangePriorityFiles(string priorityFiles)
        {
            List<string> priorityFilesList = priorityFiles.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim()).Where(e => !string.IsNullOrWhiteSpace(e)).ToList();
            string files = string.Join(", ", priorityFilesList);
            AppConfigManager.Instance.ChangeAppConfigParameter("PriorityFiles", files);
            return files;
        }

        public string GetPriorityFiles()
        {
            return AppConfigManager.Instance.GetAppConfigParameter("PriorityFiles");
        }


        // ----------------------------- Encryption extensions methods ---------------------------
        public string ChangeEncryptionExtensions(string encyptionExtensions)
        {
            List<string> encryptionExtensionsList = encyptionExtensions.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim()).Where(e => !string.IsNullOrWhiteSpace(e)).ToList();
            string extensions = string.Join(", ", encryptionExtensionsList);
            AppConfigManager.Instance.ChangeAppConfigParameter("EncryptionExtensions", extensions);
            return extensions;
        }

        public string GetEncryptionExtensions()
        {
            return AppConfigManager.Instance.GetAppConfigParameter("EncryptionExtensions");
        }


        // ----------------------------- Software packages methods ---------------------------
        public string ChangeSoftwarePackages(string softwarePackages)
        {
            List<string> softwarePackagesList = softwarePackages.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim()).Where(e => !string.IsNullOrWhiteSpace(e)).ToList();
            string softwares = string.Join(", ", softwarePackagesList);
            AppConfigManager.Instance.ChangeAppConfigParameter("SoftwarePackages", softwares);
            return softwares;
        }
        public string GetSoftwarePackages()
        {
            return AppConfigManager.Instance.GetAppConfigParameter("SoftwarePackages");
        }

        // ----------------------------- Encryption Key methods ---------------------------
        public string ChangeEncryptionKey(string encryptionKey)
        {
            CryptoManager.SetKey(encryptionKey);
            return CryptoManager.GetKeyString();
        }
        public string GetEncryptionKey()
        {
            return CryptoManager.GetKeyString();
        }
    }
}
