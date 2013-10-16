using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly;
using Firefly.Texting;

namespace khtext
{
    class ctdb
    {
        static List<KeyValuePair<string, string>> _convertChar = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("\n", "\r\n"),
                new KeyValuePair<string, string>(new string((char)0x30fb,1), "·"),
                new KeyValuePair<string, string>(new string((char)0x246c,1),"♂")
                //new KeyValuePair<string, string>("∀", "∨"),
                //new KeyValuePair<string, string>("∃", "∈")
            };

        static Encoding _agemoEncoding = Encoding.GetEncoding("gb2312");
        public static void Export(string input)
        {
            StreamEx s = new StreamEx(input, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            // txt count
            s.Position = 0x0e;
            UInt16 txtCount = s.ReadUInt16BigEndian();

            // section offset
            s.Position = 0x10;
            Int32 sec1Offset = s.ReadInt32BigEndian();
            Int32 sec2Offset = s.ReadInt32BigEndian();
            Int32 sec3Offset = s.ReadInt32BigEndian();

            // get text offset
            s.Position = sec1Offset;
            UInt16[] txtOffset = new UInt16[txtCount + 1];
            for (int i = 0; i < txtCount; i++)
            {
                s.Position += 6;
                txtOffset[i] = s.ReadUInt16BigEndian();
            }
            txtOffset[txtCount] = (UInt16)s.Length;

            // get text
            string[] txtVal = new string[txtCount];
            for (int i = 0; i < txtCount; i++)
            {
                s.Position = txtOffset[i];
                txtVal[i] = s.ReadStringWithNull(txtOffset[i + 1] - txtOffset[i] - 2, Encoding.BigEndianUnicode);

                for (int k = 0; k < _convertChar.Count; k++)
                {
                    txtVal[i] = txtVal[i].Replace(_convertChar[k].Key, _convertChar[k].Value);
                }
            }

            Agemo.WriteFile(input + ".txt", _agemoEncoding, from txt in txtVal select txt + "{END}");
        }

        public static void Import(string input)
        {
            StreamEx s = new StreamEx(input, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            string[] txtVal = Agemo.ReadFile(input + ".txt", _agemoEncoding);
            StreamEx si = new StreamEx(input + ".imp", System.IO.FileMode.Create, System.IO.FileAccess.Write);

            // section offset
            s.Position = 0x10;
            Int32 sec1Offset = s.ReadInt32BigEndian();
            Int32 sec2Offset = s.ReadInt32BigEndian();
            Int32 sec3Offset = s.ReadInt32BigEndian();

            // before text data
            s.Position = 0;
            byte[] beforeText = s.Read(sec3Offset);
            si.Write(beforeText);
            si.Flush();

            // text
            UInt16[] txtOffset = new UInt16[txtVal.Length];
            for (int i = 0; i < txtVal.Length; i++)
            {
                txtOffset[i] = (UInt16)si.Position;
                txtVal[i] = txtVal[i].Substring(0, txtVal[i].Length - 5);

                for (int k = 0; k < _convertChar.Count; k++)
                {
                    txtVal[i] = txtVal[i].Replace(_convertChar[k].Value, _convertChar[k].Key);
                }

                for (int j = 0; j < txtVal[i].Length; j++)
                {
                    si.WriteUInt16BigEndian((UInt16)txtVal[i][j]);
                }
                si.WriteUInt16(0);
            }

            // text offset
            si.Position = sec1Offset;
            for (int i = 0; i < txtOffset.Length; i++)
            {
                si.Position += 6;
                si.WriteUInt16BigEndian(txtOffset[i]);
            }
            si.Close();
        }
    }
}
