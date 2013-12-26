using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly;
using System.IO;
using Firefly.Texting;
using Firefly.TextEncoding;

namespace matelmax2r
{
    class unknown
    {
        static List<KeyValuePair<string, string>> _convertChar = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("\n", "\r\n"),
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

        static Encoding _sourceEncoding = TextEncoding.UTF16;
        static Encoding _destEncoding = TextEncoding.GB2312;

        static Int32 _fixHeaderType1 = 0x53455455;  // SETU
        static Int32 _fixHeaderType2 = 0x00;        // 00
        static Int32 _fixHeaderType3 = 0x54435243;  // TCRC

        static string readUnicodeString(StreamEx s,int endzerobyteCount)
        {
            List<byte> bytes = new List<byte>();
            int endzeros = 0;
            while (s.Position < s.Length)
            {
                byte b = s.ReadByte();
                if (b == 0)
                {
                    endzeros ++;
                }
                else
                {
                    endzeros = 0;
                }

                if (endzeros == endzerobyteCount)
                {
                    bytes.RemoveRange(bytes.Count - endzerobyteCount + 1, endzerobyteCount - 1);
                    break;
                }
                
                bytes.Add(b);
            }
           
            return new string(_sourceEncoding.GetChars(bytes.ToArray()));
        }

        static public int exportFile(string path)
        {
            StreamEx s = new StreamEx(path, FileMode.Open, FileAccess.Read);

            List<string> texts = new List<string>();

            Int32 header = s.ReadInt32BigEndian();
            if (header == _fixHeaderType1)
            {
                Console.Write("[SETU]");
                Int32 textCount = s.ReadInt32();

                Int32[] textOffset = new Int32[textCount];
                for (int i = 0; i < textCount;i++ )
                {
                    textOffset[i] = s.ReadInt32();
                }

                for (int i = 0; i < textCount;i++ )
                {
                    string txt = "";
                    if (textOffset[i] != 0)
                    {
                        s.Position = textOffset[i];
                        txt = readUnicodeString(s,2);
                    }
                    texts.Add(txt);
                }
            }
            else if ((header >> 16) == _fixHeaderType2)
            {
                Console.Write("[0000]");
                s.Position = 2;
                while(s.Position < s.Length)
                {
                    texts.Add(readUnicodeString(s, 2));
                }
            }
            else if (header == _fixHeaderType3)
            {
                Console.Write("[TCRC]");
                Int32 textOffset = s.ReadInt32() + 8;
                List<Int32> indexes = new List<Int32>();
                while (s.Position < textOffset)
                {
                    s.Position += 4;
                    indexes.Add(s.ReadInt32());
                }
                if(s.ReadInt32BigEndian() != 0x54455854) //TEXT
                {
                    throw new Exception("TEXT段错误");
                }
                textOffset += 8;
                for (int i = 0; i < indexes.Count;i++ )
                {
                    s.Position = textOffset + indexes[i];
                    texts.Add(readUnicodeString(s, 4));
                }
            }
            else
            {
                throw new Exception("不支持的文件头");
            }

            for (int i = 0; i < texts.Count;i++ )
            {
                // 处理字符集差异
                foreach (KeyValuePair<string, string> kvp in _convertChar)
                {
                    texts[i] = texts[i].Replace(kvp.Key, kvp.Value);
                }
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

            string[] files = Directory.GetFiles(path, "*.unknown");
            string[] dirs = Directory.GetDirectories(path);
            Console.WriteLine("{0}:共搜索到{1}个文件,{2}个子目录", path, files.Length, dirs.Length);

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Console.WriteLine("{0}/{1}已处理文件{2}:导出{3}行。",
                        i + 1,
                        files.Length,
                        files[i],
                        exportFile(files[i]));
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("[出错]{0}/{1}文件{2}:{3}",
                        i + 1,
                        files.Length,
                        files[i],
                        ex.Message);
                }
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

            string[] files = Directory.GetFiles(path, "*.unknown");
            Console.WriteLine("共搜索到{0}个文件", files.Length);

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Console.WriteLine("{0}/{1}已处理文件{2}:导入{3}行。",
                        i + 1,
                        files.Length,
                        files[i],
                        importFile(files[i]));
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}