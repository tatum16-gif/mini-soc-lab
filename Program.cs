using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

class Program
{
    static string logPath = @"C:\Users\galva\OneDrive\Desktop\mini soc lab\logs\auth.logs";
    static HashSet<string> blockedIPs = new HashSet<string>();

    static void Main(string[] args)
    {
        Console.WriteLine("SOC Monitor Running...\n");

        MonitorLogFile();

        Console.ReadLine();
    }

    static void MonitorLogFile()
    {
        long lastSize = 0;

        while (true)
        {
            if (File.Exists(logPath))
            {
                var fileInfo = new FileInfo(logPath);

                Console.WriteLine($"Checking file... Size: {fileInfo.Length}");

                if (fileInfo.Length > lastSize)
                {
                    Console.WriteLine("New log activity detected!");

                    AnalyzeLog();

                    lastSize = fileInfo.Length;
                }
            }
            else
            {
                Console.WriteLine("Log file does not exist.");
            }

            System.Threading.Thread.Sleep(1000);
        }
    }

    static void AnalyzeLog()
    {
        Console.WriteLine("Analyzing log..."); // DEBUG LINE

        foreach (var line in File.ReadLines(logPath))
        {
            Console.WriteLine(line); // DEBUG: print every line
        }
    }

    static void TriggerAlert(string message, string ip)
    {
        Console.WriteLine($"⚠️ ALERT: {message}");

        if (!blockedIPs.Contains(ip))
        {
            blockedIPs.Add(ip);
            Console.WriteLine($"🚫 ACTION: Blocking IP {ip}");
        }
    }

    static string ExtractIP(string logLine)
    {
        int index = logLine.IndexOf("ip=");
        if (index == -1) return null;
        return logLine.Substring(index + 3).Trim();
    }
}