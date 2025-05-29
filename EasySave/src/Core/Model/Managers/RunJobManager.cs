using Core.Model.Interfaces;
using Core.Model.Services;
using Core.ViewModel.Notifiers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Managers
{
    public class RunJobManager
    {
        public static SemaphoreSlim LargeFileSemaphore = new SemaphoreSlim(2);

        private static SemaphoreSlim _executionLock = new SemaphoreSlim(1, 1);

        private static object _lockCurrentRunningJob = new object();

        public static ManualResetEventSlim PauseEvent = new ManualResetEventSlim(false);

        private static List<BackUpJob> _currentRunningJobs = new();

        // For the processes detection
        private static bool _wasPreviouslyBlocked = false;
        private static List<string> _lastBlockingProcesses = new();

        private static CancellationTokenSource _monitoringCts;
        private static Task _monitoringTask;





        public static async Task ExecuteSelectedJobs(List<BackUpJob> jobs, ILocalizer localizer, IUIErrorNotifier notifier)
        {
            await _executionLock.WaitAsync();
            try
            {
                List<BackUpJob> jobsToRun;
                lock (_lockCurrentRunningJob)
                {
                    jobsToRun = jobs
                        .Where(j => !_currentRunningJobs.Any(rj => rj.Id == j.Id))
                        .ToList();

                    _currentRunningJobs.AddRange(jobsToRun);
                    if (jobsToRun.Any())
                    {
                        _currentRunningJobs.AddRange(jobsToRun);
                        StartMonitoring(localizer, notifier); // start the monitoring if new jobs are added 
                    }
                }

                //Décochez les selectbox
                foreach (BackUpJob job in jobsToRun)
                {
                    job.IsSelected = false;
                }

                await ExecuteJobs(jobsToRun, localizer, notifier);
            }
            finally
            {
                _executionLock.Release();
            }

        }

        private static async Task ExecuteJobs(List<BackUpJob> jobsToRun, ILocalizer localizer, IUIErrorNotifier notifier)
        {
            // Parallele run for the non priority files 
            List<Task> tasks = jobsToRun.Select(async job =>
            {
                try
                {
                    //await job.RunForPrioritizedFiles();
                    await job.Run();
                    //await job.RunForNonPrioritizedFiles();
                    job.Statement = Statement.Done;
                    job.AlterJob();
                    notifier.ShowSuccess($"Job {job.Id} done!");
                }
                catch (Exception ex)
                {
                    job.Statement = Statement.Error;
                    job.AlterJob();
                    notifier.ShowError($"Job {job.Id} failed: {ex.Message}");
                }
                lock (_lockCurrentRunningJob)
                {
                    _currentRunningJobs.Remove(job);
                }

            }).ToList();

            await Task.WhenAll(tasks);
            StopMonitoring(); // Stop monitoring only when all jobs done
        }



        private static void MonitorBlockingProcesses(ILocalizer localizer, IUIErrorNotifier notifier, CancellationToken token)
        {
            _monitoringTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    (bool blockingDetected, List<string> detectedBlocking) = CheckProcesses();
                    List<BackUpJob> activeJobs;
                    lock (_lockCurrentRunningJob)
                    {
                        activeJobs = _currentRunningJobs
                            .Where(job => job.Statement != Statement.Done && job.Statement != Statement.Error)
                            .ToList();
                    }

                    if (blockingDetected)
                    {
                        PauseEvent.Reset(); // Pause les threads
                        lock (_lockCurrentRunningJob)
                        {
                            foreach (BackUpJob job in activeJobs)
                            {
                                job.Statement = Statement.Paused;
                                job.ChangeStatement();
                            }
                            if (!_wasPreviouslyBlocked || !_lastBlockingProcesses.SequenceEqual(detectedBlocking))
                            {
                                _wasPreviouslyBlocked = true;
                                _lastBlockingProcesses = detectedBlocking;
                                notifier.ShowError($"{localizer["blocked_process"]} {string.Join("\n", detectedBlocking)}");
                            }
                        }
                    }
                    else
                    {
                        PauseEvent.Set();   // Reprend les threads
                        foreach (BackUpJob job in activeJobs)
                        {
                            job.Statement = Statement.Running;
                            job.ChangeStatement();
                        }
                    }

                    try
                    {
                        await Task.Delay(1000, token); // vérifie toutes les 1 seconde avec possibilité d’annuler
                    }
                    catch (TaskCanceledException)
                    {
                        // Arrêt demandé, on sort de la boucle
                        break;
                    }
                }
            }, token);
        }


        private static void StartMonitoring(ILocalizer localizer, IUIErrorNotifier notifier)
        {
            if (_monitoringCts != null) return; // déjà en cours

            _monitoringCts = new CancellationTokenSource();
            MonitorBlockingProcesses(localizer, notifier, _monitoringCts.Token);
        }

        private static void StopMonitoring()
        {
            if (_monitoringCts == null) return;

            _monitoringCts.Cancel();
            try { _monitoringTask?.Wait(); } catch { }
            _monitoringCts.Dispose();
            _monitoringCts = null;
            _monitoringTask = null;
        }





        public static (bool, List<string>) CheckProcesses()
        {
            var runningProcesses = Process.GetProcesses();
            string blockedProcesses = AppConfigManager.Instance.GetAppConfigParameter("SoftwarePackages");
            List<string> blockedProcessesList = blockedProcesses
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .ToList();

            List<string> detectedBlocking = runningProcesses
              .Where(proc => blockedProcessesList.Contains(proc.ProcessName, StringComparer.OrdinalIgnoreCase))
              .Select(proc => proc.ProcessName)
              .Distinct()
              .ToList();

            bool blockingDetected = detectedBlocking.Any();

            return (blockingDetected, detectedBlocking);
        }
    }
}
