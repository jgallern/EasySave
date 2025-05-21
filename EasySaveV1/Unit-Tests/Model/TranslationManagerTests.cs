using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Core.Model;
using System.Reflection;

namespace Core.Tests
{
    public class TranslationManagerTests : IDisposable
    {
        private readonly string _testRoot;
        private readonly string _resourcesPath;
        private readonly string _appConfigPath;

        public TranslationManagerTests()
        {
            // Préparation des chemins de test
            _testRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testRoot);

            _resourcesPath = Path.Combine(_testRoot, "Resources");
            Directory.CreateDirectory(_resourcesPath);

            string envPath = Path.Combine(_testRoot, "env");
            Directory.CreateDirectory(envPath);

            _appConfigPath = Path.Combine(envPath, "appconfig.json");

            // Fichier appconfig
            File.WriteAllText(_appConfigPath, "{\"Language\": \"fr-FR\"}");

            // Fichier de traduction fr-FR
            File.WriteAllText(Path.Combine(_resourcesPath, "Strings.fr-FR.json"), "{ \"Hello\": \"Bonjour\" }");

            // Fichier de fallback
            File.WriteAllText(Path.Combine(_resourcesPath, "Strings.json"), "{ \"Hello\": \"Hello\", \"Bye\": \"Goodbye\" }");

            // Override du singleton pour forcer les chemins
            ResetSingleton();
            SetPrivateStaticField("_instance", null);

            var manager = typeof(TranslationManager);
            var resPathField = manager.GetField("_resourcesPath", BindingFlags.Instance | BindingFlags.NonPublic);
            var configPathField = manager.GetField("_appConfigPath", BindingFlags.Instance | BindingFlags.NonPublic);

            var instance = TranslationManager.Instance;
            resPathField.SetValue(instance, _resourcesPath);
            configPathField.SetValue(instance, _appConfigPath);

            instance.LoadAppConfigLanguage();
            instance.LoadTranslations(instance.GetCurrentLanguage());
        }

        [Fact]
        public void GetTranslation_ShouldReturnCorrectTranslation_WhenKeyExists()
        {
            var translation = TranslationManager.Instance.GetTranslation("Hello");
            Assert.Equal("Bonjour", translation);
        }

        [Fact]
        public void GetTranslation_ShouldFallback_WhenKeyMissingInCurrentLanguage()
        {
            var translation = TranslationManager.Instance.GetTranslation("Bye");
            Assert.Equal("Goodbye", translation); // fallback depuis Strings.json
        }

        [Fact]
        public void GetTranslation_ShouldReturnKey_WhenMissingEverywhere()
        {
            var translation = TranslationManager.Instance.GetTranslation("UnknownKey");
            Assert.Equal("UnknownKey", translation);
        }

        [Fact]
        public void ChangeLanguage_ShouldReloadCorrectTranslation()
        {
            // Ajouter une autre langue
            File.WriteAllText(Path.Combine(_resourcesPath, "Strings.en-US.json"), "{ \"Hello\": \"Hello\" }");

            TranslationManager.Instance.ChangeLanguage("en-US");
            var translation = TranslationManager.Instance.GetTranslation("Hello");

            Assert.Equal("Hello", translation);
        }

        public void Dispose()
        {
            // Nettoyage
            if (Directory.Exists(_testRoot))
                Directory.Delete(_testRoot, true);

            // Réinitialisation du singleton
            SetPrivateStaticField("_instance", null);
        }

        private void SetPrivateStaticField(string fieldName, object value)
        {
            var field = typeof(TranslationManager).GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, value);
        }

        private void ResetSingleton()
        {
            var instanceField = typeof(TranslationManager).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic);
            instanceField?.SetValue(null, null);
        }
    }
}
