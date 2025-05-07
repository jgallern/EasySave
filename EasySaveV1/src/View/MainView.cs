using View;

public class MainView : IView
{
    private readonly MainViewModel _viewModel;

    public MainView(MainViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("================ EasySave ================");
            Console.WriteLine("Welcome! You can execute these diferent functionalities (enter the number e.g. '1'):")
            Console.WriteLine("1. Manage jobs");
            Console.WriteLine("2. Run jobs");
            Console.WriteLine("3. Quitter");
            Console.Write("> ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    _viewModel.ManageBackupJobCommand.Execute(null);
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
                    _viewModel.ExecuteBackupJobCommand.Execute(null);
                    //"Entrez le(s) numéro(s) de job (ex: 1-3 ou 2,3,4 ou *) :"
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Choix invalide. Appuyez sur Entrée.");
                    Console.ReadLine();
                    break;
            }
        }
    }
}
