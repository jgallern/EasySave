namespace Core.Model.Interfaces
{
    public interface ILocalizer
    {
        string this[string key] { get; }
        void ChangeLanguage(string languageCode);
        string GetCurrentLanguage();
        List<string> GetAvailableLanguages();
        int ChangeMaxFileSize(int maxFileSize);
        int GetMaxFileSize();
        string ChangeEncryptionExtensions(string encryptionExtensions);
        string GetEncryptionExtensions();
        string ChangeSoftwarePackages(string softwarePackages);
        string GetSoftwarePackages();
        string ChangeEncryptionKey(string key);
        string GetEncryptionKey();
        string ChangePriorityFiles(string PriorityFiles);
        string GetPriorityFiles();
    }
}
