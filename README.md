# FR documentation du projet FISE_A3_SE_GALLERNE
EasySave est une application de gestion de sauvegardes développée en C# en .NET 8.0 avec une architecture orientée MVVM. 
L'utilisation de .NET 8.0 ganranti une version stable et maintenue de .NET permettant de garantir la fiabilité et l'évolutivité de l'application au fure et à mesure des nouvelles versions. 
Ce projet est conçu pour évoluer par étapes, en intégrant progressivement une interface graphique (WPF) tout en maintenant une séparation claire des responsabilités et une robustesse du code. L'intégration continue dans ce projet collaboratif ainsi que les vérifications mises en place pour garantir la fiabilité et la robustesse du code déployer. 

## Objectif du projet
EasySave a pour vocation de fournir un outil flexible, sécurisé et évolutif de sauvegarde de fichiers, avec des fonctionnalités avancées pour l'utilisateur :
  - la création et l'enregistrement de jobs dans la configuration pour pouvoir les réutilisés.
  - la modification et la suppression des jobs de la configuration.
  - l'exécution de jobs:
    - la copie de l'intégralité des fichiers dans un dossier source et le transfert vers un dossier de destination (existant ou non)
    - le transfert des fichiers du dossier source vers le dossier de destination si ceux-ci ont eut des modifications.
  - Sécurité des transferts.
  - Journalisation complète des opérations (log journaliers avec les details pour chaque fichier, les logs de status permettant de remonter l'état des sauvegardes et les potentielles erreurs).
  - L'utilisation de l'application en différentes langues (actuellement francais et anglais).



| Fonction                          | Version 1.0         | Version 1.1              | Version 2.0                   | Version 3.0                                  |
|----------------------------------|---------------------|--------------------------|-------------------------------|----------------------------------------------|
| Graphical user interface         | Console             | Console                  | WPF                           | WPF                                          |
| Multi-language                   | English and French  | English and French       | English and French            | English and French                           |
| Backup jobs                      | Limited to 5        | Limited to 5             | Unlimited                     | Unlimited                                    |
| Daily log file                   | Yes in JSON only    | Yes (JSON, XML)          | Yes (JSON, XML)               | Yes (JSON, XML + encryption time)            |
| User can pause one or more jobs | No                  | No                       | No                            | Yes                                          |
| Status file                      | Yes                 | Yes                      | Yes                           | Yes                                          |
| Type of backup operation         | Concurrent or sequential | Concurrent or sequential | Concurrent or sequential | Parallel                                     |
| Stop if a software detected      | No                  | No                       | Yes (impossible to start another job) | Yes (all current transfers are stopped) |
| Use of CryptoSoft encryption     | No                  | No                       | Yes                           | Yes                                          |
| Priority file management         | No                  | No                       | No                            | Yes, with other tasks in queue               |
| Simultaneous backups for large files | No             | No                       | No                            | Yes                                          |
| Remote display interface         | No                  | No                       | No                            | Yes                                          |
| Single-instance application      | No                  | No                       | No                            | Yes                                          |
| Monitoring of network load       | No                  | No                       | No                            | Automatic reduction of flows                 |


## Structure de l'application
Cette application suit le modèle MVVM. Celle ci se structure en objects, pour des informations plus détaillés sur le fonctionnement de l'application & la communication etnre les différents objets. La structure du projet est la suivante :

### Point de démarrage du programme
Le point de démarrage du programme "program.cs" va permettre d'instancier la communication avec la configuration de l'application pour l'utilisateur. Celle-ci va alors utiliser la commande de redirection de l'"AppController" pour rediriger l'utilisateur vers le menu principal.

### View 
On retrouve dans la vue l'intégralité des interfaces utilisateurs séparés dans divers classes. On y retrouve les classes suivantes:
- **IView:** Celle-ci représente l'interface sur laquelle se bases l'ensemble des classes de View. 
- **MenuView:** Il s'agit de la vue principale proposé à l'utilisateur, celle-ci permet de naviguer entre les fonctionnalités de l'application grace aux commandes de redirections.
- **MenuItem:** Il ne s'agit pas d'une vue a proprement parler, néamoins celle ci permet de créer des objets etant un menu en liste d'affichage avec des lien avec des commandes.
- **SettingsView:** Il s'agit de la vue permettant de changer de langue, celle ci affiche à l'utilisateur la liste des langues disponibles et permet à l'utilisateur de sélectionner une langue pour l'affichage.
- **ExecuteBackUpView:** Cette classe permet l'affichage de l'interface d'execution des barckups. Celle-ci permet donc à l'utilisateur d'entrée les jobs qu'il souhaite executé et permet de lui afficher comment ceux-ci se sont déroulés. Il communique via la commande d'execution des backups.
- **ManageBackUpView:** Cette classe est l'interface humain machine qui permet à l'utilisateur la gestion des backups. cela permet d'accèder aux backups sauvegardés dans la configuration des backups de l'utilisateur et de pouvoir la modifier.

### ViewModel
La partie du ViewModel va permettre de récupérer les informations de l'interface grâce aux commandes afin de pouvoir les traités, appeler la logique métier si besoin et pouvoir renvoyer les informations dynamiques à l'interface utilisateur.
Celui-ci est séparer en différents dossiers permettant la bonne organisation des fonctionnalités de l'application. Chacune des classes ont des interfaces permettants la liaison avec la vue et appel les interfaces des Modèles.
  - **Services:**
  On retrouve dans ce dossier l'ensemble des services étant nécéssaires pour les IHM de l'application. 
    - **IAppController & AppControlelr**: Le service "AppController" va permettre la logique 
    - **ILocalizer & Localizer**: Il permet d'interroger et de communiquer avec le manager de langue présent dans le modèle et va être inclus dans l'AppController pour pouvoir permettre à l'ensemble des vues de traduire les éléments affichés.
  - **Command:**
  On retrouve ici la logique permettant l'utilisation de commandes d'actions permettant de faire appel à des fonctionnalités présentent dans les différents services de ViewModel.
    - **ICommand:** Interface utiliser pour initialiser divers commandes.
    - **RelayCommand:** Il s'agit de l'implémentation des commandes permettant d'invoquer l'exécution d'actions suivant les fonctions d'entrée.
  - **Settings:**
    - **ISettingsViewModel & SettingsViewModel:** Il s'agit de l'interface & de l'implémentation d'appel aux différentes foncionnalités de settings de l'application (pour l'instant simplement la langue). Celle ci contient les commandes liés aux settings tel que le changement de langue ainsi que les fonctionnalités de traduction d'éléments qui est déjà présent dans l'AppController pour une meilleure centralisation.
  - **BackUp:**
  On retrouve ici les différentes fonctionnalités liées aux backups. On y retrouve deux éléments:
    - **IExecuteBackUpServices & ExecuteBackUpServices:** Il s'agit ici d'avoir la commande d'execution des backups disponible via l'interface, la transmission des résultats, ainsi que la logique de traitement des entrées utilisateurs et de l'appel à la logique métier.
    - **IManageBackUpServices & ManageBackUpServices:** On y retrouve les commandes permettant la gestion des backups (créer, modifier, supprimer) ainsi que les éléments nécéssaires à la transmission des informations essentielles à la vue et à l'appel de la logique métier.

