using Core.Model.Managers;
using Core.Model.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Unit_Tests
{
    public class LocalizerIntegrationTests : IDisposable
    {
        private readonly string _tempEnvPath;
        private readonly string _tempResourcesPath;
        private readonly string _appConfigPath;

        public LocalizerIntegrationTests()
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
                { "save", "Save" }
            };
            File.WriteAllText(Path.Combine(_tempResourcesPath, "Strings.json"), JsonConvert.SerializeObject(translations, Formatting.Indented));
        }

        public void Dispose()
        {
            // Nettoyage après les tests
            if (Directory.Exists(_tempEnvPath)) Directory.Delete(_tempEnvPath, true);
            if (Directory.Exists(_tempResourcesPath)) Directory.Delete(_tempResourcesPath, true);
        }

        [Fact]
        public void Indexer_Should_Return_Translation()
        {
            // Arrange
            var localizer = new Localizer();
            localizer.ChangeLanguage("fr");

            // Act
            var translation = localizer["exit"];

            // Assert
            Assert.Equal("Quitter", translation);
        }

        [Fact]
        public void ChangeAndRetrieveEncryptionExtensions_Should_Work()
        {
            // Arrange
            var localizer = new Localizer();

            string newExtensions = ".zip, .pdf";
            localizer.ChangeEncryptionExtensions(newExtensions);

            // Act
            var result = localizer.GetEncryptionExtensions();

            // Assert
            Assert.Equal(".zip, .pdf", result);
        }

        [Fact]
        public void ChangeAndRetrieveSoftwarePackages_Should_Work()
        {
            // Arrange
            var localizer = new Localizer();

            string newSoftware = "Word.exe, Excel.exe";
            localizer.ChangeSoftwarePackages(newSoftware);

            // Act
            var result = localizer.GetSoftwarePackages();

            // Assert
            Assert.Equal("Word.exe, Excel.exe", result);
        }

        [Fact]
        public void ChangeAndRetrieveEncryptionKey_Should_Work()
        {
            // Arrange
            var localizer = new Localizer();

            string newKey = "newKey456";
            localizer.ChangeEncryptionKey(newKey);

            // Act
            var result = localizer.GetEncryptionKey();

            // Assert
            Assert.Equal("newKey456", result);
        }

        [Fact]
        public void GetAvailableLanguages_Should_Detect_LanguageFiles()
        {
            // Arrange
            var localizer = new Localizer();

            // Act
            var languages = localizer.GetAvailableLanguages();

            // Assert
            Assert.Contains("en, fr", languages);
        }
    }
}
