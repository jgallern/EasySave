using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel.Services
{
    public interface IFileDialogService
    {
        string OpenFile(string filter = "All files (*.*)|*.*");
        string SaveFile(string filter = "All files (*.*)|*.*");
        string SelectFolder();
    }

}
