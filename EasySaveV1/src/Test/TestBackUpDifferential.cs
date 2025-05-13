using BackUp.Model;
using System.Runtime.InteropServices;



public class TestBackUpDifferential
{
    public static void Run()
    {
        string folder = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
        
        ConfigManager.Initialize(@"c:\users\Florian\test.json");
        BackUpJob bcktest = new BackUpJob("test", @"C:\Users\Florian\test", @"C:\Users\Florian\test1", true);

        bcktest.Run();
    }
}