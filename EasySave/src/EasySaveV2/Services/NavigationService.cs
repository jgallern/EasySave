using Core.Model.Services;
using Core.ViewModel.Services;
using System.Windows.Input;
using System.Windows;
using EasySaveV2;

namespace Core.ViewModel.Services
{
    public class NavigationService : INavigationService
    {
        public void NavigateToMenu()
        {
            Window menuWindow = new MainWindow();
            menuWindow.Show();

            // Fermer la fenêtre active si besoin
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window is SettingsWindow)
                {
                    window.Close();
                    break;
                }
            }
        }

        public void NavigateToSettings()
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        public void CloseSettings()
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window is SettingsWindow)
                {
                    window.Close();
                    break;
                }
            }
        }

        public void CloseMenu()
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window is MainWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}