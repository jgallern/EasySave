using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace BackUp.ViewModel
{
    public class TranslationManager
    {
        private readonly string _resourcesPath;
        private readonly string _appConfigPath;
        public List<string> language_list { get; }
        private Dictionary<string, string> _language;
        private Dictionary<string, string> _translations;
        
        public TranslationManager()
        {
            _appConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "env\\appconfig.json");
            _resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
            LoadAppConfigLanguage();
            List<string> availableLanguages = GetAvailableLanguages();
            Console.WriteLine("Available languages: " + string.Join(", ", availableLanguages));
            LoadTranslations(_language["Language"]);
        }

        public void LoadAppConfigLanguage()
        {
            string json = File.ReadAllText(_appConfigPath);
            _language = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public void SaveAppConfig()
        {
            string json = JsonConvert.SerializeObject(_language, Formatting.Indented);
            File.WriteAllText(_appConfigPath, json);
        }
        public void ChangeLanguage(string language)
        {
            _language["Language"] = language;
            SaveAppConfig();
            LoadTranslations(language);
        }

        public void LoadTranslations(string language)
        {
            string filePath = Path.Combine(_resourcesPath, $"Strings.{language}.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                _translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            else
            {
                filePath = Path.Combine(_resourcesPath, $"Strings.json");
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    _translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                }
            }
        }

        public string GetTranslation(string key)
        {
            return _translations.TryGetValue(key, out string translation) ? translation : GetDefaultTranslation(key);
        }

        public List<string> GetAvailableLanguages()
        {
            List<string> languages = new List<string>();

            if (Directory.Exists(_resourcesPath))
            {
                string[] files = Directory.GetFiles(_resourcesPath, "Strings.*.json");

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    Match match = Regex.Match(fileName, @"^Strings\.([a-zA-Z\-]+)\.json$");

                    if (match.Success)
                    {
                        string langCode = match.Groups[1].Value;
                        languages.Add(langCode);
                    }
                }
            }

            return languages;
        }

        public string GetDefaultTranslation(string key)
        {
            string filePath = Path.Combine(_resourcesPath, $"Strings.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json).TryGetValue(key, out string default_value) ? default_value : key;
            }
            else
            {
                return key;
            }
        }
    }
}