### Model
On y retrouve les différentes logiques métier pour l'execution de l'application. Pour meixu comprendre les communications entre les différentes classes, vous pouvez vous référer au diagramme de classe présent dans le dossier "UML" du projet.
  - **IJobs & BackUpJobs:** Il s'agit là de l'abstraction et de l'implémentation de la logique métier lié aux BackUps. Cela implémente les fonctions et variables permettant la gestion des backups et l'execution, appelant les autres logiques métiers nécessaires.
  - **IBackUpType & BackUpFull & BackUpDifferential:** Il s'agit la d'avoir une interface permettant l'execution des différents types de backups possibles. On y retrouve le full qui va tout transférer, ainsi que le différentiel qui permet de une sauvegarde plus "intélligente" vérifiant les changements apportés aux fichiers.
  - **ILogger & Logger: (Singleton)** Ceux-ci permettent la connexion aux fichiers de log permettant de gérer la logique autour des fichiers de logs comme DailyLog & StatusLog.
  - **ConfigManager: (Singleton)** Celui-ci permet de gérer la logique d'accès et de manipulation du fichier de configuration des jobs. Les classes de logique métier des jobs sont obligés de passer par celle-ci afin de maintenir l'accès à la configuration.
  - **ITranslationManager & TranslationManager: (Singleton)** Cette classe gère la logique autour de la traduction. Elle gère la communication avec les ressources et la configuration de la langue afin de permettre une application multilangue fiable & sécurisé.

