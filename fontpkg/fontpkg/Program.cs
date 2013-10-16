using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fontpkg
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }
            try
            {
                foreach (string input in args)
                {
                    Pkg.Unpack(input);
                }
            }
            catch
            {
                Console.Write("不知道为啥，出错了。。。");
            }
        }
    }
}
