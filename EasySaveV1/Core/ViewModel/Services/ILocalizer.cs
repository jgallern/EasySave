
namespace Core.ViewModel
{
    public interface ILocalizer
    {
        string this[string key] { get; }
        void ChangeLanguage(string languageCode);
        string GetCurrentLanguage();
        List<string> GetAvailableLanguages();
    }
}
