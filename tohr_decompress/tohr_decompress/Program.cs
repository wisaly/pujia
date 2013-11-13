using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace tohr_decompress
{
    class Program
    {
        static int totalFileCount = 0;
        static int successFileCount = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("心灵传说解压缩工具");
            Console.WriteLine("pujia.kris");

            if (args.Length >= 1 && args[0] == "-d")
            {
                Console.WriteLine("将清除所有当前目录和子目录中的*.dec文件");
                delDir(Directory.GetCurrentDirectory());
                Console.WriteLine(
                    string.Format("共处理文件{0}个",
                    totalFileCount, successFileCount));
                return;
            }

            Console.WriteLine("将解压所有当前目录和子目录中的*.dat;*.bin文件");
            decompressDir(Directory.GetCurrentDirectory());
            Console.WriteLine(
                string.Format("共处理文件{0}个，成功{1}个，失败{2}",
                totalFileCount, successFileCount, totalFileCount - successFileCount));
        }

        static void decompressDir(string dir)
        {
            DirectoryInfo theFolder = new DirectoryInfo(dir);

            FileInfo[] fileInfo = theFolder.GetFiles();
            foreach (FileInfo file in fileInfo)
            {
                if (file.Extension.ToLower() == ".dat"
                    || file.Extension.ToLower() == ".bin")
                {
                    string decompressFile = file.FullName + ".dec";
                    Console.Write("正在解压:{0}...", file.FullName);
                    
                    try
                    {
                        totalFileCount++;
                        dat.decompress(file.FullName, decompressFile);
                        successFileCount++;
                        Console.WriteLine("成功.");
                    }
                    catch (System.Exception )
                    {
                        Console.WriteLine("失败.");
                    }
                }
            }

            DirectoryInfo[] subDirs = theFolder.GetDirectories();

            foreach (DirectoryInfo subdir in subDirs)
            {
                decompressDir(subdir.FullName);
            }
        }

        static void delDir(string dir)
        {
            DirectoryInfo theFolder = new DirectoryInfo(dir);

            FileInfo[] fileInfo = theFolder.GetFiles();
            foreach (FileInfo file in fileInfo)
            {
                if (file.Extension.ToLower() == ".dec")
                {
                    totalFileCount++;
                    Console.WriteLine("正在删除文件:{0}", file.FullName);
                    File.Delete(file.FullName);
                }
            }

            DirectoryInfo[] subDirs = theFolder.GetDirectories();

            foreach (DirectoryInfo subdir in subDirs)
            {
                delDir(subdir.FullName);
            }
        }
    }
}
