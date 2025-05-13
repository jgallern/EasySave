using System.Text.Json;

namespace BackUp.Model{

    public class DailyLog
    {
        private static DailyLog _instance;
        private static readonly object _lock = new object();
        private readonly string _logpath;

        private DailyLog()
        {
            _logpath = GetLogDirectory();
        }
        public static DailyLog Instance { get { lock (_lock) { return _instance ??= new DailyLog(); } } }

        public string GetLogDirectory()
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "EasySave", "Logs");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            Console.WriteLine("Log directory", folder);
            Console.ReadKey();
            return folder;
        }

        public string GetDailyLogDirectory()
        {
            string folder = Path.Combine(_logpath, "DailyLog");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        public string GetDailyLogPath()
        {
            string dailyLogName = $"DailyLog_{DateOnly.FromDateTime(DateTime.Now):yyyy-MM-dd}.json";
            return Path.Combine(GetDailyLogDirectory(), dailyLogName);
        }

        public string GetStatusLogPath()
        {
            string statusLogName = "Status.json";
            return Path.Combine(_logpath, statusLogName);
        }


        public void AddLogInfo(LogType logType, Dictionary<string, object> logEntry)
        {
            string path = logType switch
            {
                LogType.Daily => GetDailyLogPath(),
                LogType.Status => GetStatusLogPath(),
                _ => throw new ArgumentOutOfRangeException(nameof(logType), "Invalid log type")
            };
            string json = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions { WriteIndented = true });
            File.AppendAllText(path, json);
        }
    }
}