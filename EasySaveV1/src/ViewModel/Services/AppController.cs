using BackUp.ViewModel;
using BackUp.View;
using BackUp.Model;

namespace BackUp.ViewModel
{
    public class AppController : IAppController
    {
        public ICommand RedirectMenuCommand { get; }
        public ICommand RedirectSettingsCommand { get; }
        public ICommand RedirectExecuteBackupCommand { get; }
        public ICommand RedirectManageBackupsCommand { get; }
        public ICommand ExitCommand { get; }

        private readonly ILocalizer _localizer;

        public AppController(ILocalizer localizer)
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
            new MenuView(this).Run(); 
        }

        private void RedirectSettings()
        {
            ISettingsViewModel vm = new SettingsViewModel(_localizer);
            new SettingsView(this, vm).Run();
        }
        public void RunSettings()
        {
            var vm = new SettingsViewModel(_localizer);
            new SettingsView(vm).Run();
        }
        public void RunManageJobs()
        {
            var vm = new ManageBackUp();
            new ManageBackUpView(vm).Run(); 
        }

        private void RedirectExecuteBackup()
        {
            var vm = new ExecuteBackUpServices(this);
            new ExecuteBackUpView(this, vm).Run();
        }

        private void RedirectManageBackups() { /*var vm = new ManageBackUpServices(); new SettingsView(this, vm).Run();*/ }
        private void Exit() => Environment.Exit(0);

        public string GetCurrentLanguage() => _localizer.GetCurrentLanguage();
        public List<string> GetAvailableLanguages() => _localizer.GetAvailableLanguages();
        public string Translate(string key) => _localizer[key];

    }
}