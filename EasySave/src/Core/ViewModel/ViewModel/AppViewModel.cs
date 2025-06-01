using Core.Model.Interfaces;
using Core.Model.Services;
using Core.ViewModel.Notifiers;
using Core.ViewModel.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public abstract class AppViewModel : INotifyPropertyChanged
{
    protected readonly ILocalizer _localizer;
    protected static IUIErrorNotifier _notifier { get; private set; }

    public AppViewModel()
    {

    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}