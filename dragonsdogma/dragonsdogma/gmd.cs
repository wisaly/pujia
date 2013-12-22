using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly;
using System.IO;
using Firefly.Texting;

namespace dragonsdogma
{
    class gmd
    {
        static List<KeyValuePair<string, string>> _convertChar = new List<KeyValuePair<string, string>>()
            {
                //new KeyValuePair<string, string>("\n", "\r\n"),
                //new KeyValuePair<string, string>("\v",   "{vtab}"),
                //new KeyValuePair<string, string>("\a",   "{bell}"),
                //new KeyValuePair<string, string>("\b",   "{back}"),
                //new KeyValuePair<string, string>("\x10", "{dle}"),
                //new KeyValuePair<string, string>("\x11", "{dc1}"),
                //new KeyValuePair<string, string>("\x3",  "{etx}"),
                new KeyValuePair<string, string>("♪",  "{tune}"),
                new KeyValuePair<string, string>("・", "·")
                //new KeyValuePair<string, string>("∀", "∨"),
                //new KeyValuePair<string, string>("∃", "∈")
            };

        static Encoding _sourceEncoding = Encoding.GetEncoding("utf-8");
        static Encoding _destEncoding = Encoding.GetEncoding("gb2312");

        static public int exportFile(string path)
        {
            StreamEx s = new StreamEx(path, FileMode.Open, FileAccess.Read);

            Int32 header = s.ReadInt32BigEndian();
            if (header != 0x00444D47 )
            {
                throw new Exception("不支持的文件头");
            }

            s.Position = 0x18;
            Int32 textCount = s.ReadInt32BigEndian();
            Int32 textOffset = s.ReadInt32BigEndian() + textCount * 8 + 0x30;
            s.Position = textOffset;

            List<string> texts = new List<string>();
            while (s.Position < s.Length)
            {
                Int64 curPos = s.Position;
                string curToken = s.ReadString((int)(s.Length - s.Position), _sourceEncoding);
                s.Position = curPos + _sourceEncoding.GetByteCount(curToken) + 1;

                // 处理字符集差异
                foreach (KeyValuePair<string, string> kvp in _convertChar)
                {
                    curToken = curToken.Replace(kvp.Key, kvp.Value);
                }

                texts.Add(curToken);
            }
            Agemo.WriteFile(path + ".txt", _destEncoding, from txt in texts select txt + "{END}");

            return texts.Count;
        }


        static public int importFile(string path)
        {
            string[] texts = Agemo.ReadFile(path + ".txt", _destEncoding);
            StreamEx ssource = new StreamEx(path, FileMode.Open, FileAccess.Read);

            Int32 header = ssource.ReadInt32BigEndian();
            if (header != 0x00444D47)
            {
                throw new Exception("不支持的文件头");
            }

            ssource.Position = 0x18;
            Int32 textCount = ssource.ReadInt32BigEndian();
            Int32 textOffset = ssource.ReadInt32BigEndian() + textCount * 8 + 0x30;

            StreamEx sdest = new StreamEx(path + ".imp", System.IO.FileMode.Create, System.IO.FileAccess.Write);

            ssource.Position = 0;
            sdest.WriteFromStream(ssource, textOffset);

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i] = texts[i].Remove(texts[i].Length - 5);
                // 处理字符集差异
                foreach (KeyValuePair<string, string> kvp in _convertChar)
                {
                    texts[i] = texts[i].Replace(kvp.Value, kvp.Key);
                }
                sdest.WriteString(texts[i], _sourceEncoding.GetByteCount(texts[i]) + 1, _sourceEncoding);
            }

            Int32 textLength = (int)(sdest.Position - textOffset);
            sdest.Position = 0x20;
            sdest.WriteInt32BigEndian(textLength);

            sdest.Close();

            return texts.Length;
        }


        static public void export(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new Exception("路径不存在");
            }

            string[] files = Directory.GetFiles(path, "*.gmd");
            string[] dirs = Directory.GetDirectories(path);
            Console.WriteLine("{0}:共搜索到{1}个文件,{2}个子目录", path, files.Length, dirs.Length);

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine("{0}/{1}已处理文件{2}:导出{3}行。",
                    i + 1,
                    files.Length,
                    files[i],
                    exportFile(files[i]));
            }

            for (int i = 0; i < dirs.Length; i++)
            {
                export(dirs[i]);
            }
        }
        static public void import(string path)
        {

            if (!Directory.Exists(path))
            {
                throw new Exception("路径不存在");
            }

            string[] files = Directory.GetFiles(path, "*.gmd");
            Console.WriteLine("共搜索到{0}个文件", files.Length);

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine("{0}/{1}已处理文件{2}:导入{3}行。",
                    i + 1,
                    files.Length,
                    files[i],
                    importFile(files[i]));
            }
        }
    }
}
