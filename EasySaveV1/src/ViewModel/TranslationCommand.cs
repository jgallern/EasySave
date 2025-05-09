using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUp.ViewModel
{
    public class TextTranslationCommand : ICommand
    {
        private readonly TextTranslationCommand _text_translation;
        public TextTranslationCommand(TextTranslationCommand text_translation)
        {
            _text_translation = text_translation;
        }
        public string GetTranslation(string key)
        {

        }
        public void Run()
        {
            // Afficher un message traduit dans la console
            Console.WriteLine(GetTranslation(key));
        }

    }
}
