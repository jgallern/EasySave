using BackUp.View;
using BackUp.ViewModel;

namespace BackUp.View
{

    public class ManageBackUpView : IView
    {
        private readonly ManageBackUp _vm;

        public ManageBackUpView(ManageBackUp vm)
        {
            _vm = vm;
        }

        public void Run()
        {
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("=== Page de gestion des jobs ===\n");

                List<KeyValuePair<int, string>> jobs = _vm.GetAllJobs();

                for (int i = 0; i < jobs.Count+1; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    if (i == jobs.Count)
                    {
                        Console.WriteLine("➕ Ajouter un nouveau job");
                    }
                    else
                    {
                        Console.WriteLine($"{jobs[i].Key}. {jobs[i].Value}");
                    }
                    Console.ResetColor();
                }

                Console.WriteLine("\nUtilisez ↑ ↓ pour naviguer, Entrée pour modifier, Échap pour quitter.");

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? jobs.Count : selectedIndex - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == jobs.Count) ? 0 : selectedIndex + 1;
                        break;

                    case ConsoleKey.Enter:
                        if (selectedIndex < jobs.Count)
                        {
                            int selectedJobId = jobs[selectedIndex].Key;
                            Dictionary<string, object> jobData = _vm.GetJobById(selectedJobId);

                            Console.Clear();
                            Console.WriteLine($"=== Modification du job ID {selectedJobId} ===\n");

                            // Nom
                            Console.Write($"Nom du job [{jobData["Name"]}] : ");
                            string newName = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrEmpty(newName))
                                jobData["Name"] = newName;

                            // Source
                            Console.Write($"Chemin source [{jobData["SourcePath"]}] : ");
                            string newSource = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrEmpty(newSource))
                                jobData["SourcePath"] = newSource;

                            // Destination
                            Console.Write($"Chemin destination [{jobData["DestinationPath"]}] : ");
                            string newDest = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrEmpty(newDest))
                                jobData["DestinationPath"] = newDest;

                            // Différentiel
                            Console.Write($"Mode différentiel (actuel : {((bool)jobData["IsDifferential"] ? "oui" : "non")}) (o/n) : ");
                            string diffInput = Console.ReadLine()?.Trim().ToLower();
                            if (diffInput == "o" || diffInput == "oui")
                                jobData["IsDifferential"] = true;
                            else if (diffInput == "n" || diffInput == "non")
                                jobData["IsDifferential"] = false;

                            // Appeler la méthode pour mettre à jour le job dans la configuration
                            _vm.UpdateJob(selectedJobId, jobData);

                            Console.WriteLine("\n✅ Job mis à jour !");
                            Console.WriteLine("Appuyez sur une touche pour revenir...");
                            Console.ReadKey(true);
                            break;
                        }
                        else
                        {
                            if (jobs.Count == 5)
                            {
                                Console.WriteLine("nombre maximum de jobs (5) déjà atteint");
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("=== Ajout d’un nouveau job ===\n");

                                string name, sourcePath, destinationPath;
                                bool isDifferential = false;

                                // Nom du job
                                do
                                {
                                    Console.Write("Nom du job : ");
                                    name = Console.ReadLine()?.Trim();
                                } while (string.IsNullOrEmpty(name));

                                // Chemin source
                                do
                                {
                                    Console.Write("Chemin source : ");
                                    sourcePath = Console.ReadLine()?.Trim();
                                } while (string.IsNullOrEmpty(sourcePath));

                                // Chemin destination
                                do
                                {
                                    Console.Write("Chemin destination : ");
                                    destinationPath = Console.ReadLine()?.Trim();
                                } while (string.IsNullOrEmpty(destinationPath));

                                // Mode différentiel (boucle tant que réponse invalide)
                                string diffInput = "";
                                do
                                {
                                    Console.Write("Mode différentiel ? (o/n) : ");
                                    diffInput = Console.ReadLine()?.Trim().ToLower();
                                } while (diffInput != "o" && diffInput != "n" && diffInput != "oui" && diffInput != "non");

                                isDifferential = diffInput.StartsWith("o");


                                _vm.CreateJob(name, sourcePath, destinationPath, isDifferential);
                                Console.WriteLine("\n✅ Nouveau job ajouté !");
                            }
                            Console.WriteLine("Appuyez sur une touche pour revenir...");
                            Console.ReadKey(true);
                            break;
                        }
                       
                }
            } while (key != ConsoleKey.Escape);


            // Print all jobs name:
            //      Create a job
            //      Modify a job
            //      Delete a job
            //      View details job
            //          Modify job
            //          Delete job
            //          exit
            //      exit
        }
    }
}