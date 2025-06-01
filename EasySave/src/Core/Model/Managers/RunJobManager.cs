using Core.Model.Interfaces;
using Core.Model.Services;
using Core.ViewModel.Notifiers;
using Core.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Core.Model.Managers
{
    public class RunJobManager
    {
        public static SemaphoreSlim LargeFileSemaphore = new SemaphoreSlim(1,1);

        private static object _lockCurrentRunningJob = new object();

        public static ManualResetEventSlim PauseEventProcesses = new ManualResetEventSlim(false);

        private static List<BackUpJob> _currentRunningJobs = new();

        // For the processes detection
        private static bool _wasPreviouslyBlocked = false;
        private static List<string> _lastBlockingProcesses = new();

        private static CancellationTokenSource? _monitoringCts;
        private static Task? _monitoringTask;





        public static void ExecuteSelectedJobs(List<BackUpJob> jobs, ILocalizer localizer, IUIErrorNotifier notifier)
        {
            //Décochez les selectbox
            foreach (BackUpJob job in jobs)
            {
                job.IsSelected = false;
            }

            List<BackUpJob> jobsToRun;
            lock (_lockCurrentRunningJob)
            {
                jobsToRun = jobs
                    .Where(j => !_currentRunningJobs.Any(rj => rj.Id == j.Id))
                    .ToList();

                if (jobsToRun.Any())
                {
                    _currentRunningJobs.AddRange(jobsToRun);
                    StartMonitoring(localizer, notifier); // start the monitoring if new jobs are added 
                }
            }

            // Run execution without blocked the new demands
            _ = Task.Run(() => ExecuteJobs(jobsToRun, localizer, notifier));
        }

        private static async Task ExecuteJobs(List<BackUpJob> jobsToRun, ILocalizer localizer, IUIErrorNotifier notifier)
        {
            List<Task> tasks = jobsToRun.Select(async job =>
            {
                try
                {
                    await job.Run();
                    job.Statement = Statement.Done;
                    job.ChangeStatement();
                    notifier.ShowSuccess($"Job {job.Id} done!");
                }
                catch (Exception ex)
                {
                    if (job.Statement != Statement.Canceled)
                    {
                        job.Statement = Statement.Error;
                        job.ChangeStatement();
                        notifier.ShowError($"Job {job.Id} failed: {ex.Message}");
                    }
                }
                finally
                {
                    lock (_lockCurrentRunningJob)
                    {
                        BackUpJob toRemove = _currentRunningJobs.FirstOrDefault(j => j.Id == job.Id);
                        if (toRemove != null)
                            _currentRunningJobs.Remove(toRemove);
                    }
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
                        //PauseEventProcesses.Reset(); // Pause les threads
                        lock (_lockCurrentRunningJob)
                        {
                            foreach (BackUpJob job in activeJobs)
                            {
                                job.ProcessEventPause = true;
                                job.Pause();
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
                        //PauseEventProcesses.Set();   // Reprend les threads
                        foreach (BackUpJob job in activeJobs)
                        {
                            job.ProcessEventPause = false;
                            job.Resume();
                        }
                    }

                    try
                    {
                        await Task.Delay(100, token); // vérifie toutes les 1 seconde avec possibilité d’annuler
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



        public static void Pause(BackUpJob job)
        {
            if (job.Statement == Statement.Running)
            {
                job.ManualEventPause = true;
                job.Pause();
            }
        }

        public static void Stop(BackUpJob job)
        {
            job.Stop();
        }

        public static void RunJob(BackUpJob job, ILocalizer localizer, IUIErrorNotifier notifier)
        {
            if (job.Statement == Statement.Error || job.Statement == Statement.Canceled || job.Statement == Statement.Done)
            {
                job.ManualEventPause = false;
                job.Reset();
                List<BackUpJob> jobList = new List<BackUpJob>();
                jobList.Add(job);
                ExecuteSelectedJobs(jobList, localizer, notifier);

            }
            else
            {
                job.ManualEventPause = false;
                job.Resume();
            }
        }
    }
}
