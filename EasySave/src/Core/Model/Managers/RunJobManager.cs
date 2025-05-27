using Core.ViewModel.Notifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Managers
{
    public class RunJobManager
    {
        public static async Task ExecuteSelectedJobs(List<BackUpJob> jobs, IUIErrorNotifier notifier)
        {
            foreach (var job in jobs)
            {
                job.IsSelected = false;
            }
            foreach (var job in jobs)
            {
                try
                {
                    await job.Run();
                    notifier.ShowSuccess($"Job {job.Id} done!");
                }
                catch (Exception ex)
                {
                    notifier.ShowError(ex.Message);
                }
            }

        }
    }
}
