using System.Windows.Input;

namespace Core.ViewModel.Commands
{
    public interface IAppRedirectionCommands
    {
        ICommand RedirectMenuCommand { get; }
        ICommand RedirectSettingsCommand { get; }
        ICommand RedirectExecuteBackupCommand { get; }
        ICommand RedirectManageBackupsCommand { get; }
        ICommand ExitCommand { get; }
    }

}