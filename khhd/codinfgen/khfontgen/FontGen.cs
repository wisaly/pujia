using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Firefly;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace khfontgen
{
    class CharUnit
    {
        public UInt16 charVal;
        public int x;
        public int y;
        public int actrualWidth;
    }
    class FontGen
    {
        public static void GetCharset(string input, out List<UInt16> chars)
        {
            StreamEx s = new StreamEx(input, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            byte xff = s.ReadByte();
            byte xfe = s.ReadByte();
            if (xff != 0xff || xfe != 0xfe)
            {
                throw new Exception("字库文件需要使用Big-edian unicode格式文本文件");
            }

            chars = new List<UInt16>();
            while (s.Position < s.Length)
            {
                chars.Add(s.ReadUInt16());
            }
            s.Close();
        }

        public static bool Generate(
            List<UInt16> chars,int charOffset,
            int totWidth,int totHeight,
            int charWidth,int charHeight,
            Font font,bool drawGrid,out Bitmap bmp,string savepath)
        {
            bmp = new Bitmap(totWidth, totHeight);

            List<CharUnit> charUnits = new List<CharUnit>();

            for (int i = 0; i < chars.Count;i ++ )
            {
                CharUnit cu = new CharUnit();
                cu.charVal = chars[i];
                cu.x = i % (totWidth / charWidth) * charWidth;
                cu.y = i / (totWidth / charWidth) * charHeight;
                cu.actrualWidth = 0;

                charUnits.Add(cu);
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.LineAlignment = StringAlignment.Far;
                stringFormat.Alignment = StringAlignment.Near;

                g.FillRectangle(Brushes.Black, 0, 0, totWidth, totHeight);

                if (drawGrid)
                {
                    for (int i = 0; i < totWidth; i += charWidth)
                    {
                        g.DrawLine(Pens.Red, i, 0, i, totHeight);
                    }
                    for (int i = 0; i < totHeight; i += charHeight)
                    {
                        g.DrawLine(Pens.Red, 0, i, totWidth, i);
                    }
                }

                foreach (CharUnit cu in charUnits)
                {
                    string c = new string((char)cu.charVal, 1);
                    Rectangle cellArea = new Rectangle(
                            cu.x + charOffset, cu.y,
                            charWidth - charOffset, charHeight);
                    
                    //SizeF charSz = g.MeasureString(c, font, cellArea.Size, StringFormat.GenericTypographic);
                    Size charSz = TextRenderer.MeasureText(g,c, font,cellArea.Size, TextFormatFlags.NoPadding);
                    cu.actrualWidth = charSz.Width;
                    
                    Point charLeftTop = new Point(
                        cu.x + charOffset,
                        cu.y + (charHeight - charSz.Height));
                    if (drawGrid)
                    {
                        g.DrawRectangle(Pens.Blue, charLeftTop.X, charLeftTop.Y, charSz.Width, charSz.Height);
                    }
                    
                    //g.DrawString(c, font, Brushes.White, charLeftTop);
                    TextRenderer.DrawText(g, c, font, charLeftTop, Color.White,  TextFormatFlags.NoPadding);
                    Application.DoEvents();
                }
            }

            if (savepath != null)
            {
                bmp.Save(savepath + "\\font.png",ImageFormat.Png);
                StreamEx sInf = new StreamEx(savepath + "\\font.inf.dat", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                sInf.WriteUInt16BigEndian((UInt16)chars.Count);
                sInf.WriteUInt16BigEndian((UInt16)totWidth);
                sInf.WriteUInt16BigEndian((UInt16)totHeight);
                sInf.WriteByte((byte)charWidth);
                sInf.WriteByte((byte)charHeight);
                sInf.Close();

                StreamEx sCod = new StreamEx(savepath + "\\font.cod.dat", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                for (int i = 0; i < charUnits.Count; i++)
                {
                    sCod.WriteUInt16BigEndian(charUnits[i].charVal);
                    sCod.WriteUInt16BigEndian((UInt16)charUnits[i].x);
                    sCod.WriteUInt16BigEndian((UInt16)charUnits[i].y);
                    sCod.WriteUInt16BigEndian((UInt16)charUnits[i].actrualWidth);
                }
            }

            return true;
        }
    }
}
