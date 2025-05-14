namespace BackUp.ViewModel
{
    public interface IExecuteBackUpServices
    {
        ICommand ExecuteBackUpCommand { get; }
        string ResultMessage { get; set; }
        string LogPath { get; }
    }
}