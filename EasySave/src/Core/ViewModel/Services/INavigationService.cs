using System.Windows.Input;

namespace Core.ViewModel.Services
{
    public interface INavigationService
    {
        void NavigateToMenu();
        void NavigateToSettings();
        void NavigateToBackUp();
        void CloseMenu();
        void CloseSettings();
        void CloseBackUp();
        // etc.
    }

}