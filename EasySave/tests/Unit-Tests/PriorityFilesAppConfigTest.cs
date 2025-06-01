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
    public class PriorityFilesAppConfigTest : IDisposable
    {
        private readonly string _tempEnvPath;
        private string _appConfigPath;
        private ILocalizer _localizer;

        public PriorityFilesAppConfigTest()
        {
            // Create a tomporary repository
            var basePath = Directory.GetCurrentDirectory();
            _tempEnvPath = Path.Combine(basePath, "env");

            // S'assure que le dossier existe
            Directory.CreateDirectory(_tempEnvPath);

            _appConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "appconfig.json");

            Dictionary<string, string> config = new()
            {
                { "Language", "en" },
                { "EncryptionExtensions", ".txt, .docx" },
                { "SoftwarePackages", "notepad" },
                { "CryptoSoftKey", "abc123" },
                { "PriorityFiles", ".pdf, .html" }
            };

            File.WriteAllText(_appConfigPath, JsonConvert.SerializeObject(config, Formatting.Indented));

            _localizer = new Localizer();
        }

        public void Dispose()
        {
            // Nettoyage après les tests
            if (Directory.Exists(_tempEnvPath)) Directory.Delete(_tempEnvPath, true);
        }

        /*
        [Fact]
        public void RetrievePriorityFiles_Should_Work()
        {
            string result = _localizer.GetPriorityFiles();

            // Assert
            Assert.Equal(".docx, .pdf", result);
        }
        */

        [Fact]
        public void ChangeAndRetrievePriorityFiles_Should_Work()
        {
            string newFiles = ".docx,, .pdf,";
            string result = _localizer.ChangePriorityFiles(newFiles);

            // Assert
            Assert.Equal(".docx, .pdf", result);
        }
    }
}
