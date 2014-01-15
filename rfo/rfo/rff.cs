using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly;
using System.IO;

namespace rfo
{
    class rff
    {
        static Int32 fixHeaderNLCM = 0x4E4C434D;
        public static void unpack(string input)
        {
            string inputbin = input + ".bin";
            string inputdat = input + ".dat";
            Console.WriteLine(inputbin);
            Console.WriteLine(inputdat);
            StreamEx sbin = new StreamEx(inputbin, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamEx sdat = new StreamEx(inputdat, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            string unpackdir = Path.GetFullPath(input);
            if (!Directory.Exists(unpackdir))
            {
                Directory.CreateDirectory(unpackdir);
            }

            Int64 fixedHeaderRead = sbin.ReadInt32BigEndian();
            if (fixedHeaderRead != fixHeaderNLCM)
            {
                throw new Exception("文件头不能识别");
            }

            sbin.ReadInt32BigEndian();
            sbin.ReadInt32BigEndian();
            Int32 count = sbin.ReadInt32BigEndian();
            Console.WriteLine("{0}个索引", count);
            sbin.Position = 0x38;

            for (int i = 0; i < count; i++)
            {
                Int32 length = sbin.ReadInt32BigEndian();
                sbin.Position += 4;
                Int32 offset = sbin.ReadInt32BigEndian();
                sbin.Position += 4;

                sdat.Position = offset;

                string newfile = unpackdir + "\\" + i.ToString("D5") + ".bin";
                StreamEx sNew = new StreamEx(newfile, FileMode.Create, FileAccess.Write);

                Console.WriteLine("解包文件{0}/{1}:{2} ({3}->{4})", i + 1, count, newfile, offset, length);
                
                sNew.WriteFromStream(sdat, length);
                sNew.Close();
            }
        }


        static void align(StreamEx origin)
        {
            zeroTo(origin,origin.Position + (0x800 - origin.Position % 0x800) % 0x800);
        }
        static void zeroTo(StreamEx s, Int64 offset)
        {
            Int64 len = offset - s.Position;
            byte[] b = new byte[len];
            s.Write(b);
        }

        internal static void repack(string input)
        {
            if (!Directory.Exists(input))
            {
                throw new Exception("输入参数必须是目录");
            }

            string[] inputFiles = Directory.GetFiles(input, "*.bin");


            //IEnumerable<string> orderFiles = inputFiles.OrderBy(str => int.Parse(str.Substring(str.LastIndexOf('\\') + 1, str.Length - str.LastIndexOf('\\') - 5)));
            IEnumerable<string> orderFiles = inputFiles.OrderBy(str => str);          

            Console.WriteLine("共有{0}个输入文件", inputFiles.Length);

            StreamEx sbin = new StreamEx(input + ".repack.bin", FileMode.Create, FileAccess.Write);
            StreamEx sdat = new StreamEx(input + ".repack.dat", FileMode.Create, FileAccess.Write);

            sbin.WriteInt32BigEndian(fixHeaderNLCM);
            sbin.WriteInt32BigEndian(0x38);
            sbin.WriteInt32BigEndian(0x14);
            sbin.WriteInt32BigEndian(inputFiles.Length);
            sbin.WriteInt32BigEndian(0x1);
            
            int ii = input.LastIndexOf('\\');
            sbin.WriteSimpleString(input.Substring(ii + 1,input.Length - ii - 1)+".dat",0x24);

            int i = 0;
            foreach (string file in orderFiles)
            {
                StreamEx sr = new StreamEx(file, FileMode.Open, FileAccess.Read);
                Console.WriteLine("封入文件{0}/{1}:{2} {3}->{4}", ++i, file, inputFiles.Length, sdat.Position, sr.Length);
                sbin.WriteInt32BigEndian((Int32)sr.Length);
                sbin.WriteInt32BigEndian(0);
                sbin.WriteInt32BigEndian((Int32)sdat.Position);
                sbin.WriteInt32BigEndian(0);

                sdat.WriteFromStream(sr, sr.Length);

                align(sdat);
            }

            sbin.Close();
            sdat.Close();
        }
    }
}
