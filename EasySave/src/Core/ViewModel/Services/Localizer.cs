using Core.Model;

namespace Core.ViewModel
{
    public class Localizer : ILocalizer
    {
        private static Localizer _instance;
        private static readonly object _lock = new object();
        private readonly ITranslationManager _translationManager;

        public Localizer() : this(TranslationManager.Instance) { }
        public Localizer(ITranslationManager translationManager)
        {
            _translationManager = translationManager;
        }

        public static Localizer Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new Localizer();
                }
            }
        }

        public string this[string key] => _translationManager.GetTranslation(key);

        public void ChangeLanguage(string languageCode)
        {
            _translationManager.ChangeLanguage(languageCode);
        }

        public string GetCurrentLanguage()
        {
            return _translationManager.GetCurrentLanguage();
        }

        public List<string> GetAvailableLanguages()
        {
            return _translationManager.GetAvailableLanguages();
        }
    }
}
