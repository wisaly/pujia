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

            string[] inputFiles = Directory.GetFiles(input);

//             List<headerNode> headers = new List<headerNode>();
//             int lastOffset = 0x10 + inputFiles.Length * 0x40;
//             align(ref lastOffset);
// 
//             for (int i = 0; i < inputFiles.Length; i++)
//             {
//                 headerNode h = new headerNode();
//                 FileInfo f = new FileInfo(inputFiles[i]);
//                 h.fileName = f.Name;
//                 h.fileLength = (int)f.Length;
//                 h.fileOffset = lastOffset;
// 
//                 lastOffset += h.fileLength;
//                 align(ref lastOffset);
// 
//                 headers.Add(h);
//             }
//             Console.WriteLine("共有{0}个输入文件", headers.Count);
// 
//             StreamEx s = new StreamEx(input + ".repack.dat", FileMode.Create, FileAccess.Write);
// 
//             s.WriteInt64BigEndian(fixHeaderPS3FS_V1);
//             s.WriteInt32BigEndian(inputFiles.Length);
//             s.WriteInt32BigEndian(0);
// 
//             for (int i = 0; i < headers.Count; i++)
//             {
//                 s.WriteSimpleString(headers[i].fileName, 0x30);
//                 s.WriteInt32BigEndian(0);
//                 s.WriteInt32BigEndian(headers[i].fileLength);
//                 s.WriteInt32BigEndian(0);
//                 s.WriteInt32BigEndian(headers[i].fileOffset);
//             }
//             Console.WriteLine("文件索引写入完毕");
// 
//             for (int i = 0; i < headers.Count; i++)
//             {
//                 Console.WriteLine("正在封入文件{0}/{1}:{2} ({3}->{4})", i + 1, headers.Count, headers[i].fileName, headers[i].fileOffset, headers[i].fileLength);
// 
//                 s.Position = headers[i].fileOffset;
//                 StreamEx sr = new StreamEx(inputFiles[i], FileMode.Open, FileAccess.Read);
//                 s.WriteFromStream(sr, headers[i].fileLength);
//             }
//             zeroTo(s, lastOffset);
//             s.Close();
        }
    }
}
