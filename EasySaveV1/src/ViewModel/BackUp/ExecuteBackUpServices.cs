using BackUp.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BackUp.ViewModel
{
    public class ExecuteBackUpServices : IExecuteBackUpServices, INotifyPropertyChanged
    {
        private const int MaxJobId = 5;
        private AppController _app;
        private List<int> _availableJobs = GetConfigJobsID();
        private List<int> _result;
        private string _resultMessage = string.Empty;

        public ICommand ExecuteBackUpCommand { get; }

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

        // Constructeur pour initialiser les dépendances
        public ExecuteBackUpServices(AppController app)
        {
            _app = app;
            _availableJobs = GetConfigJobsID(); // Obtenir les jobs disponibles

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
                _resultMessage = "❌ Please enter something.";
                return;
            }

            foreach (char c in input)
            {
                if (!char.IsDigit(c) && c != ',' && c != '-' && c != '*')
                {
                    _resultMessage = "❌ Invalid input. Only digits, ',', '-', and '*' are allowed.";
                    return;
                }
            }

            // Si l'entrée est '*', on exécute tous les jobs
            if (input == "*")
            {
                _result = new List<int>(_availableJobs);
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
                if (_availableJobs.Contains(single))
                {
                    _result = new List<int> { single };
                    return;
                }
                else
                {
                    _resultMessage = $"❌ Invalid job number: {single}.";
                    return;
                }
            }

            _resultMessage = $"❌ Invalid input: {input.Trim()}."; // Erreur générique si aucune condition n'est remplie
        }

        private void ParseRange(string input)
        {
            string[] parts = input.Split('-');

            if (parts.Length != 2)
            {
                _resultMessage = "❌ Invalid range format.";
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
                _resultMessage = "❌ Invalid range: start > end.";
                return;
            }

            for (int i = start; i <= end; i++)
                if (_availableJobs.Contains(i))
                    _result.Add(i);
        }

        private void ParseList(string input)
        {
            string[] jobs = input.Split(',');
            foreach (var job in jobs)
            {
                if (!int.TryParse(job.Trim(), out int jobId))
                {
                    _resultMessage += $"❌ Invalid input in list: {job}. ";  // Concaténer les erreurs
                    continue;
                }

                if (!_availableJobs.Contains(jobId))
                {
                    _resultMessage += $"❌ Invalid job number in list: {jobId}. "; // Concaténer les erreurs
                    continue;
                }

                _result.Add(jobId);
            }
        }

        private static List<int> GetConfigJobsID()
        {
            var allJobs = BackUpJob.GetAllJobsFromConfig(); // Appel de la méthode qui récupère les jobs
            return allJobs.Select(job => job.Id).ToList();
        }

        private string ExecuteJobs()
        {
            if (_result == null || _result.Count == 0)
            {
                return _resultMessage ?? "No jobs to execute."; // Retourner le message d'erreur si les jobs sont vides
            }

            // Exécuter réellement les jobs
            foreach (int jobId in _result)
            {
                BackUpJob job = BackUpJob.GetJobByID(jobId);
                job.Run();
            }

            return "Successfully run BackUp"; // Message de succès
        }
    }
}
