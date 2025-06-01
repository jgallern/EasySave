using Core.Model.Services;
using Core.ViewModel.Services;
using System.Windows.Input;
using System.Windows;
using EasySaveV3;
using Core.Model;

namespace Core.ViewModel.Services
{
    public class NavigationService : INavigationService
    {
        public void NavigateToMenu()
        {
            Window menuWindow = new MainWindow();
            menuWindow.Show();
        }

        public void NavigateToSettings()
        {
            Window settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        public void NavigateToBackUp(BackUpViewModel vm)
        {
            Window backupWindow = new BackUpWindow(vm);
            backupWindow.Show();
        }

        public void NavigateToMonitoring(MonitoringViewModel vm)
        {
            Window monitoringWindow = new MonitoringWindow(vm);
            monitoringWindow.Show();
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

        public void CloseBackUp()
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window is BackUpWindow)
                {
                    window.Close();
                    break;
                }
            }
        }

        public void CloseMonitoring()
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window is MonitoringWindow)
                {
                    window.Close();
                    break;
                }
            }
        }

    }
}