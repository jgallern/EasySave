using System.Text.Json;

namespace Core.Model{

    public interface ILogger
    {
        public string GetDailyLogDirectory();

        public string GetDailyLogPath();

        public string GetStatusLogPath();

        public void AddLogInfo(LogType logType, Dictionary<string, object> logEntry);
        public void OpenLogs();
    }
}
