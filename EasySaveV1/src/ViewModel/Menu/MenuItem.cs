namespace BackUp.ViewModel
{
    public class MenuItem
    {
        public string Label { get; }
        public Action Action { get; }

        public MenuItem(string label, Action action)
        {
            Label = label;
            Action = action;
        }
    }
}