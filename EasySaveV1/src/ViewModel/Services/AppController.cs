using BackUp.ViewModel;
using BackUp.View;

namespace BackUp.ViewModel
{
    public class AppController : IAppController
    {
        public ICommand RunSettingsCommand { get; }
        public ICommand RunExecuteBackupCommand { get; }
        public ICommand RunManageJobsCommand { get; }
        public ICommand ExitCommand { get; }

        private readonly ILocalizer _localizer;

        public AppController(ILocalizer localizer)
        {
            _localizer = localizer;

            RedirectSettingsCommand = new RelayCommand(_ => RunSettings());
            RedirectExecuteBackupCommand = new RelayCommand(_ => RunExecuteBackup());
            RedirectManageJobsCommand = new RelayCommand(_ => RunManageJobs());
            ExitCommand = new RelayCommand(_ => Exit());
        }

        public void RunApp()
        {
            var vm = new MenuViewModel(this); // tu passes this en tant que IRedirectionCommands
            new MenuView(vm).Run();
        }

        // Actions ciblées
        public void RedirectMenu() { new SettingsView(this).Run(); }
        public void RedirectSettings() { ISettingsViewModel vm = new SettingsViewModel(_localizer); new SettingsView(this, vm).Run(); }
        public void RedirectExecuteBackup() { var vm = new ExecuteBackUpServices(); new SettingsView(this, Execute).Run(); }
        public void RedirectManageJobs() { var vm = new ManageBackUpServices(); new SettingsView(this).Run(); }
        public void Exit() => Environment.Exit(0);
    }

    /*
    public class AppController : IAppController
    {

        public void RunApp()
        {
            var vm = new MenuViewModel(this);
            new MenuView(vm).Run();
        }
        public void RunExecuteBackup()
        {
            /*
            var vm = new ExecuteBackup(new BackupService(), _localizer);
            new ExecuteBackUpView(vm).Run();
          
        }
        public void RunSettings()
        {
            var vm = new SettingsViewModel(_localizer);
            new SettingsView(vm).Run();
        }
        public void RunManageJobs()
        {
        }
        public void Exit()
        {
            Environment.Exit(0);
        }
        // ajouter BackUp managment et potentiel autre
    }
    */
}