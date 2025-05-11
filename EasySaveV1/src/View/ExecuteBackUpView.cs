using BackUp.ViewModel;
using System;
using BackUp.View;

namespace BackUp.View
{
    public class ExecuteBackUpView : IView
    {
        private readonly MainViewModel _viewModel;
        private string _userInput;
        private List<int> _jobs;
        private bool _isAll;
        private bool _isList;
        private bool _isRange;
        private bool _isSingleJob;

        public ExecuteBackUpView(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Run()
        {
            List<string> availableLanguages = _viewModel.GetAvailableLanguages();
            int selectedIndex = 0;
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine($"=== {_viewModel.GetTranslation("run_jobs_menu")} ===\n");

                Console.WriteLine("Select the jobs you want to execute by enter the number of the jobs");
                Console.WriteLine("(e.g: '1-4': execute all jobs from 1 to 4\n      '1,2,5': execute the jobs in this list\n      '2': execute the job '2'\n      '*': execute all jobs\n");
                Console.Write(">");
                _userInput = Console.ReadLine();
                // Affichage pour débogage
                //Console.WriteLine($"Input entered: {_userInput}");
                //Console.ReadKey();

                DetectJobs(_userInput);

                if (_isAll)
                {
                    Console.WriteLine("execute all backup");
                    Console.ReadKey();
                    break;
                    //Execute all backup
                }
                if (_isList)
                {
                    Console.WriteLine("execute a list of backup");
                    Console.ReadKey();
                    break;
                    //Execute the list of backup
                }
                if (_isRange)
                {
                    Console.WriteLine("execute a range of backup");
                    Console.ReadKey();
                    break;
                    //Execute the range of backup
                }
                if (_isSingleJob)
                {
                    Console.WriteLine("execute a backup");
                    Console.ReadKey();
                    break;
                    //Execute a backup
                }
                
            } while (true);
        }

        private void DetectJobs(string input)
        {
            const int MaxJobId = 5; // Changer par le max dans la config du user
            // Réinitialise à chaque appel
            _jobs = new List<int>();
            _isAll = _isList = _isRange = _isSingleJob = false;

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Please enter something.");//Mettre les messages d'erreurs dans une classe a part
                Console.ReadKey();
                Console.ResetColor();
                return;
            }

            if (!IsValidInput(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Invalid input. Only digits, ',', '-', and '*' are allowed.");
                Console.ReadKey();
                Console.ResetColor();
                return;
            }

            // Si l'entrée est '*', on exécute tous les jobs
            if (input == "*")
            {
                _isAll = true;
                return;
            }

            // Si l'entrée contient un tiret, c'est une plage de jobs
            if (input.Contains("-"))
            {
                string[] parts = input.Split('-');

                if (parts.Length == 2)
                {
                    bool hasStart = int.TryParse(parts[0].Trim(), out int start);
                    bool hasEnd = int.TryParse(parts[1].Trim(), out int end);

                    if (!hasStart && hasEnd)
                        start = 1;
                    if (hasStart && !hasEnd)
                        end = MaxJobId;

                    if ((hasStart || !string.IsNullOrWhiteSpace(parts[0])) &&
                        (hasEnd || !string.IsNullOrWhiteSpace(parts[1])) && start <= end)
                    {
                        for (int i = start; i <= end && i <= MaxJobId; i++)
                            _jobs.Add(i);

                        _isRange = true;
                        return;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Invalid range.");
                        Console.ReadKey();
                        Console.ResetColor();
                        return;
                    }
                }
            }

            // Si l'entrée contient des virgules, on considère une liste de jobs
            if (input.Contains(","))
            {
                string[] jobs = input.Split(',');
                foreach (var job in jobs)
                {
                    if (int.TryParse(job.Trim(), out int singleJob))
                    {
                        if (singleJob >= 1 && singleJob <= MaxJobId)
                        {
                            _jobs.Add(singleJob);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"❌ Invalid job number: {job.Trim()}.");
                            Console.ReadKey();
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"❌ Invalid job in list: {job.Trim()}.");
                        Console.ReadKey();
                        Console.ResetColor();
                    }
                }
                _isList = true;
                return;
            }

            // Si l'entrée est un seul nombre
            if (int.TryParse(input.Trim(), out int parsedJob))
            {
                if (parsedJob >= 1 && parsedJob <= MaxJobId)
                {
                    _jobs.Add(parsedJob);
                    _isSingleJob = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌ Invalid job number: {input.Trim()}.");
                    Console.ReadKey();
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Invalid input: {input.Trim()}.");
                Console.ReadKey();
                Console.ResetColor();
            }
        }

        private bool IsValidInput(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c) && c != ',' && c != '-' && c != '*')
                    return false;
            }
            return true;
        }


    }
}
