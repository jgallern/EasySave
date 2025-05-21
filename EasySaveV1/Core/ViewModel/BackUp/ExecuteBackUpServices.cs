using Core.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Core.ViewModel
{
    public class ExecuteBackUpServices : IExecuteBackUpServices, INotifyPropertyChanged
    {
        private const int MaxJobId = 5;
        private AppController _app;
        private List<int> _availableJobsId;
        private List<string> _availableJobsName;
        private List<int> _result;
        private string _resultMessage = string.Empty;

        public ICommand ExecuteBackUpCommand { get; }

        public string JobList { get
            {
                var lines = _availableJobsId.Zip(_availableJobsName, (id, name) => $"{id}: {name}");
                return string.Join("\n\t", lines);
            }
        }

        public string ResultMessage
        {
            get => _resultMessage;
            set
            {
                if (_resultMessage != value)
                {
                    _resultMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string LogPath => Logger.GetLogDirectory();

        // Constructeur pour initialiser les dépendances
        public ExecuteBackUpServices(AppController app)
        {
            _app = app;
            (_availableJobsId, _availableJobsName) = GetConfigJobsID(); // Obtenir les jobs disponibles

            // Initialisation de la commande
            ExecuteBackUpCommand = new RelayCommand(
                param =>
                {
                    var input = param as string ?? string.Empty;
                    TryParseJobs(input); // Valider les entrées
                    if (_resultMessage == string.Empty)
                    {
                        ResultMessage = ExecuteJobs();
                    }
                    else
                    {
                        ResultMessage = _resultMessage;
                    }
                },
                param =>
                {
                    var input = param as string ?? string.Empty;
                    return !string.IsNullOrEmpty(input); // Vérifier que l'entrée n'est pas vide
                }
            );
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        //-------------------------- Parsing ----------------------------------------

        private void TryParseJobs(string input)
        {
            _result = new List<int>();
            _resultMessage = string.Empty; // Réinitialiser l'erreur à chaque nouvelle validation

            if (string.IsNullOrWhiteSpace(input))
            {
                _resultMessage = _app.Translate("no_input");
                return;
            }

            foreach (char c in input)
            {
                if (!char.IsDigit(c) && c != ',' && c != '-' && c != '*')
                {
                    _resultMessage = _app.Translate("invalid_run_input");
                    return;
                }
            }

            // Si l'entrée est '*', on exécute tous les jobs
            if (input == "*")
            {
                _result = new List<int>(_availableJobsId);
                return;
            }

            // Si l'entrée contient un tiret, c'est une plage de jobs
            if (input.Contains("-"))
            {
                ParseRange(input);
                return;
            }

            // Si l'entrée contient des virgules, on considère une liste de jobs
            if (input.Contains(","))
            {
                ParseList(input);
                return;
            }

            // Si l'entrée est un seul nombre
            if (int.TryParse(input.Trim(), out int single))
            {
                if (_availableJobsId.Contains(single))
                {
                    _result = new List<int> { single };
                    return;
                }
                else
                {
                    _resultMessage = _app.Translate("invalid_job_num") + $" {single}.";
                    return;
                }
            }

            _resultMessage = _app.Translate("invalid_input") + $" {input.Trim()}."; // Erreur générique si aucune condition n'est remplie
        }

        private void ParseRange(string input)
        {
            string[] parts = input.Split('-');

            if (parts.Length != 2)
            {
                _resultMessage = _app.Translate("invalid_range");
                return;
            }

            bool startOk = int.TryParse(parts[0].Trim(), out int start);
            bool endOk = int.TryParse(parts[1].Trim(), out int end);

            if (!startOk && string.IsNullOrWhiteSpace(parts[0]))
                start = 1;

            if (!endOk && string.IsNullOrWhiteSpace(parts[1]))
                end = MaxJobId;

            if (start > end)
            {
                _resultMessage = _app.Translate("start_supp_end");
                return;
            }

            for (int i = start; i <= end; i++)
                if (_availableJobsId.Contains(i))
                    _result.Add(i);
        }

        private void ParseList(string input)
        {
            string[] jobs = input.Split(',');
            foreach (var job in jobs)
            {
                if (!int.TryParse(job.Trim(), out int jobId))
                {
                    _resultMessage += _app.Translate("invalid_input_list") + $" {job}. ";  // Concaténer les erreurs
                    continue;
                }

                if (!_availableJobsId.Contains(jobId))
                {
                    _resultMessage += _app.Translate("invalid_num_list") + $" {jobId}. "; // Concaténer les erreurs
                    continue;
                }

                _result.Add(jobId);
            }
        }

        private static (List<int>, List<string>) GetConfigJobsID()
        {
            List<BackUpJob> allJobs = BackUpJob.GetAllJobsFromConfig(); // Appel de la méthode qui récupère les jobs
            List<int> ids = allJobs.Select(job => job.Id).ToList();
            List<string> names = allJobs.Select(job => job.Name).ToList();
            return (ids, names);
        }

        private string ExecuteJobs()
        {
            if (!string.IsNullOrEmpty(_resultMessage) || _result == null)
            {
                return _resultMessage ?? _app.Translate("no_job_config"); // Retourner le message d'erreur si les jobs sont vides
            }

            // Exécuter réellement les jobs
            foreach (int jobId in _result)
            {
                BackUpJob job = BackUpJob.GetJobByID(jobId);
                try
                {
                    job.Run();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(_app.Translate("run_succeed"));
                    Console.ResetColor();
                    Console.WriteLine($" {job.Id}:{job.Name}.");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(_app.Translate("execution_error"));
                    Console.ResetColor();
                    Console.WriteLine($" {job.Id}:{job.Name}.");
                }
            }
            return _app.Translate("log_repo") + LogPath;
        }
    }
}
