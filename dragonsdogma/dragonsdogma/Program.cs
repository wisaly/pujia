using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dragonsdogma
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("龙之信条 文本导出导入");
            Console.WriteLine("gmd <---> txt");
            Console.WriteLine("pujia.kris");

            if (args.Length != 2)
            {
                Console.WriteLine("导出（目录）： dragonsdogma -e x:\\data");
                Console.WriteLine("导入（目录）： dragonsdogma -i x:\\data");
                return;
            }

            if (args[0] == "-e")
            {
                try
                {
                    gmd.export(args[1]);
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
                    gmd.import(args[1]);
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
