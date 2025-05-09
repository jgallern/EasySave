using View;
using BackUp.ViewModel;
using Microsoft.Extensions.Localization;

public class MainView : IView
{
    private readonly ILocalizer _localizer;

    public MainView(ILocalizer localizer)
    {
        _localizer = localizer;
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("================ EasySave ================");
            Console.WriteLine("Welcome! You can execute these diferent functionalities (enter the number e.g. '1'):");
            Console.WriteLine($"1. {_localizer["manage_jobs"]}");
            Console.WriteLine($"2. {_localizer["execute_backup"]}");
            Console.WriteLine($"3. Version");
            Console.WriteLine($"4. {_localizer["exit"]}");
            Console.Write("> ");
            
            string choice = Console.ReadLine();
            Console.WriteLine($"DEBUG: you entered -> '{choice}'");
            switch (choice)
            {
                case "1":
                    Console.WriteLine("case 1");
                    Console.ReadLine(); // juste pour test
                    // Sinon le while True relance le clear
                    // _viewModel.ManageBackupJobCommand.Execute(null);
                    // Print all jobs name:
                    //      Create a job
                    //      Modify a job
                    //      Delete a job
                    //      View details job
                    //          Modify job
                    //          Delete job
                    //          exit
                    //      exit
                    break;
                case "2":
                    Console.WriteLine("case 2");
                    Console.ReadLine(); // juste pour test
                    // _viewModel.ExecuteBackupJobCommand.Execute(null);
                    //"Entrez le(s) numéro(s) de job (ex: 1-3 ou 2,3,4 ou *) :"
                    break;
                case "3"://Translation
                    Console.WriteLine("case 3");
                    Console.ReadLine(); // juste pour test
                    break;
                case "4"://Exit
                    Console.WriteLine("case 4");
                    Console.ReadLine(); // juste pour test
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Choix invalide. Appuyez sur Entrée.");
                    Console.ReadLine();
                    break;
            }
        }
    }
}
