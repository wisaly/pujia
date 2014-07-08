using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly;
using System.IO;

namespace kanjipkg
{
    class kanjidat
    {
        /* 文件格式说明
         * 
         * 0~0xc000是文件头
         * 每个文件头节点：
         * string 0x20
         * Int32 文件偏移量（加上c000等于实际偏移量）
         * Int32 文件长度
         * 8 byte 0对齐
         */
        class headerNode
        {
            public string fileName;
            public Int32 fileOffset;
            public Int32 fileLength;
        }
        public static void unpack(string input)
        {
            StreamEx s = new StreamEx(input, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            string unpackdir = Path.GetFullPath(input) + "_unpack";
            if (!Directory.Exists(unpackdir))
            {
                Directory.CreateDirectory(unpackdir);
            }

            List<headerNode> headers = new List<headerNode>();
            while (s.Position < 0xc000)
            {
                headerNode h = new headerNode();
                h.fileName = s.ReadSimpleString(0x20);
                h.fileOffset = s.ReadInt32() + 0xc000;
                h.fileLength = s.ReadInt32();
                s.Position += 8;

                if (h.fileLength == 0)
                {
                    break;
                }

                headers.Add(h);
            }

            Console.WriteLine("文件头解析:共有{0}个文件", headers.Count);

            for (int i = 0; i < headers.Count; i++)
            {
                StreamEx sNew = new StreamEx(unpackdir + "\\" + headers[i].fileName, FileMode.Create, FileAccess.Write);

                Console.WriteLine("正在导出文件{0}/{1}:{2} ({3}<={4})", i + 1, headers.Count, headers[i].fileName,headers[i].fileOffset,headers[i].fileLength);
                s.Position = headers[i].fileOffset;
                byte[] d = s.Read(headers[i].fileLength);
                sNew.Write(d);
            }
        }

        public static void repack(string folder,string repackFile)
        {
            var files = Directory.GetFiles(folder).OrderBy(f=>f).ToArray();

            Console.WriteLine("目录中共有{0}个文件", files.Length);

            StreamEx s = new StreamEx(
                Path.Combine(Path.GetDirectoryName(Path.GetFullPath(folder)), repackFile),
                FileMode.Create, FileAccess.Write);

            for (int i = 0; i < 0xc000; i++)
            {
                s.WriteByte(0);
            }
            s.Position = 0;

            Int32 packDataPos = 0;
            for (int i = 0; i < files.Length;i++ )
            {
                s.Position = 0x30 * i;

                StreamEx sfile = new StreamEx(files[i], FileMode.Open, FileAccess.Read);
                s.WriteSimpleString(Path.GetFileName(files[i]), 0x20);
                s.Position = 0x30 * i + 0x20;
                s.WriteInt32(packDataPos + 0xc000);
                s.WriteInt32((Int32)sfile.Length);

                packDataPos += (Int32)sfile.Length;
                s.Position = s.Length;

                Console.WriteLine("正在导出文件{0}/{1}:{2} ({3}<={4})", i + 1, files.Length, files[i], packDataPos, sfile.Length);
                
                s.WriteFromStream(sfile, sfile.Length);
            }

            s.Close();
        }

    }
}
