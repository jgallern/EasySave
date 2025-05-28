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
        public static SemaphoreSlim LargeFileSemaphore = new SemaphoreSlim(2);
        public static async Task ExecuteSelectedJobs(List<BackUpJob> jobs, IUIErrorNotifier notifier)
        {
            foreach (BackUpJob job in jobs)
            {
                job.IsSelected = false;
            }

            //Parallele run for the priority files 

            //Await all tasks finished 

            // Parallele run for the non priority files 
            List<Task> tasks = jobs.Select(async job =>
            {
                try
                {
                    //await job.RunForPrioritizedFiles();
                    await job.Run();
                    //await job.RunForNonPrioritizedFiles();
                    notifier.ShowSuccess($"Job {job.Id} done!");
                }
                catch (Exception ex)
                {
                    notifier.ShowError($"Job {job.Id} failed: {ex.Message}");
                }

            }).ToList();

            await Task.WhenAll(tasks);

        }
    }
}
