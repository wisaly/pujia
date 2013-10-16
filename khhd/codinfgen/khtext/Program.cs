using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace khtext
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length  < 2)
            {
                return;
            }
            if (args[0] == "-e")
            {
                try
                {
                    ctdb.Export(args[1]);
                    Console.WriteLine("Done");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if (args[0] == "-i")
            {
                try
                {
                    ctdb.Import(args[1]);
                    Console.WriteLine("Done");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
