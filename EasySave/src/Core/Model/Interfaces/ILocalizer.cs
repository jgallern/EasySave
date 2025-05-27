namespace Core.Model.Interfaces
{
    public interface ILocalizer
    {
        string this[string key] { get; }
        void ChangeLanguage(string languageCode);
        string GetCurrentLanguage();
        List<string> GetAvailableLanguages();
        string ChangeEncryptionExtensions(string encryptionExtensions);
        string GetEncryptionExtensions();
        string ChangeSoftwarePackages(string softwarePackages);
        string GetSoftwarePackages();
        string ChangeEncryptionKey(string key);
        string GetEncryptionKey();
    }
}
