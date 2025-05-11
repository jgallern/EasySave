namespace BackUp.ViewModel
{
    public interface IAppController
    {
        void RunApp();
        void RunManageJobs();
        void RunExecuteBackup();
        void RunSettings();
        void Exit();
    }

}