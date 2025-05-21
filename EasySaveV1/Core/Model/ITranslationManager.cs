using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Core.Model
{
    public interface ITranslationManager
    {
        void ChangeLanguage(string language);
        string GetCurrentLanguage();
        string GetTranslation(string key);
        List<string> GetAvailableLanguages();
    }
}

