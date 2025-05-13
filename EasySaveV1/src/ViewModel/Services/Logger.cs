namespace BackUp.ViewModel
{
    public class ConsoleErrorService : IErrorService
    {
        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Erreur] {message}");
            Console.ResetColor();
        }

        public void LogError(Exception ex)
        {
            // Ici on pourrait écrire dans un fichier log
            Console.WriteLine($"[Log] {ex.GetType().Name}: {ex.Message}");
        }
    }

}