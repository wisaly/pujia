using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace codinfgen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Kingdom Heart HD 1.5 Font File");
            if (args.Length < 1)
            {
                return;
            }

            string argname = args[0];
            string argval = args[1];

            if (argname == "-cod" && args.Length == 3)
            {
                if(cod.ExportFont(args[1],args[2]))
                {
                    Console.WriteLine("Done.");
                }
                else
                {
                    Console.WriteLine("Format error...");
                }
            }
            else if (argname == "-gen" && args.Length == 2)
            {
                if (cod.GenCodInf(args[1]))
                {
                    Console.WriteLine("Done.");
                }
                else
                {
                    Console.WriteLine("Format error...");
                }
            }
        }
    }
}
