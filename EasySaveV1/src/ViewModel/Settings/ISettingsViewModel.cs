namespace BackUp.ViewModel
{
    public interface ISettingsViewModel
    {
        IReadOnlyList<string> AvailableLanguages { get; }
        string CurrentLanguage { get; }
        ICommand ChangeLanguageCommand { get; }
        string this[string key] { get; }
        event Action LanguageChanged;
    }
}