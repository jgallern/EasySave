using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Core.Model.Managers
{
    public class AppConfigManager : IAppConfigManager
    {
        private static AppConfigManager _instance;
        private static readonly object _lock = new object();
        private static readonly object _lockWriteConfig = new object();
        private readonly string _resourcesPath;
        private readonly string _appConfigPath;
        private Dictionary<string, string> _config = new();

        private AppConfigManager()
        {
            _appConfigPath = Path.Join(AppContext.BaseDirectory, Path.Join("env","appconfig.json"));
            LoadAppConfig();
        }

        public static AppConfigManager Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new AppConfigManager();
                }
            }
        }

        // ------- new methods -----------
        public void ChangeAppConfigParameter(string parameter, string value)
        {
            try
            {
                _config[$"{parameter}"] = value;
                SaveAppConfig();
                LoadAppConfig();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during the changement of the {parameter} with the value {value} in the AappConfig", ex);
            }
        }

        public string GetAppConfigParameter(string parameter)
        {
            if (_config.TryGetValue($"{parameter}", out string value))
            {
                return value;
            }
            if (parameter == "Language")
            {
                return "en";
            }
            return null;
        }
        public void LoadAppConfig()
        {
            string json = File.ReadAllText(_appConfigPath);
            _config = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public void SaveAppConfig()
        {
            lock (_lockWriteConfig)
            {
                string json = JsonConvert.SerializeObject(_config, Formatting.Indented);
                File.WriteAllText(_appConfigPath, json);
            }
        }
    }
}

