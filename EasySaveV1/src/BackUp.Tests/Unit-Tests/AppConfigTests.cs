using BackUp.Model;
using System.Globalization;
using System.IO;
using Xunit;

namespace BackUp.Tests.Unit
{
    public class AppConfigTests
    {
        [Fact]
        public void AppConfig_ShouldCreateConfigFile_WhenNotExists()
        {
            // Arrange
            AppConfig.Reset(); // Assure un fichier propre

            // Act
            var config = new AppConfig();

            // Assert
            Assert.True(File.Exists("../env/appconfig.json"));
            Assert.Equal("en-US", config.Language);
        }

        [Fact]
        public void AppConfig_ChangeLanguage_ShouldPersistValue()
        {
            // Arrange
            var config = AppConfig.Reset(); // Nouveau fichier
            var newLang = "fr-FR";

            // Act
            config.ChangeLanguage(newLang);
            var loadedConfig = AppConfig.Load();

            // Assert
            Assert.Equal("fr-FR", loadedConfig.Language);
            Assert.Equal(new CultureInfo("fr-FR").Name, loadedConfig.GetCultureInfo().Name);
        }

        [Fact]
        public void AppConfig_Load_ShouldReturnDefaultWhenFileMissing()
        {
            // Arrange
            if (File.Exists("../env/appconfig.json"))
                File.Delete("../env/appconfig.json");

            // Act
            var config = AppConfig.Load();

            // Assert
            Assert.NotNull(config);
            Assert.Equal("en-US", config.Language);
        }
    }
}
