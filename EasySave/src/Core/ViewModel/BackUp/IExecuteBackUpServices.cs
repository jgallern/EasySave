using System.Windows.Input;

namespace Core.ViewModel
{
    public interface IExecuteBackUpServices
    {
        ICommand ExecuteBackUpCommand { get; }
        string ResultMessage { get; set; }
        string LogPath { get; }
        string JobList { get; }
    }
}