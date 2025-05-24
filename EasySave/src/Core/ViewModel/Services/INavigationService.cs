using System.Windows.Input;

namespace Core.ViewModel.Services
{
    public interface INavigationService
    {
        void NavigateToMenu();
        void NavigateToSettings();
        void CloseMenu();
        void CloseSettings();
        // etc.
    }

}