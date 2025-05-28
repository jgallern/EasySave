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
    public class EncryptionAppConfigTest : IDisposable
    {
        private readonly string _tempEnvPath;
        private string _appConfigPath;
        private ILocalizer _localizer;

        public EncryptionAppConfigTest()
        {
            // Create a tomporary repository
            var basePath = Directory.GetCurrentDirectory();
            _tempEnvPath = Path.Combine(basePath, "env");

            // S'assure que le dossier existe
            Directory.CreateDirectory(_tempEnvPath);
        }

        private void CreateRealAppConfig()
        {
            _appConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "appconfig.json");

            Dictionary<string, string> config = new()
            {
                { "Language", "en" },
                { "EncryptionExtensions", ".txt, .docx" },
                { "SoftwarePackages", "Notepad++, Visual Studio" },
                { "CryptoSoftKey", "abc123" },
                { "PriorityFiles", ".pdf, .html" }
            };

            File.WriteAllText(_appConfigPath, JsonConvert.SerializeObject(config, Formatting.Indented));

            _localizer = new Localizer(); // Ne change pas
        }

        private void Cleanup()
        {
            if (File.Exists(_appConfigPath))
                File.Delete(_appConfigPath);
        }
        public void Dispose()
        {
            // Nettoyage après les tests
            if (Directory.Exists(_tempEnvPath)) Directory.Delete(_tempEnvPath, true);
        }

        
        [Fact]
        public void RetrieveEncryptionExtensions_Should_Work()
        {
            CreateRealAppConfig();
            _appConfigPath = Path.Combine(_tempEnvPath, $"appconfig_{Guid.NewGuid()}.json");

            string result = _localizer.GetEncryptionExtensions();

            // Assert
            Assert.Equal(".zip, .pdf", result);
            Cleanup();
        }

        [Fact]
        public void ChangeAndRetrieveEncryptionExtensions_Should_Work()
        {
            CreateRealAppConfig();
            _appConfigPath = Path.Combine(_tempEnvPath, $"appconfig_{Guid.NewGuid()}.json");

            string newExtensions = ".zip, .pdf,,";
            string result = _localizer.ChangeEncryptionExtensions(newExtensions);

            // Assert
            Assert.Equal(".zip, .pdf", result);
            Cleanup();
        }


        [Fact]
        public void RetrieveEncryptionKey_Should_Work()
        {
            CreateRealAppConfig();
            _appConfigPath = Path.Combine(_tempEnvPath, $"appconfig_{Guid.NewGuid()}.json");

            string result = _localizer.GetEncryptionKey();

            // Assert
            Assert.Equal("newKey456", result);
            Cleanup();
        }


        [Fact]
        public void ChangeAndRetrieveEncryptionKey_Should_Work()
        {
            CreateRealAppConfig();
            _appConfigPath = Path.Combine(_tempEnvPath, $"appconfig_{Guid.NewGuid()}.json");

            string newKey = "newKey456";
            string result = _localizer.ChangeEncryptionKey(newKey);

            // Assert
            Assert.Equal("newKey456", result);

        }
    }
}
