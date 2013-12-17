using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly;
using System.IO;

namespace BLJM61076
{
    class dat
    {
        class headerNode
        {
            public string fileName = "";
            public Int32 fileOffset = 0;
            public Int32 fileLength = 0;
        }

        static Int64 fixHeaderFilename = 0x46696C656E616D65;
        static Int64 fixHeaderPack     = 0x5061636B20202020;
        static bool needAlign = true;

        static Int64 align(Int64 origin)
        {
            if (needAlign)
            {
                return origin + (0x10 - origin % 0x10);
            }
            else
            {
                return origin;
            }
        }
        static void zeroTo(StreamEx s,int offset)
        {
            int len = offset - (int)s.Position;
            byte[] b = new byte[len];
            s.Write(b);
        }

        public static void setAlign(bool nAlign)
        {
            needAlign = nAlign;
        }

        public static void unpack(string input)
        {
            StreamEx s = new StreamEx(input, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            string unpackdir = Path.GetFullPath(input) + "_unpack";
            if (!Directory.Exists(unpackdir))
            {
                Directory.CreateDirectory(unpackdir);
            }

            Int64 fixedHeaderRead = s.ReadInt64BigEndian();
            if (fixedHeaderRead != fixHeaderFilename)
            {
                throw new Exception("文件头不能识别");
            }

            Int32 fileNameBlockEnd = s.ReadInt32BigEndian();

            List<Int32> fileNameOffset = new List<Int32>();

            fileNameOffset.Add(s.ReadInt32BigEndian() + 0xc);
            while (s.Position < fileNameOffset[0])
	        {
                fileNameOffset.Add(s.ReadInt32BigEndian() + 0xc);
	        }

            Console.WriteLine("共读取{0}个文件名索引",fileNameOffset.Count);
            fileNameOffset.Add(fileNameBlockEnd);
            List<string> fileNames = new List<string>();
            for (int i = 0;i < fileNameOffset.Count-1;i++)
            {
                s.Position = fileNameOffset[i];
                fileNames.Add(s.ReadSimpleString(fileNameOffset[i+1]-fileNameOffset[i]));
            }

            Console.WriteLine("共读取{0}个文件名索引",fileNames.Count);
            s.Position = align(s.Position);
            List<Int32> fileOffset = new List<Int32>();
            List<Int32> fileLength = new List<Int32>();

            Int64 packHeader = s.ReadInt64BigEndian();
            if (packHeader != fixHeaderPack)
            {
                throw new Exception("包块开头不能识别");
            }
            Int32 dataOffset = (Int32)s.Position + s.ReadInt32BigEndian();
            Int32 packCount = s.ReadInt32BigEndian();

            for (int i = 0; i < packCount; i++)
            {
                fileOffset.Add(s.ReadInt32BigEndian());
                fileLength.Add(s.ReadInt32BigEndian());
            }

            for (int i = 0; i < packCount; i++)
            {
                StreamEx sNew = new StreamEx(unpackdir + "\\" + fileNames[i], FileMode.Create, FileAccess.Write);

                Console.WriteLine("正在解包文件{0}/{1}:{2} ({3}->{4})", i + 1, packCount, fileNames[i], fileOffset[i], fileLength[i]);
                s.Position = fileOffset[i];

                sNew.WriteFromStream(s, fileLength[i]);
                sNew.Close();
            }
        }
        public static void repack(string input)
        {
            //if (!Directory.Exists(input))
            //{
            //    throw new Exception("输入参数必须是目录");
            //}

            //string[] inputFiles = Directory.GetFiles(input);

            //List<headerNode> headers = new List<headerNode>();
            //int lastOffset = 0x10 + inputFiles.Length * 0x40;
            //align(ref lastOffset);
            
            //for (int i = 0; i < inputFiles.Length;i++ )
            //{
            //    headerNode h = new headerNode();
            //    FileInfo f = new FileInfo(inputFiles[i]);
            //    h.fileName = f.Name;
            //    h.fileLength = (int)f.Length;
            //    h.fileOffset = lastOffset;

            //    lastOffset += h.fileLength;
            //    align(ref lastOffset);

            //    headers.Add(h);
            //}
            //Console.WriteLine("共有{0}个输入文件", headers.Count);
            
            //StreamEx s = new StreamEx(input + ".repack.dat", FileMode.Create, FileAccess.Write);

            //s.WriteInt64BigEndian(fixHeaderPS3FS_V1);
            //s.WriteInt32BigEndian(inputFiles.Length);
            //s.WriteInt32BigEndian(0);

            //for (int i = 0; i < headers.Count;i++ )
            //{
            //    s.WriteSimpleString(headers[i].fileName, 0x30);
            //    s.WriteInt32BigEndian(0);
            //    s.WriteInt32BigEndian(headers[i].fileLength);
            //    s.WriteInt32BigEndian(0);
            //    s.WriteInt32BigEndian(headers[i].fileOffset);
            //}
            //Console.WriteLine("文件索引写入完毕");

            //for (int i = 0; i < headers.Count; i++)
            //{
            //    Console.WriteLine("正在封入文件{0}/{1}:{2} ({3}->{4})", i + 1, headers.Count, headers[i].fileName, headers[i].fileOffset, headers[i].fileLength);
                
            //    s.Position = headers[i].fileOffset;
            //    StreamEx sr = new StreamEx(inputFiles[i], FileMode.Open, FileAccess.Read);
            //    s.WriteFromStream(sr, headers[i].fileLength);
            //}
            //zeroTo(s, lastOffset);
            //s.Close();
        }
    }
}
