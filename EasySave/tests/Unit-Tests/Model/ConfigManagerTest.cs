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
            BackUpJob bcktest = new BackUpJob("test", "c:/users/jg/test1", "c:/users/jg/test", false, false);    
            bcktest.CreateJob();    
            BackUpJob bcktest1 = new BackUpJob("test1", "c:/users/jg/test1", "c:/users/jg/test1", false, false);    
            bcktest1.CreateJob();    
            bcktest.DeleteJob();
        }
    }
}
