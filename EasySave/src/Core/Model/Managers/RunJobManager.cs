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
            foreach (BackUpJob job in jobs)
            {
                job.IsSelected = false;
            }
            Parallel.ForEach(jobs, job =>
            {
                try
                {
                    //await job.RunForPrioritizedFiles();
                    _ = job.Run();
                    //await job.RunForNonPrioritizedFiles();
                    notifier.ShowSuccess($"Job {job.Id} done!");
                }
                catch (Exception ex)
                {
                    notifier.ShowError(ex.Message);
                }
            });

        }
    }
}
