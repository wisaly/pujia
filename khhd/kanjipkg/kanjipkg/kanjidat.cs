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
    }
}
