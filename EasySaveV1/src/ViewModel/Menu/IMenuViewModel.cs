namespace BackUp.ViewModel
{
    public interface IMenuViewModel
    {
        IReadOnlyList<MenuItem> Items { get; }
        string CurrentLanguage { get; }
        string SelectLanguageLabel { get; }

        void Refresh();
        void NavigateToSettings();

    }
}
