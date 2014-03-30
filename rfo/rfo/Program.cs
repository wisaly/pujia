using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("符文工房：海洋 解包");
            Console.WriteLine("pujia.kris");

            if (args.Length < 2)
            {
                Console.WriteLine("解包（文件）： rfo -u x:\\RFF2");
                Console.WriteLine("封包（目录）： rfo -r x:\\RFF2");
                return;
            }

            if (args[0] == "-u")
            {
                try
                {
                    rff.unpack(args[1]);
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
                    rff.repack(args[1]);
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
