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
            do
            {
                Console.Clear();
                Console.Write("Entrez les jobs à exécuter > ");
                string input = Console.ReadLine() ?? "";

                // Exécute la commande
                if (_vm.ExecuteBackUpCommand.CanExecute(input))
                    _vm.ExecuteBackUpCommand.Execute(input);
                else
                    _vm.ResultMessage = "❌ Entrée invalide !";

                // Affiche le résultat
                Console.WriteLine(_vm.ResultMessage);
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();

            } while (true);
        }
    }
}
