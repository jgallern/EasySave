using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace Unit_Tests
{
    public class JobConfigManagerTest
    {
        [Fact]
        public void Main()
        {

            //BackUpJob bcktest = new BackUpJob("test", "c:/users/jg/test1", "c:/users/jg/test", false);
            //try
            //{
            //    bcktest.CreateJob();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}
            BackUpJob bcktest1 = new BackUpJob("test1", "c:/users/jg/test", "c:/users/jg/test1", false, true);
            try
            {
                bcktest1.CreateJob();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() );
            }
            //bcktest.DeleteJob();
            bcktest1.Run();
        }
    }
}
