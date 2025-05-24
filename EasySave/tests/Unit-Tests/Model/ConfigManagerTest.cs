using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace Unit_Tests
{
    public class ConfigManagerTest
    {
        [Fact]
        public void Main()
        {

            JobConfigManager.Initialize();
            BackUpJob bcktest = new BackUpJob("test", "c:/users/jg/test1", "c:/users/jg/test", false);    
            bcktest.CreateJob();    
            BackUpJob bcktest1 = new BackUpJob("test1", "c:/users/jg/test1", "c:/users/jg/test1", false);    
            bcktest1.CreateJob();    
            bcktest.DeleteJob();
        }
    }
}
