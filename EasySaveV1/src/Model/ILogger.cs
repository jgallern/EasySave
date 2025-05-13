using System.Text.Json;

namespace BackUp.Model{

    public interface ILogger
    {
        public string GetLogDirectory();

        public string GetDailyLogDirectory();

        public string GetDailyLogPath();

        public string GetStatusLogPath();

        public void AddLogInfo(LogType logType, Dictionary<string, object> logEntry);
    }
}