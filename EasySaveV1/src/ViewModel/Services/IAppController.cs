namespace BackUp.ViewModel
{
    public interface IAppController
    {
        ICommand RedirectMenuCommand { get; }
        ICommand RedirectSettingsCommand { get; }
        ICommand RedirectExecuteBackupCommand { get; }
        ICommand RedirectManageBackupsCommand { get; }
        ICommand ExitCommand { get; }

        string GetCurrentLanguage();
        List<string> GetAvailableLanguages();
        string Translate(string key);
    }

}