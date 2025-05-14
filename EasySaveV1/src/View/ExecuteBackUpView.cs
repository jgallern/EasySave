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
                Console.WriteLine(_app.Translate("job_exec_title"));
                Console.Write(_app.Translate("enter_jobs"));

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
                    _vm.ResultMessage = _app.Translate("invalid_input");

                if (!string.IsNullOrWhiteSpace(_vm.ResultMessage))
                {
                    if (_vm.ResultMessage.Contains(_app.Translate("log_repo")))
                    {
                        Console.WriteLine(_vm.ResultMessage);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(_vm.ResultMessage);
                        Console.ResetColor();
                    }
                }
                Console.ReadKey();

            } while (true);
        }
    }
}
