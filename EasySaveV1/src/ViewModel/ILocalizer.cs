using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUp.ViewModel
{
    public interface ILocalizer
    {
        string this[string key] { get; }
    }
}
