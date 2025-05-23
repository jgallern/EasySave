using System.Windows.Input;

namespace Core.ViewModel
{
    public interface ISettingsCommands
    {
        IReadOnlyList<string> AvailableLanguages { get; }
        string CurrentLanguage { get; }
        ICommand ChangeLanguageCommand { get; }
        string this[string key] { get; }
    }
}