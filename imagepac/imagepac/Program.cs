using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imagepac
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("阿斯兰战姬解包封包程序");
            Console.WriteLine("pujia.kris");

            if (args.Length != 2)
            {
                Console.WriteLine("解包（文件）： imagepac -u x:\\charasel_image.pac");
                Console.WriteLine("封包（目录）： imagepac -r x:\\charasel_image");
                return;
            }

            if (args[0] == "-u")
            {
                try
                {
                    pac.unpack(args[1]);
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
                    pac.repack(args[1]);
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
