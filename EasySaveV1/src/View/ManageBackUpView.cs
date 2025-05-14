using BackUp.View;
using BackUp.ViewModel;

namespace BackUp.View
{

    public class ManageBackUpView : IView
    {
        private readonly ManageBackUp _vm;
        private readonly IAppController _app;

        public ManageBackUpView(ManageBackUp vm, IAppController app)
        {
            _vm = vm;
            _app = app;
        }

        public void Run()
        {
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("=== "+_app.Translate("manage_job_menu")+"===\n");

                List<KeyValuePair<int, string>> jobs = _vm.GetAllJobs();

                for (int i = 0; i < jobs.Count+2; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    if (i == jobs.Count)
                    {
                        Console.WriteLine("\n"+ _app.Translate("add_job"));
                    }
                    else if (i == jobs.Count + 1)
                    {
                        Console.WriteLine("- Supprimer un job");
                    }
                    else
                    {
                        Console.WriteLine($"{jobs[i].Key}. {jobs[i].Value}");
                    }
                    Console.ResetColor();
                }

                Console.WriteLine("\n"+ _app.Translate("arrow_instruction"));

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? jobs.Count : selectedIndex - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == jobs.Count +1) ? 0 : selectedIndex + 1;
                        break;

                    case ConsoleKey.Enter:
                        if (selectedIndex < jobs.Count)
                        {
                            int selectedJobId = jobs[selectedIndex].Key;
                            Dictionary<string, object> jobData = _vm.GetJobById(selectedJobId);

                            Console.Clear();
                            Console.WriteLine($"=== Modif job, ID : {selectedJobId} ===\n");

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

                            Console.WriteLine("\n"+_app.Translate("confirm_update"));
                            Console.ReadKey(true);
                            break;
                        }
                        else if (selectedIndex == jobs.Count)
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
                        else if (selectedIndex == jobs.Count + 1)
                        {
                            if (jobs.Count == 0)
                            {
                                Console.WriteLine("\n!! Aucun Job à supprimer");
                                Console.ReadKey(true);
                                break;
                            }

                            selectedIndex = 0;
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("==== Veuillez selectionner le Job à Supprimer ====\n");
                                for (int i = 0; i < jobs.Count; i++)
                                {
                                    if (i == selectedIndex)
                                    {
                                        Console.BackgroundColor = ConsoleColor.Gray;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                    }
                                    Console.WriteLine($"{jobs[i].Key}. {jobs[i].Value}");
                                    Console.ResetColor();
                                }

                                key = Console.ReadKey(true).Key;
                                switch (key)
                                {
                                    case ConsoleKey.UpArrow:
                                        selectedIndex = (selectedIndex == 0) ? jobs.Count : selectedIndex - 1;
                                        break;

                                    case ConsoleKey.DownArrow:
                                        selectedIndex = (selectedIndex == jobs.Count + 1) ? 0 : selectedIndex + 1;
                                        break;
                                    case ConsoleKey.Enter:
                                        Console.WriteLine(jobs[selectedIndex].Key);
                                        _vm.DeleteJob(jobs[selectedIndex].Key);
                                        key = ConsoleKey.Escape;
                                        break;
                                }
                            }while (key != ConsoleKey.Escape);
                            break;
                        }
                        else
                        {
                            break;
                        }

                        }
                } while (key != ConsoleKey.Escape) ;


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