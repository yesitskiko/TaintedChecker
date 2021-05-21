using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaintedChecker
{
    public class MyConsole
    {
        public static void Error(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Warn(string Message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(Message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Print(string Message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Message);
        }
    }

    class Program
    {
        static bool IsTainted = false;
        static void Main(string[] args)
        {
            Console.Title = "Dimenzia | Tainted Checker";
            MainThread();
            for (;;)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }
        static async void MainThread()
        {
            MyConsole.Warn("[ + ] Welcome to Tainted Checker.");
            MyConsole.Warn("[ + ] This program was created as a part of the Dimenzia series.");
            MyConsole.Warn("[ + ] It is completely open-sourced at our Github.");

            string RobloxPath = String.Format("C:\\Users\\{0}\\AppData\\Local\\Roblox", Environment.UserName);
            if (!Directory.Exists(RobloxPath))
            {
                /* Todo:
                    - Open a file dialog where the user can select their path.
                */
                MyConsole.Error("Couldn't locate Roblox path, try to reinstall Roblox or try again in a few minutes.");
                return;
            }
            MyConsole.Print("[ + ] Located Roblox path.");
            if (Directory.Exists(RobloxPath + "\\logs") && Directory.Exists(RobloxPath + "\\logs\\archive"))
            {
                MyConsole.Print("[ + ] Located Logs directory.");
                foreach(string FileName in Directory.GetFiles(RobloxPath + "\\logs\\archive"))
                {
                    string[] ParsedFileName = FileName.Split('.');
                    string[] ParsedPath     = FileName.Split('\\');
                    // -> First thing I thought of, so I implemented it.
                    if (ParsedFileName[1].ToString() == "ini")
                    {
                        MyConsole.Print("[ + ] Checking file -> " + ParsedPath[ParsedPath.Length - 1]);
                        foreach (var FileLine in File.ReadAllLines(FileName))
                        {
                            string[] ParsedLineContent = FileLine.ToString().Split('=');
                            if(ParsedLineContent.Length > 1 && ParsedLineContent[0] == "IsTainted")
                            {
                                if(ParsedLineContent[1] == "true")
                                {
                                    IsTainted = true;
                                }
                            }
                        }   
                    }
                }
                if (IsTainted == true)
                {
                    MyConsole.Error("[ + ] The scan shows that your device has been Tainted.");
                    MyConsole.Error("[ + ] Don't panic, most Tainted reports don't go through unless there is an upcoming banwave.");
                }
                return;
            }
            MyConsole.Error("There was an issue locating the logs directory.\nAttempt to fix this issue by reinstalling Roblox and playing the game a few times.\nOther than that there is no permanent fix for this.");
            return;
        }
    }
}
