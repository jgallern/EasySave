using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace BackUp.Model
{
    public class AppConfig
    {
        public string Language { get; set; } = "en-US";

        private static readonly string ConfigPath = "../env/appconfig.json";


        public AppConfig()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                {
                    Console.WriteLine("AppConfig creation...");
                    Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath));

                    // Crée un objet anonyme à la place d'une nouvelle AppConfig
                    var defaultConfig = new { Language = "en-US" };
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string json = JsonSerializer.Serialize(defaultConfig, options);

                    File.WriteAllText(ConfigPath, json);
                }
            }
            catch
            {
                Console.WriteLine("Error during AppConfig instantiation!");
                ErrInitialiseConfig();
            }
        }

        // Charge la configuration depuis un fichier JSON
        public static object Load()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                    return new AppConfig();

                string json = File.ReadAllText(ConfigPath);
                var config = JsonSerializer.Deserialize<AppConfig>(json);
                return config ?? new AppConfig();
            }
            catch
            {
                Console.WriteLine("Error during AppConfig loading!");
                return ErrLoadConfig();
            }
        }

        // Sauvegarde la configuration actuelle dans le fichier JSON
        public void ChangeLanguage(string language)
        {
            try
            {
                Language = language;
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(ConfigPath, json);
            }
            catch
            {
                Console.WriteLine("Error during the language modification!");
            }
        }

        public CultureInfo GetCultureInfo() => new CultureInfo(Language);

        public static object Reset()
        {
            try
            {
                if (File.Exists(ConfigPath))
                    File.Delete(ConfigPath);
                return new AppConfig();
            }
            catch
            {
                Console.WriteLine("Error during reset AppConfig!");
                return ErrResetConfig();
            }
        }

        private static AppConfig ErrInitialiseConfig()
        {
            throw new InvalidOperationException("Failed to initialise the AppConfig.");
        }

        private static AppConfig ErrLoadConfig()
        {
            throw new InvalidOperationException("Failed to load the AppConfig.");

        }
        private static AppConfig ErrResetConfig()
        {
            throw new InvalidOperationException("Failed to reset the AppConfig.");
        }
    }
}