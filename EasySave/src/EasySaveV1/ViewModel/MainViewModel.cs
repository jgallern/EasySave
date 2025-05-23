using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // Événement obligatoire pour le binding
        public event PropertyChangedEventHandler PropertyChanged;

        // Méthode utilitaire pour déclencher l'événement
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Exemple de propriété liée à l'interface
        private string _titre;
        public string Titre
        {
            get => _titre;
            set
            {
                if (_titre != value)
                {
                    _titre = value;
                    OnPropertyChanged(); // Notifie la vue
                }
            }
        }

        // Constructeur
        public MainViewModel()
        {
            Titre = "Bienvenue dans EasySave !";
        }
    }
}
