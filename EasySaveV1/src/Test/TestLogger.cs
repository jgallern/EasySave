using BackUp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class TestLogger
{
    public static void Run()
    {
        Console.WriteLine("Test Logger :");
        ILogger log = Logger.Instance;

        Console.WriteLine("Test Add DailyLogs :");
        Dictionary<string, object> json_content = new Dictionary<string, object>
        {
            { "JobName", "Job Test 2" },
            { "SourcePath", "D:\\projet_2\\TEST\\source1234\\fichiertxt_2.txt" },
            { "TargetPath", "E:\\SAVE\\TEST\\source1234\\fichiertxt_2.txt" },
            { "FileSize", 104592 },
            { "FileTransferTime", 32.121 },
            { "Timestamp", "17 / 12 / 2020 17:06:50" }
        };
        log.AddLogInfo(LogType.Daily,  json_content);
        Console.WriteLine($"You can find your daily log in {log.GetDailyLogDirectory()}");

        //Add status logF
        Console.WriteLine("Test Add StatusLog :");
        Dictionary<string, object> json_content1 = new Dictionary<string, object>
        {
            { "JobName", "Job Test 2" },
            { "TotalSize", 104592 },
            { "TotalFileTransferTime", 32.121 },
            { "Timestamp", "17 / 12 / 2020 17:06:50" }
        };
        log.AddLogInfo(LogType.Status, json_content1);
        Console.WriteLine($"You can find your status log in {Logger.GetLogDirectory()}");
    }
}