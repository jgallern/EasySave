using BackUp.ViewModel;
using System;
using BackUp.View;

namespace BackUp.View
{
    public class ExecuteBackUpView : IView
    {
        private readonly ExecuteBackUpViewModel _vm;

        public ExecuteBackUpView(ExecuteBackUpViewModel vm) 
        {
            _vm = vm;
        }

        public void Run()
        {
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.Write("Entrez les jobs à exécuter > ");
                string input = Console.ReadLine() ?? "";

                if (_vm.RunJobsCommand.CanExecute(input))
                {
                    _vm.RunJobsCommand.Execute(input);
                    Console.WriteLine("✔ Sauvegardes lancées !");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Entrée invalide. Réessayez.");
                    Console.ResetColor();
                    Console.ReadKey();
                }

            } while (true);
        }
    }


}
