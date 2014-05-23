using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace xor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("xor tool");
            Console.WriteLine("pujia.kris");
            if (args.Length < 6)
            {
                Console.WriteLine("param missing");
                Console.WriteLine("xor -k keystring -i input -o output");
                Console.WriteLine("xor -f keyfile -i input -o output");
                return;
            }

            byte[] key = null;
            if (args[0] == "-k")
            {
                key = new byte[args[1].Length];
                for (int i = 0; i < args[1].Length; i++)
                {
                    key[i] = (byte)args[1][i];
                }
            }
            else if (args[0] == "-f")
            {
                try
                {
                    FileStream fs = File.Open(args[1], FileMode.Open);
                    key = new byte[fs.Length];
                    fs.Read(key, 0, (int)fs.Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine("cannot open key file : {0}", e.Message);
                    return;
                }
            }
            else
            {
                Console.WriteLine("invalid param {0}", args[0]);
                return;
            }

            Console.WriteLine("key length : {0}", key.Length);

            if (args[2] == "-i" && args[4] == "-o")
            {
                try
                {
                    FileStream fi = File.Open(args[3], FileMode.Open);
                    FileStream fo = File.Open(args[5], FileMode.Create);

                    Console.WriteLine("input length : {0}", fi.Length);
                    Console.Write("start work : [..........]"+new string('\b',10));

                    DateTime startTime = DateTime.Now;

                    int count = 0;
                    int currentProgress = 0;
                    int k = 0;
                    while (fi.Position < fi.Length)
                    {
                        byte b = (byte)fi.ReadByte();
                        b ^= key[k];
                        k = (k + 1) % key.Length;

                        fo.WriteByte(b);

                        count++;
                        int progress = count * 10 / (int)fi.Length;
                        if (currentProgress < progress)
                        {
                            Console.Write("\b" +
                                new string('=', progress - currentProgress)
                                + ">");

                            currentProgress = progress;
                        }
                    }
                    fi.Close();
                    fo.Close();

                    TimeSpan delta = DateTime.Now - startTime;
                    Console.WriteLine("\b]");
                    Console.WriteLine("work done in : {0}s",
                        delta.TotalSeconds);
                }
                catch (Exception e)
                {
                    Console.WriteLine("cannot open file : {0}", e.Message);
                }
            }
            else
            {
                Console.WriteLine("invalid param");
            }
        }
    }
}
