using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUp.ViewModel
{
    public class MainViewModel
    {
        private readonly TranslationManager _translationManager;

        public ICommand ChangeLanguageCommand { get; }

        public MainViewModel(TranslationManager translationManager)
        {
            _translationManager = translationManager;

            ChangeLanguageCommand = new RelayCommand<string>(lang =>
            {
                _translationManager.LoadTranslations(lang);
                _translationManager.SaveAppConfig();
            });
        }

        public List<string> GetAvailableLanguages() => _translationManager.GetAvailableLanguages();
        public void ChangeLanguage(string  language) => _translationManager.ChangeLanguage(language);
        public string GetCurrentLanguage() => _translationManager.GetCurrentLanguage();
        public string GetTranslation(string key) => _translationManager.GetTranslation(key);
    }
}
