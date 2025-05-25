using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using System.IO;

namespace Unit_Tests
{
    public class ConfigManagerTest
    {
        [Fact]
        public void Main()
        {

            string basePath = Path.Combine(Path.GetTempPath(), "easysave-test");
            string sourcePath1 = Path.Combine(basePath, "testsource1");
            string destPath1 = Path.Combine(basePath, "testdest1");

            Directory.CreateDirectory(sourcePath1);
            Directory.CreateDirectory(destPath1);

            BackUpJob bcktest = new BackUpJob("test1", sourcePath1, destPath1, false, false);    
            bcktest.CreateJob();

            string sourcePath2 = Path.Combine(basePath, "testsource2");
            string destPath2 = Path.Combine(basePath, "testdest2");

            Directory.CreateDirectory(sourcePath2);
            Directory.CreateDirectory(destPath2);

            BackUpJob bcktest1 = new BackUpJob("test2", sourcePath2, destPath2, false, false);    
            bcktest1.CreateJob();    
            bcktest.DeleteJob();
        }
    }
}
