using System;

namespace BackUp.Model
{
    public sealed class DailyLog
    {
        public list filelist { get; set; }
        public string logpath { get; set; }

        public DailyLog getInstance()
        {

        }

        public void createDailyLog()
        {

        }

        public void addFileToLog(IFile)
        {

        }
    }
}


{
    public sealed class DailyLog
    {
        // Instance unique, thread-safe et initialisée à la demande
        private static readonly Lazy<DailyLog> instance = new Lazy<DailyLog>(() => new DailyLog());

        // Propriété publique d’accès global à l’instance
        public static DailyLog Instance => instance.Value;

        // Constructeur privé : empêche la création externe d’instances
        private DailyLog()
        {
            // <<< À MODIFIER : initialisation personnalisée ici
        }

        // Méthodes et propriétés métier
       


    }
}