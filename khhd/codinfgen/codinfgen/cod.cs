using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly;

namespace codinfgen
{
    class cod
    {
        public static bool ExportFont(string input,string refinput)
        {
            StreamEx sOri = new StreamEx(input, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            List<UInt16> originChars = new List<UInt16>();
            while (sOri.Position < sOri.Length)
            {
                originChars.Add(sOri.ReadUInt16BigEndian());
                sOri.Position += 6;
            }
            sOri.Close();

            // load ref file
            StreamEx sRef = new StreamEx(refinput, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            if (sRef.Length < 2)
            {
                return false;
            }
            byte xff = sRef.ReadByte();
            byte xfe = sRef.ReadByte();
            if (xff != 0xff || xfe != 0xfe)
            {
                return false;
            }
            while (sRef.Position < sRef.Length)
            {
                UInt16 c = sRef.ReadUInt16();
                if (!originChars.Contains(c))
                {
                    // add not already exist char
                    originChars.Add(c);
                }
            }
            sRef.Close();

            StreamEx sExp = new StreamEx(input + ".txt", System.IO.FileMode.Create, System.IO.FileAccess.Write);
            sExp.WriteByte(0xff);
            sExp.WriteByte(0xfe);
            foreach (UInt16 c in originChars)
            {
                sExp.WriteUInt16(c);
            }

            sExp.Close();

            return true;
        }

        public static bool GenCodInf(string input)
        {
            StreamEx sInput = new StreamEx(input, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            if (sInput.Length < 2)
            {
                return false;
            }
            byte xff = sInput.ReadByte();
            byte xfe = sInput.ReadByte();
            if (xff != 0xff || xfe != 0xfe)
            {
                return false;
            }

            List<UInt16> chars = new List<UInt16>();
            while (sInput.Position < sInput.Length)
            {
                chars.Add(sInput.ReadUInt16());
            }

            UInt16 totWidth = 2048;
            UInt16 totHeight = 2048;
            byte charWidth = 32;
            byte charHeight = 32;

            StreamEx sInf = new StreamEx(input + ".inf.dat", System.IO.FileMode.Create, System.IO.FileAccess.Write);
            sInf.WriteUInt16BigEndian((UInt16)chars.Count);
            sInf.WriteUInt16BigEndian((UInt16)totWidth);
            sInf.WriteUInt16BigEndian((UInt16)totHeight);
            sInf.WriteByte(charWidth);
            sInf.WriteByte(charHeight);
            sInf.Close();

            StreamEx sCod = new StreamEx(input + ".cod.dat", System.IO.FileMode.Create, System.IO.FileAccess.Write);
            for (int i = 0; i < chars.Count; i++)
            {
                sCod.WriteUInt16BigEndian(chars[i]);
                sCod.WriteUInt16BigEndian((UInt16)(i % (totWidth / charWidth) * charWidth));
                sCod.WriteUInt16BigEndian((UInt16)(i / (totWidth / charWidth) * charHeight));
                sCod.WriteUInt16BigEndian((UInt16)(charWidth - 2));
            }
            return true;
        }
    }
}
