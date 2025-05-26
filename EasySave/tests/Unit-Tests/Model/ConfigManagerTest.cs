using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Core.Model.Managers;
using System.IO;

namespace Unit_Tests
{
    public class ConfigManagerTest
    {
        [Fact]
        public void Main()
        {

            string basePath = AppContext.BaseDirectory;
            string _tempEnvPath = Path.Combine(basePath, "env");
            // Crée un appconfig.json de test
            string _appConfigPath = Path.Combine(_tempEnvPath, "appconfig.json");

            BackUpJob bcktest = new BackUpJob("test1", "c:/users/jg/test", "c:/users/jg/test1", false, true);    
            //bcktest.CreateJob();

            bcktest.Run();
        }
    }
}
