namespace BackUp.ViewModel
{
    public class ExecuteBackUpServices 
    {
        /*
        private readonly IBackupService _backupService;
        private const int MaxJobId = 5;

        public ICommand RunJobsCommand { get; }

        public ExecuteBackUpViewModel(IBackupService backupService)
        {
            _backupService = backupService;
            RunJobsCommand = new RelayCommand<string>(
                input => ExecuteJobs(input),
                input => DetectJobs(input)
            );
        }

        private bool IsValidInput(string? input)
        {
            // Réutilise ta méthode de validation
            if (string.IsNullOrWhiteSpace(input)) return false;
            foreach (char c in input!)
                if (!char.IsDigit(c) && c != ',' && c != '-' && c != '*')
                    return false;
            return true;
        }*/
        /*
        private string DetectJobs(string input)
        {
            string inputError = null;
            const int MaxJobId = 5; // Changer par le max dans la config du user
            // Réinitialise à chaque appel
            _jobs = new List<int>();
            _isAll = _isList = _isRange = _isSingleJob = false;

            if (string.IsNullOrWhiteSpace(input))
            {
                inputError = "❌ Please enter something.";//Mettre les messages d'erreurs dans une classe a part
                return inputError;
            }

            if (!IsValidInput(input))
            {
                inputError = "❌ Invalid input. Only digits, ',', '-', and '*' are allowed.";
                return inputError;
            }

            // Si l'entrée est '*', on exécute tous les jobs
            if (input == "*")
            {
                _isAll = true;
                return inputError;
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
                        return inputError;
                    }
                    else
                    {
                        inputError = "❌ Invalid range.";
                        return inputError;
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
                            inputError = $"❌ Invalid job number: {job.Trim()}.";
                            return inputError;
                        }
                    }
                    else
                    {
                        inputError = $"❌ Invalid job in list: {job.Trim()}.";
                        return inputError;
                    }
                }
                _isList = true;
                return inputError;
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
                    inputError = $"❌ Invalid job number: {input.Trim()}.";
                    return inputError;
                }
            }
            else
            {
                inputError = $"❌ Invalid input: {input.Trim()}.";
                return inputError;
            }
            return null;
        }

        private void ExecuteJobs(string input)
        {
            if (DetectJobs(input)){
            // Copie ta méthode DetectJobs, ici simplifiée :
            var jobs = JobParser.Parse(input, MaxJobId);
            if (jobs == null)
            {
                // tu peux lever ou logger une erreur
                return;
            }

            // Exécute réellement les jobs
            foreach (int jobId in jobs)
            {
                _backupService.RunJob(jobId);
            }
        }*/
    }

}