## La configurationd de l'application 
Les fichiers de configurations de l'applications sont sauvegardés dans le dossier d'installation de l'application, on y retrouve l'"appconfig.json" qui contient les paramètres de l'application comme le language et "jobconfig.json" qui contient les jobs enregistrés par l'utilisateur. 

## Les ressources de l'application 
Dans les ressources de l'application on y retrouve les traductions dans les différentes langues afin de permettre une application multilangue. Ces fichiers sont des fichiers json contenants les traductions des différents éléments dans les langues disponibles (anglais:"en" et francais:"fr").

## Les logs de d'exécution des BackUps 
Le **StatusLog** est un fichier généraliste qui rapporte des informations globales sur l'exécution de chaque job :
  - Nom du job
  - Heure d'exécution
  - Durée d'exécution
  - État du job (succès ou erreur)

C'est également dans ce fichier que sont enregistrées les erreurs survenues lors des sauvegardes.  
Il est au format **JSON**, et chaque objet représente un job exécuté. Exemple :
```json
{
	"Name": "test4",
	"JobTime": 46,
	"Result": "Job Succeed!",
	"TimeStamp": "5/14/2025 21:20:13"
}{
	"Name": "test3",
	"JobTime": 85,
	"Result": "Erreur pendant le backup complet : The value cannot be an empty string. (Parameter \u0027path\u0027)",
	"TimeStamp": "5/14/2025 22:18:30"
}
```
! Celui-ci doit être vidé de temps en temps afin d'évité d'avoir un trop grand nombre de log qui pourrait entrainer réduire l'ergonomie de navigation dans les logs.  

Le **dossier DailyLog** va alors centralisé l'ensemble des détails d'executions des jobs pour chaque jours ![DailyLogs](./pictures/DailyLog_directory.png)
  Les fichiers **Dailylog** contiennent les informations a propos des fichiers transférés dans les backlogs. Cela donne les détails sur le transfert de chaque fichier permettant ainsi d'avoir un suivis propre de chaque sauvegardes. Il se représente comme ceci:
```json
{
	"FileName": "test1",
	"SourcePath": "C:\\Users\\Florian\\test\\donn\u00E9e1.wav",
	"TargetPath": "C:\\Users\\Florian\\test1\\donn\u00E9e1.wav",
	"FileSize": 882058,
	"FileTransferTime": 3,
	"TimeStamp": "5/15/2025 16:01:23"
}{
	"FileName": "test1",
	"SourcePath": "C:\\Users\\Florian\\test\\Exemple_extraction_donnes_csv_et_tracer.ipynb",
	"TargetPath": "C:\\Users\\Florian\\test1\\Exemple_extraction_donnes_csv_et_tracer.ipynb",
	"FileSize": 3557,
	"FileTransferTime": 1,
	"TimeStamp": "5/15/2025 16:01:23"
}{
	"FileName": "testf",
	"SourcePath": "C:\\Users\\Florian\\Documents\\CESI cours\\stage\\Mini-Book-de-Stages-2024-2025.pdf",
	"TargetPath": "C:\\Users\\Florian\\Documents\\CESI cours\\Mini-Book-de-Stages-2024-2025.pdf",
	"FileSize": 8181206,
	"FileTransferTime": 31,
	"TimeStamp": "5/15/2025 16:03:25"
}
```