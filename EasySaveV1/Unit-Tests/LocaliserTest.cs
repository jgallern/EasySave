using Core.Model;
using Core.ViewModel;
using Moq;
using Xunit;
using System.Collections.Generic;

namespace Unit_Tests
{
    public class LocalizerTests
    {
        private readonly Mock<ITranslationManager> _mockTranslationManager;
        private readonly Localizer _localizer;

        public LocalizerTests()
        {
            _mockTranslationManager = new Mock<ITranslationManager>();
            _localizer = new Localizer(_mockTranslationManager.Object);
        }

        [Fact]
        public void Indexer_Returns_Translation_From_TranslationManager()
        {
            // Arrange
            string key = "exit";
            string expected = "Quitter";
            _mockTranslationManager.Setup(tm => tm.GetTranslation(key)).Returns(expected);

            // Act
            var result = _localizer[key];

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ChangeLanguage_Calls_TranslationManager()
        {
            // Arrange
            string language = "fr";

            // Act
            _localizer.ChangeLanguage(language);

            // Assert
            _mockTranslationManager.Verify(tm => tm.ChangeLanguage(language), Times.Once);
        }

        [Fact]
        public void GetCurrentLanguage_Returns_Value_From_TranslationManager()
        {
            // Arrange
            string expected = "en";
            _mockTranslationManager.Setup(tm => tm.GetCurrentLanguage()).Returns(expected);

            // Act
            var result = _localizer.GetCurrentLanguage();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetAvailableLanguages_Returns_List_From_TranslationManager()
        {
            // Arrange
            var expected = new List<string> { "en", "fr" };
            _mockTranslationManager.Setup(tm => tm.GetAvailableLanguages()).Returns(expected);

            // Act
            var result = _localizer.GetAvailableLanguages();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Instance_Returns_Singleton_Instance()
        {
            // Act
            var instance1 = Localizer.Instance;
            var instance2 = Localizer.Instance;

            // Assert
            Assert.Same(instance1, instance2);
        }
    }
}
