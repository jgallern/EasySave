using System.Windows.Input;

namespace Core.ViewModel.Services
{
    public interface INavigationService
    {
        void NavigateToMenu();
        void NavigateToSettings();
        void NavigateToBackUp(BackUpViewModel vm);
        void CloseMenu();
        void CloseSettings();
        void CloseBackUp();
        // etc.
    }

}