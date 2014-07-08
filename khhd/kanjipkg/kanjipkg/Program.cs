using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kanjipkg
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("khhd kanjidat");
                Console.WriteLine("pujia.kris");
                Console.WriteLine("unpack:kanjidat -u file");
                Console.WriteLine("repack:kanjidat -r folder repackfile");
                return;
            }

            if (args[0] == "-u")
            {
                try
                {
                    kanjidat.unpack(args[1]);
                    Console.WriteLine("导出完毕");
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (args[0] == "-r")
            {
                try
                {
                    kanjidat.repack(args[1],args[2]);
                    Console.WriteLine("导出完毕");
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
