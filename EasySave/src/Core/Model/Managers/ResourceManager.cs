using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace Core.Model.Managers
{
    public class ResourceManager
    {
        private static ResourceManager _instance;
        private static readonly object _lock = new object();
        private readonly string _resourcesPath;
        private Dictionary<string, string> _translations;

        public ResourceManager()
        {
            _resourcesPath = Path.Join(AppContext.BaseDirectory, "Resources");
        }

        public static ResourceManager Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new ResourceManager();
                }
            }
        }


        public void LoadTranslations(string language)
        {
            string filePath = Path.Join(_resourcesPath, $"Strings.{language}.json");

            if (File.Exists(filePath))
                _translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filePath));
            else
                LoadDefaultTranslations();
        }

        public string Translate(string key)
        {
            return _translations?.TryGetValue(key, out var translation) == true
                ? translation
                : GetDefaultTranslation(key);
        }

        public List<string> GetAvailableLanguages()
        {
            if (!Directory.Exists(_resourcesPath)) return new();

            return Directory.GetFiles(_resourcesPath, "Strings.*.json")
                .Select(Path.GetFileName)
                .Select(fn => Regex.Match(fn, @"^Strings\.([a-zA-Z\-]+)\.json$"))
                .Where(match => match.Success)
                .Select(match => match.Groups[1].Value)
                .ToList();
        }

        private void LoadDefaultTranslations()
        {
            string filePath = Path.Join(_resourcesPath, "Strings.json");
            if (File.Exists(filePath))
                _translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filePath));
        }

        private string GetDefaultTranslation(string key)
        {
            string filePath = Path.Join(_resourcesPath, "Strings.json");
            if (!File.Exists(filePath)) return key;

            var defaultTranslations = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filePath));
            return defaultTranslations.TryGetValue(key, out var defaultValue) ? defaultValue : key;
        }
    }
}
