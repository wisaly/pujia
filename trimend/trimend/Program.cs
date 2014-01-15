using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace trimend
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("截断末尾字符");
            Console.WriteLine("pujia.kris");

            if (args.Length == 3)
            {
                string[] files = Directory.GetFiles(args[0], args[1]);

                Console.WriteLine("{0}个文件", files.Length);

                if (args[2] == "-s")
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        Console.Write("{0}/{1}:{2} ---> ", i + 1, files.Length, files[i]);
                        FileStream s = File.Open(files[i], FileMode.Open, FileAccess.ReadWrite);
                        if (s.Length > 0)
                        {
                            s.Position = s.Length - 1;
                            int c = s.ReadByte();
                            if (c == ' ')
                            {
                                s.SetLength(s.Length - 1);
                                s.Close();
                                Console.WriteLine("有空格");
                            }
                            else
                            {
                                Console.WriteLine("无空格");
                            }
                        }
                    }
                }
                else
                {
                    int n = int.Parse(args[2]);

                    for (int i = 0; i < files.Length; i++)
                    {
                        Console.WriteLine("{0}/{1}:{2}", i + 1, files.Length, files[i]);
                        FileStream s = File.Open(files[i], FileMode.Open, FileAccess.Write);
                        s.SetLength(s.Length > n ? s.Length - n : 0);
                        s.Close();
                    }
                }
            }
        }
    }
}
