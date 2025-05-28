using Core.Model.Interfaces;
using Core.Model.Managers;
using Core.Model.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Unit_Tests
{
    public class TranslationTest : IDisposable
    {
        private readonly string _tempEnvPath;
        private readonly string _tempResourcesPath;
        private readonly string _appConfigPath;
        private readonly ILocalizer _localizer;

        public TranslationTest()
        {
            // Create a tomporary repository
            var basePath = Directory.GetCurrentDirectory();
            _tempEnvPath = Path.Combine(basePath, "env");
            _tempResourcesPath = Path.Combine(basePath, "Resources");

            Directory.CreateDirectory(_tempEnvPath);
            Directory.CreateDirectory(_tempResourcesPath);

            // Crée un appconfig.json de test
            _appConfigPath = Path.Combine(_tempEnvPath, "appconfig.json");
            var config = new Dictionary<string, string>
            {
                { "Language", "en" },
                { "EncryptionExtensions", ".txt, .docx" },
                { "SoftwarePackages", "Notepad++, Visual Studio" },
                { "CryptoSoftKey", "abc123" }
            };
            File.WriteAllText(_appConfigPath, JsonConvert.SerializeObject(config, Formatting.Indented));

            // Crée un fichier de ressources Strings.en.json
            var translationsEn = new Dictionary<string, string>
            {
                { "exit", "Exit" },
                { "save", "Save" }
            };
            File.WriteAllText(Path.Combine(_tempResourcesPath, "Strings.en.json"), JsonConvert.SerializeObject(translationsEn, Formatting.Indented));

            // Crée un fichier de ressources Strings.en.json
            var translationsFr = new Dictionary<string, string>
            {
                { "exit", "Quitter" },
                { "save", "Sauvegarder" }
            };
            File.WriteAllText(Path.Combine(_tempResourcesPath, "Strings.fr.json"), JsonConvert.SerializeObject(translationsFr, Formatting.Indented));

            // Crée un fichier de ressources Strings.json
            var translations = new Dictionary<string, string>
            {
                { "exit", "Exit" },
                { "save", "Save" },
                { "new", "New" }
            };
            File.WriteAllText(Path.Combine(_tempResourcesPath, "Strings.json"), JsonConvert.SerializeObject(translations, Formatting.Indented));


            _localizer = new Localizer();
        }

        public void Dispose()
        {
            // Nettoyage après les tests
            if (Directory.Exists(_tempEnvPath)) Directory.Delete(_tempEnvPath, true);
            if (Directory.Exists(_tempResourcesPath)) Directory.Delete(_tempResourcesPath, true);
        }

        [Fact]
        public void Change_Language_Return_Translation()
        {
            _localizer.ChangeLanguage("fr");

            // Act
            string translation = _localizer["exit"];

            // Assert
            Assert.Equal("Quitter", translation);
        }

        [Fact]
        public void Indexer_Should_Return_Translation()
        {
            // Act
            string translation = _localizer["exit"];

            // Assert
            Assert.Equal("Exit", translation);
        }

        public void Indexer_Should_Return_Default_Translation()
        {
            // Act
            string translation = _localizer["new"];

            // Assert
            Assert.Equal("New", translation);
        }

        [Fact]
        public void GetAvailableLanguages_Should_Detect_LanguageFiles()
        {
            // Arrange
            var localizer = new Localizer();

            // Act
            List<string> languages = localizer.GetAvailableLanguages();

            // Assert
            Assert.Equal(["en", "fr"], languages);
        }
    }
}
