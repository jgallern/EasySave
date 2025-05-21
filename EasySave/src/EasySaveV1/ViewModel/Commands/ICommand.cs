namespace BackUp.ViewModel
{
	public interface ICommand // Possible de directement utiliser l'interface ICommand de Windows.Input
	{
        
        bool CanExecute(object parameter);
        void Execute(object parameter);
        event EventHandler CanExecuteChanged;
    }
}