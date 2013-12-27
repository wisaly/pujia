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
		        new KeyValuePair<string, string>("\x00","{null}"),
		        new KeyValuePair<string, string>("\x3f","{0x003f}"),
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
                if ((b == 0) && ((bytes.Count % 2 == 1 && endzeros != 0) || (bytes.Count % 2 == 0)))
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

                Int32 nextBlockOffset = s.ReadInt32() + (Int32)s.Position;

                textOffset += 8;
                texts.Add("{TEXT}");
                for (int i = 0; i < indexes.Count;i++ )
                {
                    s.Position = textOffset + indexes[i];
                    texts.Add(readUnicodeString(s, 4));
                }

                if (nextBlockOffset < s.Length)
                {
                    s.Position = nextBlockOffset;
                    if (s.ReadInt32BigEndian() == 0x4E504354) //NCPT
                    {
                        texts.Add("{NCPT}");
                        s.Position = s.ReadInt32() + s.Position;
                    }
                    if (s.ReadInt32BigEndian() == 0x4E414D45) //NAME
                    {
                        texts.Add("{NAME}");
                        Int32 nameBlockLength = s.ReadInt32();
                        s.Position += 2;
                        while (s.Position < s.Length)
                        {
                            texts.Add(readUnicodeString(s, 2));
                        }
                    }
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

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i] = texts[i].Remove(texts[i].Length - 5);
                // 处理字符集差异
                foreach (KeyValuePair<string, string> kvp in _convertChar)
                {
                    texts[i] = texts[i].Replace(kvp.Value, kvp.Key);
                }
            }

            StreamEx ssource = new StreamEx(path, FileMode.Open, FileAccess.Read);
            StreamEx sdest = new StreamEx(path + ".imp", System.IO.FileMode.Create, System.IO.FileAccess.Write);

            Int32 header = ssource.ReadInt32BigEndian();
            if (header == _fixHeaderType1)
            {
                Console.Write("[SETU]");
                sdest.WriteInt32BigEndian(_fixHeaderType1);
                sdest.WriteInt32(texts.Length);

                Int32[] textOffset = new Int32[texts.Length];

                for (int i = 0; i < texts.Length;i++ )
                {
                    sdest.WriteInt32(0); // 长度占位写0
                }

                for (int i = 0; i < texts.Length; i++)
                {
                    if (texts[i].Length == 0)
                    {
                        textOffset[i] = 0;
                        continue;
                    }
                    textOffset[i] = (Int32)sdest.Position; // 文本偏移量
                    sdest.Write(_sourceEncoding.GetBytes(texts[i]));
                    sdest.WriteInt16(0); // 结束0
                }

                ssource.Position = ssource.Length - 139;
                sdest.WriteFromStream(ssource, 139);

                sdest.Position = 8;
                for (int i = 0; i < textOffset.Length;i++ )
                {
                    sdest.WriteInt32(textOffset[i]);
                }
            }
            else if ((header >> 16) == _fixHeaderType2)
            {
                Console.Write("[0000]");
                sdest.WriteInt16(0);

                for(int i = 0;i < texts.Length;i++)
                {
                    sdest.Write(_sourceEncoding.GetBytes(texts[i]));
                    sdest.WriteInt16(0);
                }
            }
            else if (header == _fixHeaderType3)
            {
                Console.Write("[TCRC]");
                Int32 textBlockOffset = ssource.ReadInt32() + 8; // 指向"TEXT"

                ssource.Position = 0;
                sdest.WriteFromStream(ssource, textBlockOffset); // 写至索引结束
                sdest.WriteFromStream(ssource, 4); // 写入"TEXT"
                Int32 sourceTextLength = ssource.ReadInt32(); // 原文本段长度
                sdest.WriteInt32(0);

                List<Int32> textIndexes = new List<Int32>();
                if (texts[0] != "{TEXT}")
                {
                    throw new Exception("文本分段错误");
                }

                int iText = 1;
                for (; iText < texts.Length && texts[iText] != "{NCPT}" && texts[iText] != "{NAME}"; iText++)
                {
                    textIndexes.Add((Int32)sdest.Position - textBlockOffset - 8); // 获取偏移
                    sdest.Write(_sourceEncoding.GetBytes(texts[iText])); // 写入文本
                    sdest.WriteInt32(0); // 写入0结尾
                }
                Int32 destTextLength = (Int32)sdest.Position - textBlockOffset - 8; // 文本段长度

                sdest.Position = 8;
                for (int i = 0; i < textIndexes.Count;i++ )
                {
                    sdest.Position += 4;
                    sdest.WriteInt32(textIndexes[i]);
                }
                if (sdest.Position != textBlockOffset)
                {
                    throw new Exception("文本段写入失败");
                }
                sdest.Position += 4;
                sdest.WriteInt32(destTextLength);

                if (iText < texts.Length)
                {
                    ssource.Position = textBlockOffset + 8 + sourceTextLength;
                    sdest.Position = textBlockOffset + 8 + destTextLength;
                    if (texts[iText] == "{NCPT}")
                    {
                        ssource.Position += 4;
                        Int32 ncptLength = ssource.ReadInt32(); // get ncpt block length
                        ssource.Position -= 8;

                        sdest.WriteFromStream(ssource, ncptLength + 8);
                        iText++;
                    }
                    if (texts[iText] == "{NAME}")
                    {
                        sdest.WriteInt32BigEndian(0x4E414D45);
                        Int32 nameBlockLengthPosition = (Int32)sdest.Position;
                        sdest.WriteInt32(0); // 长度占位

                        sdest.WriteInt16(0);
                        for (iText = iText + 1; iText < texts.Length; iText++)
                        {
                            sdest.Write(_sourceEncoding.GetBytes(texts[iText]));
                            sdest.WriteInt16(0);
                        }
                        Int32 nameBlockLength = (Int32)sdest.Position - nameBlockLengthPosition - 4;
                        sdest.Position = nameBlockLengthPosition;
                        sdest.WriteInt32(nameBlockLength);
                    }
                }
            }
            else
            {
                throw new Exception("不支持的文件头");
            }
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
                        importFile(files[i]));
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
                import(dirs[i]);
            }
        }
    }
}
