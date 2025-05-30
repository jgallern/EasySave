using System;
using System.Windows.Threading;
using System.Windows;

namespace Core.ViewModel.Services
{
    public class WpfDispatcher : IDispatcher
    {
        private readonly Dispatcher _dispatcher;

        public WpfDispatcher()
        {
            _dispatcher = Application.Current.Dispatcher; 

        }

        public void Invoke(Action action)
        {
            if (_dispatcher.CheckAccess())
                action();
            else
                _dispatcher.Invoke(action);
        }
    }
}
