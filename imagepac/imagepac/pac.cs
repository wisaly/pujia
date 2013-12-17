using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly;
using System.IO;

namespace imagepac
{
    class pac
    {
        public static void unpack(string input)
        {
            StreamEx s = new StreamEx(input, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            string unpackdir = Path.GetFullPath(input) + "_unpack";
            if (!Directory.Exists(unpackdir))
            {
                Directory.CreateDirectory(unpackdir);
            }

            Int32 fileCount = s.ReadInt32();

            Console.WriteLine("文件头解析:共有{0}个文件", fileCount);

            for (int i = 0; i < fileCount; i++)
            {
                Int32 fileLength = s.ReadInt32();
                StreamEx sNew = new StreamEx(unpackdir + "\\" + i.ToString() + ".dds", FileMode.Create, FileAccess.Write);

                Console.WriteLine("正在解包文件{0}/{1}:{2}->{3}", i + 1,fileCount, s.Position,fileLength);

                sNew.WriteFromStream(s, fileLength);
                sNew.Close();
            }
        }
        public static void repack(string input)
        {
            if (!Directory.Exists(input))
            {
                throw new Exception("输入参数必须是目录");
            }

            string[] inputFiles = Directory.GetFiles(input,"*.dds");

            inputFiles.OrderBy(str => str);

            Console.WriteLine("共有{0}个输入文件", inputFiles.Length);

            StreamEx s = new StreamEx(input + ".repack.pac", FileMode.Create, FileAccess.Write);

            s.WriteInt32(inputFiles.Length);

            for (int i = 0; i < inputFiles.Length; i++)
            {
                StreamEx sr = new StreamEx(inputFiles[i], FileMode.Open, FileAccess.Read);
                Console.WriteLine("正在封入文件{0}/{1}:{2} {3}->{4}", i + 1,inputFiles[i], inputFiles.Length,s.Position, sr.Length);
                s.WriteInt32((Int32)sr.Length);

                s.WriteFromStream(sr, sr.Length);
            }
            s.Close();
        }
    }
}
