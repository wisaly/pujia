using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mazetower
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("迷宫塔路解包封包程序");
            Console.WriteLine("pujia.kris");
            
            if (args.Length != 2)
            {
                Console.WriteLine("解包（文件）： mazetower -u x:\\data.dat");
                Console.WriteLine("封包（目录）： mazetower -r x:\\data");
                return;
            }

            if (args[0] == "-u")
            {
                try
                {
                    dat.unpack(args[1]);
                    Console.WriteLine("解包完毕");
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
                    dat.repack(args[1]);
                    Console.WriteLine("封包完毕");
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
