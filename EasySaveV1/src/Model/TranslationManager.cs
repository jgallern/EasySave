using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace BackUp.ViewModel
{
    public class TranslationManager
    {
        private readonly string _resourcesPath;
        private readonly string _appConfigPath;
        private Dictionary<string, string> _language;
        private Dictionary<string, string> _translations;
        
        public TranslationManager()
        {
            _appConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "env\\appconfig.json");
            _resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
            Console.WriteLine($"Directory : {_resourcesPath}"); // Affichage ici, dans le constructeur
            LoadAppConfigLanguage();
            LoadTranslations(_language["Language"]);
        }

        public void LoadAppConfigLanguage()
        {
            string json = File.ReadAllText(_appConfigPath);
            _language = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public void SaveAppConfig()
        {
            Console.WriteLine($"Language selected : {_language["Language"]}");
            Console.WriteLine("Saving app config to: " + _appConfigPath); // debug
            string json = JsonConvert.SerializeObject(_language, Formatting.Indented);
            Console.WriteLine("New json: " + json); // debug
            File.WriteAllText(_appConfigPath, json);
        }
        public void ChangeLanguage(string language)
        {
            try
            {
                _language["Language"] = language;
                SaveAppConfig();
                LoadTranslations(language);
            }
            catch
            {
                Console.Write("An error occured while changing language.");
            }
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
                Console.WriteLine("Error Json not found");
                _translations = new Dictionary<string, string>(); // Charger des valeurs par défaut si fichier manquant.
            }
        }

        public string GetTranslation(string key)
        {
            try
            {
                return _translations.TryGetValue(key, out var translation) ? translation : key;
            }
            catch
            {
                Console.WriteLine("Cannot translate the element.");
                return _translations.TryGetValue(key, out var translation) ? translation : key;
            }
        }
    }
}

