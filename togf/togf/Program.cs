using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace togf
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("神恩传说F 文本导出导入");
            Console.WriteLine("scs <---> txt");
            Console.WriteLine("pujia.kris");

            if (args.Length != 2)
            {
                Console.WriteLine("导出（目录）： togf -e x:\\data");
                Console.WriteLine("导入（目录）： togf -i x:\\data");
                return;
            }

            if (args[0] == "-e")
            {
                try
                {
                    scs.export(args[1]);
                    Console.WriteLine("导出完毕");
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (args[0] == "-i")
            {
                try
                {
                    scs.import(args[1]);
                    Console.WriteLine("导入完毕");
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("\a");
            }
        }
    }
}
