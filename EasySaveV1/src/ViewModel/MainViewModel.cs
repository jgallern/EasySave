using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackUp.Model;

namespace BackUp.ViewModel
{
    public class MainViewModel
    {
        private readonly ILocalizer _localizer;

        public ICommand ChangeLanguageCommand { get; }

        public MainViewModel(ILocalizer localizer)
        {
            _localizer = localizer;

            ChangeLanguageCommand = new RelayCommand<string>(lang =>
            {
                _localizer.ChangeLanguage(lang);
            });
        }

        public List<string> GetAvailableLanguages() => _localizer.GetAvailableLanguages();
        public string GetCurrentLanguage() => _localizer.GetCurrentLanguage();
        public string GetTranslation(string key) => _localizer[key];
    }

}
