using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUp.ViewModel
{
    public class Localizer : ILocalizer
    {
        public readonly IStringLocalizer _stringLocalizer;
        public Localizer(IStringLocalizer<Resources.Strings> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        public string this[string key] => _stringLocalizer[key];
    }
}
