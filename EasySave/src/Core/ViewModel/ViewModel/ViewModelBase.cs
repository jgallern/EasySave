using Core.Model.Interfaces;
using Core.Model.Services;
using Core.ViewModel.Notifiers;
using Core.ViewModel.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    protected readonly ILocalizer _localizer;

    public ViewModelBase()
    {
        _localizer = new Localizer();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string this[string key] => _localizer[key];

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}