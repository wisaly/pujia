using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLJM61076
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("BLJM61076解包及文本");
            Console.WriteLine("pujia.kris");

            if (args.Length < 2)
            {
                Console.WriteLine("解包（文件）： BLJM61076 [-n] -u x:\\data.dat");
                Console.WriteLine("封包（目录）： BLJM61076 [-n] -r x:\\data");
                return;
            }

            if (args[0] == "-n")
            {
                dat.setAlign(false);
            }

            if (args[1] == "-u")
            {
                try
                {
                    dat.unpack(args[2]);
                    Console.WriteLine("解包完毕");
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (args[1] == "-r")
            {
                try
                {
                    dat.repack(args[2]);
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
