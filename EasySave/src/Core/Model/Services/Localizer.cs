using Core.Model.Interfaces;
using Core.Model.Managers;

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
            AppConfigManager.Instance.ChangeAppConfigParameter("CryptoSoftKey", encryptionKey);
            return encryptionKey;
        }
        public string GetEncryptionKey()
        {
            return AppConfigManager.Instance.GetAppConfigParameter("CryptoSoftKey");
        }
    }
}
