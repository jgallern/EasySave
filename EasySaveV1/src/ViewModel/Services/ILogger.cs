namespace BackUp.ViewModel
{
    public interface IErrorService
    {
        void ShowError(string message);
        void LogError(Exception ex);
    }

}