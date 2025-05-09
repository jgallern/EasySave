using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackUp;
using System.Transactions;
namespace BackUp.ViewModel
{
    public class Localizer : ILocalizer
    {
        public readonly TranslationManager _translationManager;
        public Localizer(TranslationManager translationManager)
        {
            _translationManager = translationManager;
        }

        public string this[string key] => _translationManager.GetTranslation(key);
    }
}
