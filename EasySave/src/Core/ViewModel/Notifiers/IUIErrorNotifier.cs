using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel.Notifiers
{
    public interface IUIErrorNotifier
    {
        void ShowError(string message);
        void ShowSuccess(string message);
    }
}
