using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Firefly;
using Firefly.Texting;
using System.Data.SQLite;
using System.Data;

namespace togf
{
    class scs
    {
        static List<KeyValuePair<string, string>> _convertChar = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("\n", "\r\n"),
                new KeyValuePair<string, string>("\v",   "{vtab}"),
                new KeyValuePair<string, string>("\a",   "{bell}"),
                new KeyValuePair<string, string>("\b",   "{back}"),
                new KeyValuePair<string, string>("\x10", "{dle}"),
                new KeyValuePair<string, string>("\x11", "{dc1}"),
                new KeyValuePair<string, string>("\x3",  "{etx}"),
                new KeyValuePair<string, string>("♪",  "{tune}"),
                new KeyValuePair<string, string>("・", "·"),
                new KeyValuePair<string, string>("","※")
                //new KeyValuePair<string, string>("∀", "∨"),
                //new KeyValuePair<string, string>("∃", "∈")
            };

        static Encoding _sourceEncoding = Encoding.GetEncoding("shift-jis");
        static Encoding _destEncoding = Encoding.GetEncoding("gb2312");

        static private string convertStringToGBK(string src)
        {
            foreach (KeyValuePair<string,string> kvp in _convertChar)
            {
                src = src.Replace(kvp.Key, kvp.Value);
            }
            return src;
        }

        static public int exportFile(string path)
        {
            StreamEx s = new StreamEx(path, FileMode.Open, FileAccess.Read);
            Int32 textCount = s.ReadInt32BigEndian();

            Int32[] textOffsets = new Int32[textCount + 1];
            for (int i = 0; i < textCount;i++ )
            {
                textOffsets[i] = s.ReadInt32BigEndian();
            }
            textOffsets[textCount] = (Int32)s.Length;

            string[] texts = new string[textCount];
            for (int i = 0; i < textCount;i++ )
            {
                s.Position = textOffsets[i];
                texts[i] = s.ReadString(textOffsets[i + 1] - textOffsets[i], _sourceEncoding);

                // 处理字符集差异
                texts[i] = convertStringToGBK(texts[i]);
            }

            Agemo.WriteFile(path + ".txt", _destEncoding, from txt in texts select txt + "{END}");

            return textCount;
        }

        static public int importFile(string path)
        {
            string[] texts = Agemo.ReadFile(path, _destEncoding);
            StreamEx s = new StreamEx(path + ".scs", System.IO.FileMode.Create, System.IO.FileAccess.Write);

            s.WriteInt32BigEndian(texts.Length);
            s.Position += texts.Length * 4;
            Int32[] textOffset = new Int32[texts.Length];

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i] = texts[i].Remove(texts[i].Length - 5);
                textOffset[i] = (Int32)s.Position;
                // 处理字符集差异
                foreach (KeyValuePair<string, string> kvp in _convertChar)
                {
                    texts[i] = texts[i].Replace(kvp.Value, kvp.Key);
                }
                s.WriteString(texts[i], _sourceEncoding.GetByteCount(texts[i]) + 1, _sourceEncoding);
            }

            s.Position = 4;
            for (int i = 0; i < textOffset.Length;i++ )
            {
                s.WriteInt32BigEndian(textOffset[i]);
            }

            return texts.Length;
        }

        static public int exportTogFile(string path)
        {
            SQLiteConnection conn = new SQLiteConnection(string.Format("data source={0}", path));

            SQLiteDataAdapter da = new SQLiteDataAdapter(
                "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='Japanese' " +
                "UNION ALL " +
                "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='Text'"
                , conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if((long)dt.Rows[0][0] == 1)
            {
                da = new SQLiteDataAdapter("select string from Japanese", conn);
                dt = new DataTable();
                da.Fill(dt);
            }
            else if ((long)dt.Rows[1][0] == 1)
            {
                da = new SQLiteDataAdapter("select english from text", conn);
                dt = new DataTable();
                da.Fill(dt);
            }
            else
            {
                throw new Exception("没有找到Japanese或Text表");
            }

            List<string> texts = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
//                 if (dr[0].ToString() == string.Empty)
//                 {
//                     continue;
//                 }
                texts.Add(convertStringToGBK(dr[0].ToString()));
            }

            if (texts.Count != 0)
            {
                Agemo.WriteFile(path + ".txt", _destEncoding, from txt in texts select txt + "{END}");
            }

            return texts.Count;
        }

        static public void exportTog(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new Exception("路径不存在");
            }

            string[] files = Directory.GetFiles(path, "*");
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
                        exportTogFile(files[i]));
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("处理时发生错误{0}:{1}", files[i],ex.Message);
                }
            }

            for (int i = 0; i < dirs.Length; i++)
            {
                exportTog(dirs[i]);
            }
        }

        static public void export(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new Exception("路径不存在");
            }

            string[] files = Directory.GetFiles(path, "*.scs");
            string[] dirs = Directory.GetDirectories(path);
            Console.WriteLine("{0}:共搜索到{1}个文件,{2}个子目录", path, files.Length,dirs.Length);

            for (int i = 0; i < files.Length;i++ )
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

            string[] files = Directory.GetFiles(path, "*.txt");
            Console.WriteLine("共搜索到{0}个文件", files.Length);

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine("{0}/{1}已处理文件{2}:导出{3}行。",
                    i + 1,
                    files.Length,
                    files[i],
                    importFile(files[i]));
            }
        }
    }
}
