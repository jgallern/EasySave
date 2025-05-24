using Core.Model.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Core.ViewModel.Commands
{
    public class AppRedirectionCommands : IAppRedirectionCommands
    {
        public ICommand RedirectMenuCommand { get; }
        public ICommand RedirectSettingsCommand { get; }
        public ICommand RedirectExecuteBackupCommand { get; }
        public ICommand RedirectManageBackupsCommand { get; }
        public ICommand ExitCommand { get; }

        private readonly ILocalizer _localizer;

        public AppRedirectionCommands(ILocalizer localizer)
        {
            _localizer = localizer;

            RedirectMenuCommand = new RelayCommand(_ => RedirectMenu());
            RedirectSettingsCommand = new RelayCommand(_ => RedirectSettings());
            RedirectExecuteBackupCommand = new RelayCommand(_ => RedirectExecuteBackup());
            RedirectManageBackupsCommand = new RelayCommand(_ => RedirectManageBackups());
            ExitCommand = new RelayCommand(_ => Exit());
        }

        private void RedirectMenu()
        {
            //new MenuView(this).Run(); 
        }

        private void RedirectSettings()
        {
            IAppRedirectionCommands vm = new AppRedirectionCommands(_localizer);
            //new SettingsView(this, vm).Run();
        }
        public void RedirectManageBackups()
        {
            //IManageBackUpServices vm = new ManageBackUpServices(this);
            //new ManageBackUpView(this, vm).Run(); 
        }

        private void RedirectExecuteBackup()
        {
            //var vm = new ExecuteBackUpServices(this);
            //new ExecuteBackUpView(this, vm).Run();
        }

        private void Exit() => Environment.Exit(0);

    }
}