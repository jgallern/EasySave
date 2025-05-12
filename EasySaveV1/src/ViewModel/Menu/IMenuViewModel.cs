namespace BackUp.ViewModel
{
    public interface IMenuViewModel
    {
        IReadOnlyList<MenuItem> Items { get; }
        string CurrentLanguage { get; }
        
        string SelectLabel(string id);
        void RefreshMenu();
        void NavigateToSettings();

    }
}
