using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly;
using System.IO;
using Firefly.TextEncoding;

namespace SDBJPY
{
    class BSDJPN
    {
        List<KeyValuePair<string, string>> _convertChar = new List<KeyValuePair<string, string>>();
        public BSDJPN()
        {
            _convertChar.Add(new KeyValuePair<string, string>("\n", "\r\n"));
            _convertChar.Add(new KeyValuePair<string, string>("・", "·"));
            _convertChar.Add(new KeyValuePair<string, string>("♪", "♂"));
            _convertChar.Add(new KeyValuePair<string, string>("∀", "∨"));
            _convertChar.Add(new KeyValuePair<string, string>("∃", "∈"));
        }

        public void AddConvertChar(string originMapPath,string newMapPath)
        {
            var originmap = TblCharMappingFile.ReadFile(originMapPath);
            var newmap = TblCharMappingFile.ReadFile(newMapPath);

            var diff = from o in originmap
                       join n in newmap
                       on o.CodeString equals n.CodeString
                       where o.Character != n.Character
                       select new KeyValuePair<string, string>(o.Character, n.Character);

            _convertChar.AddRange(diff);
        }

        public KeyValuePair<string, string>[] Read(string path)
        {
            StreamEx s = new StreamEx(path, FileMode.Open, FileAccess.Read);

            return Read(s);
        }
        private KeyValuePair<string, string>[] Read(ZeroPositionStreamPasser sp)
        {
            StreamEx s = sp.GetStream();
            s.Position = 0x0;
            Int32 header = s.ReadInt32BigEndian();

            int count = s.ReadInt32BigEndian();

            Int32[] keyaddresses = new Int32[count];
            Int32[] blockaddress = new Int32[count];

            for (int i = 0; i < count; i++)
            {
                keyaddresses[i] = (Int32)s.Position;
                Int32 addrread = s.ReadInt32BigEndian();
                keyaddresses[i] = addrread == 0 ? 0 : keyaddresses[i] + addrread;

                blockaddress[i] = (Int32)s.Position;
                addrread = s.ReadInt32BigEndian();
                blockaddress[i] = addrread == 0 ? 0 : blockaddress[i] + addrread;
            }

            Encoding encoding = Encoding.GetEncoding("utf-8");
            KeyValuePair<string, string>[] result = new KeyValuePair<string, string>[count];
            for (int i = 0; i < count; i++)
            {
                string key = "";
                string txt = "";

                if (keyaddresses[i] != 0)
                {
                    s.Position = keyaddresses[i];
                    key = s.ReadString((int)(s.Length - s.Position), encoding);
                }
//                     ((
//                     (blockaddress[i] == 0)
//                     ? (i + 1 == count ? (Int32)s.Length : keyaddresses[i + 1]) 
//                     : blockaddress[i])
//                     - keyaddresses[i],encoding);
                if (blockaddress[i] != 0)
                {
//                    txt = s.ReadString(((i + 1 == count) ? (Int32)s.Length : keyaddresses[i + 1]) - blockaddress[i], encoding);
                    s.Position = blockaddress[i];
                    txt = s.ReadString((int)(s.Length - s.Position),encoding);

                    for (int k = 0; k < _convertChar.Count;k ++ )
                    {
                        txt = txt.Replace(_convertChar[k].Key, _convertChar[k].Value);
                    }
                }
                
                result[i] = new KeyValuePair<string, string>(key,txt);
            }
            return result;
        }

        public void Write(string path, KeyValuePair<string, string>[] text)
        {
            StreamEx s = new StreamEx(path, FileMode.Create, FileAccess.ReadWrite);

            Write(s, text);
        }
        private void Write(ZeroLengthStreamPasser sp, KeyValuePair<string, string>[] text)
        {
            StreamEx s = sp.GetStream();
            s.Position = 0;
            Int32 header = 0x8;
            Int32 count = text.Length;

            s.WriteInt32BigEndian(header);
            s.WriteInt32BigEndian(count);

            Encoding encoding = Encoding.GetEncoding("utf-8");

            Int32[] keyaddresses = new Int32[count];
            Int32[] blockaddress = new Int32[count];

            s.Position = 0x8 + count * 8;
            for (int i = 0; i < count; i++)
            {
                if (text[i].Key.Length == 0)
                {
                    keyaddresses[i] = 0;
                }
                else
                {
                    keyaddresses[i] = (Int32)s.Position;
                    s.WriteString(text[i].Key, text[i].Key.Length + 1, encoding);
                }

                if (text[i].Value.Length == 0)
                {
                    blockaddress[i] = 0;
                }
                else
                {
                    blockaddress[i] = (Int32)s.Position;
                    string txt = text[i].Value;

                    for (int k = 0; k < _convertChar.Count; k++)
                    {
                        txt = txt.Replace(_convertChar[k].Value, _convertChar[k].Key);
                    }

                    s.WriteString(txt, encoding.GetByteCount(txt) + 1, encoding);
                }
            }

            s.Position = 0x8;
            for (int i = 0; i < count; i++)
            {
                s.WriteInt32BigEndian((keyaddresses[i] == 0) ? 0 : (keyaddresses[i] - (Int32)s.Position));
                s.WriteInt32BigEndian((blockaddress[i] == 0) ? 0 : (blockaddress[i] - (Int32)s.Position));
            }
        }
    }
}
