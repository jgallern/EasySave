using BackUp;
using System.Collections.Generic;

namespace BackUp.ViewModel
{
    public class Localizer : ILocalizer
    {
        private readonly TranslationManager _translationManager;

        public Localizer(TranslationManager translationManager)
        {
            _translationManager = translationManager;
        }

        public string this[string key] => _translationManager.GetTranslation(key);

        public void ChangeLanguage(string languageCode)
        {
            _translationManager.ChangeLanguage(languageCode);
        }

        public List<string> GetAvailableLanguages()
        {
            return _translationManager.GetAvailableLanguages();
        }
    }
}
