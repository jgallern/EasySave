using BackUp.ViewModel;
using System;

namespace BackUp.View
{
    public class ExecuteBackUpView : IView
    {
        private readonly IExecuteBackUpServices _vm;
        private readonly IAppController _app;

        public ExecuteBackUpView(IAppController app, IExecuteBackUpServices vm)
        {
            _app = app;
            _vm = vm;
        }


        public void Run()
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true; // Empêche la fermeture brutale
                _app.RedirectMenuCommand.Execute(null);
            };

            do
            {
                Console.Clear();
                Console.WriteLine("=== Exécution des jobs ===");
                Console.Write("Entrez les jobs à exécuter (ou Échap pour revenir au menu) > ");

                string input = "";
                ConsoleKeyInfo key;

                while (true)
                {
                    key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.Escape)
                    {
                        _app.RedirectMenuCommand.Execute(null);
                        return;
                    }

                    if (key.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine(); // Aller à la ligne
                        break;
                    }

                    if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                        Console.Write("\b \b"); // Supprime visuellement le dernier caractère
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        input += key.KeyChar;
                        Console.Write(key.KeyChar);
                    }
                }

                // Exécute la commande
                if (_vm.ExecuteBackUpCommand.CanExecute(input))
                    _vm.ExecuteBackUpCommand.Execute(input);
                else
                    _vm.ResultMessage = "❌ Entrée invalide !";

                Console.WriteLine(_vm.ResultMessage);
                Console.WriteLine($"You can find the Log files into:");
                Console.WriteLine(_vm.LogPath);
                Console.ReadKey();

            } while (true);
        }
    }
}
