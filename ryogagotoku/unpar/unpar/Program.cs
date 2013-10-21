using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace unpar
{
    class Program
    {
        static bool isTest = false;
        static void Main(string[] args)
        {
            if (args.Length >= 1 && args[0] == "-t")
            {
                isTest = true;
            }
            if (args.Length >= 1 && args[0] == "-d")
            {
                delDir(Directory.GetCurrentDirectory());
                return;
            }
            unpardir(Directory.GetCurrentDirectory());
        }

        static void unpardir(string dir)
        {
            DirectoryInfo theFolder = new DirectoryInfo(dir);

            FileInfo[] fileInfo = theFolder.GetFiles();
            foreach (FileInfo file in fileInfo)
            {
                if (file.Extension.ToLower() == ".par")
                {
                    ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                    psi.RedirectStandardOutput = false;
                    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    psi.UseShellExecute = false;
                    psi.Arguments = string.Format("/c \"quickbms.exe -Y par.bms.txt {0} {0}_unpar\"", file.FullName);
                    Console.WriteLine("正在解压:{0}", file.FullName);
                    if (isTest)
                    {
                        continue;
                    }
                    Process p = Process.Start(psi);
                    p.WaitForExit();
                }
            }

            DirectoryInfo[] subDirs = theFolder.GetDirectories();

            foreach (DirectoryInfo subdir in subDirs)
            {
                unpardir(subdir.FullName);
            }
        }

        static void delDir(string dir)
        {
            DirectoryInfo theFolder = new DirectoryInfo(dir);

            DirectoryInfo[] subDirs = theFolder.GetDirectories();

            foreach (DirectoryInfo subdir in subDirs)
            {
                if (subdir.Name.ToLower().EndsWith("_unpar"))
                {
                    Console.WriteLine("正在删除目录:{0}",subdir.FullName);
                    Directory.Delete(subdir.FullName, true);
                }
                else
                {
                    delDir(subdir.FullName);
                }
            }

        }
    }
}
