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

            BackUpJob bcktest = new BackUpJob("test", "c:/users/jg/test1", "c:/users/jg/test", false, false);
            try
            {
                bcktest.CreateJob();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            BackUpJob bcktest1 = new BackUpJob("test1", "c:/users/jg/test", "c:/users/jg/test1", true, false);
            try
            {
                bcktest1.CreateJob();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() );
            }
            BackUpJob bcktest2 = new BackUpJob("testfdezaj", "c:/users/jg/tdeest1", "c:/users/jg/test", false, false);
            try
            {
                bcktest2.CreateJob();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            BackUpJob bcktest3 = new BackUpJob("testejzd,elkz1", "c:/users/jg/tdejzest", "c:/users/jg/test1", true, false);
            try
            {
                bcktest3.CreateJob();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() );
            }
            bcktest.DeleteJob();
        }
    }
}
