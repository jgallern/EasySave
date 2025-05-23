using System.Windows.Input;

namespace Core.ViewModel
{
    public interface ISettingsCommands
    {
        IReadOnlyList<string> AvailableLanguages { get; }
        string CurrentLanguage { get; set; }
        string this[string key] { get; }
    }
}