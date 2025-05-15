using BackUp.Model;
using System.Runtime.InteropServices;



public class TestBackUpFull
{
    public static void Run()
    {
        string folder = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
        
        ConfigManager.Initialize();
        BackUpJob bcktest = new BackUpJob("test", @"C:\Users\Florian\test", @"C:\Users\Florian\test1", false);

        bcktest.Run();
    }
}