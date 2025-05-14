using BackUp.ViewModel;
using BackUp.View;

namespace BackUp.ViewModel
{
    public class AppController : IAppController
    {
        private readonly ILocalizer _localizer;

        public AppController(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        public void RunApp()
        {
            var vm = new MenuViewModel(this, _localizer);
            new MenuView(vm).Run();
        }
        public void RunExecuteBackup()
        {
            /*
            var vm = new ExecuteBackup(new BackupService(), _localizer);
            new ExecuteBackUpView(vm).Run();
            */
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
        public void Exit()
        {
            Environment.Exit(0);
        }
        // ajouter BackUp managment et potentiel autre
    }

}