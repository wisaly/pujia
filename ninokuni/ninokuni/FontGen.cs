using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Firefly;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Firefly.TextEncoding;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace khfontgen
{
    class CharUnit
    {
        public string charVal;
        public int x;
        public int y;
        public int actrualWidth;
        public int adjustX;
        public int visualWidth;
        public int channel;
    }
    class FontGen
    {
        public static void GetCharset(string input, out List<char> chars)
        {
            StreamEx s = new StreamEx(input, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            byte b1 = s.ReadByte();
            byte b2 = s.ReadByte();
            byte b3 = s.ReadByte();
            if (b1 != 0xef || b2 != 0xbb || b3 != 0xbf)
            {
                throw new Exception("字库文件需要使用UTF-8格式文本文件");
            }

            chars = new List<char>();
            
            string c = s.ReadString((Int32)s.Length - 3, Encoding.UTF8);

            for (int i = 0; i < c.Length;i++ )
            {
                chars.Add(c[i]);
            }
            
            s.Close();
        }

        public static void GetRefInfo(string input,out int width,out int height,out int charHeight,out int charCount)
        {
            StreamEx s = new StreamEx(input, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            s.Position = 0x18;
            charHeight = s.ReadInt32BigEndian();
            charCount = s.ReadInt32BigEndian();
            width = s.ReadInt32BigEndian();
            height = s.ReadInt32BigEndian();
        }

        public static string GetChannelText(int channelIndex)
        {
            string channelText = "_outofrange";
            if (channelIndex == 0)
            {
                channelText = "_blue";
            }
            else if (channelIndex == 1)
            {
                channelText = "_alpha";
            }
            else if (channelIndex == 2)
            {
                channelText = "_red";
            }
            else if (channelIndex == 3)
            {
                channelText = "_green";
            }
            else
            {
                channelText += (channelIndex - 3).ToString();
            }
            return channelText;
        }
        public static int getCodeInt(byte[] coded)
        {
            int ret = 0;
            for (int i = 0; i < coded.Length; i++)
            {
                ret = (ret << 8) | coded[i];
            }

            return ret;
        }
        public static bool Generate(
            List<char> chars,bool gdiRender,TextRenderingHint textRenderingHint,
            int totWidth,int totHeight,
            int charWidth,int charHeight,
            Font font,bool drawGrid,out Bitmap bmp,string savepath,string refFileName)
        {
            List<CharUnit> charUnits = new List<CharUnit>();

            for (int i = 0; i < chars.Count;i ++ )
            {
                CharUnit cu = new CharUnit();
                cu.charVal = chars[i].ToString();
//                 cu.x = i % (totWidth / charWidth) * charWidth;
//                 cu.y = i / (totWidth / charWidth) * charHeight;
//                 cu.actrualWidth = 0;

                charUnits.Add(cu);
            }

            int currentChannel = 0;
            bmp = new Bitmap(totWidth, totHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                //g.PageUnit=GraphicsUnit.Pixel;
                g.TextRenderingHint = textRenderingHint;
                //g.SmoothingMode=SmoothingMode.HighQuality;
                StringFormat stringFormat = StringFormat.GenericTypographic;
                stringFormat.FormatFlags &= ~StringFormatFlags.MeasureTrailingSpaces;
                //stringFormat.LineAlignment = StringAlignment.Near;
                //stringFormat.Alignment = StringAlignment.Near;
                //stringFormat.FormatFlags = StringFormatFlags.

                g.FillRectangle(Brushes.Black, 0, 0, totWidth, totHeight);

                if (drawGrid)
                {
//                     for (int i = 0; i < totWidth; i += charWidth)
//                     {
//                         g.DrawLine(Pens.Red, i, 0, i, totHeight);
//                     }
                    for (int i = 0; i < totHeight; i += charHeight + 1)
                    {
                        g.DrawLine(Pens.Red, 0, i, totWidth, i);
                    }
                }

                Point currentPos = new Point(1, 1);
                foreach (CharUnit cu in charUnits)
                {
                    //string c = new string((char)cu.charVal, 1);
                    Rectangle cellArea = new Rectangle(
                            currentPos.X, currentPos.Y,
                            charHeight, charHeight);
                    
                    Size charSz;
                    if (gdiRender)
                    {
                        // GDI渲染
                        charSz = TextRenderer.MeasureText(g, cu.charVal, font, cellArea.Size, TextFormatFlags.NoPadding);
                    }
                    else
                    {
                        // GDI+渲染
                        charSz = Size.Round(g.MeasureString(cu.charVal, font, charHeight * 2, stringFormat));
                    }

                    cu.actrualWidth = charSz.Width;
                    cu.visualWidth = cu.actrualWidth;
                    cu.adjustX = 0;
                    
                    if (currentPos.X + cu.actrualWidth + 2 >= totWidth)
                    {
                        currentPos.X = 1;
                        currentPos.Y += charHeight + 1;
                    }
                    if (currentPos.Y + charHeight + 1 >= totHeight)
                    {
                        // save
                        if (savepath != null)
                        {
                            bmp.Save(savepath + "\\" + refFileName + GetChannelText(currentChannel) + ".png",
                                ImageFormat.Png);
                        }
                        g.FillRectangle(Brushes.Black, 0, 0, totWidth, totHeight);
                        currentChannel++;
                        currentPos.X = 1;
                        currentPos.Y = 1;
                    }
                    cu.x = currentPos.X;
                    cu.y = currentPos.Y;
                    cu.channel = currentChannel;

                    if (gdiRender)
                    {
                        // GDI渲染
                        TextRenderer.DrawText(g, cu.charVal, font, currentPos, Color.White,  TextFormatFlags.NoPadding);
                    }
                    else
                    {
                        // GDI+渲染
                        g.DrawString(cu.charVal, font, Brushes.White, currentPos.X, currentPos.Y, stringFormat);
                    }

                    // 绘制边框
                    if (drawGrid)
                    {
                        g.DrawRectangle(Pens.White, currentPos.X, currentPos.Y, cu.actrualWidth, charHeight - 1);
                        Application.DoEvents();
                    }

                    currentPos.X += cu.actrualWidth + 2;
                }
            }

            if (savepath != null)
            {
                bmp.Save(savepath + "\\" + refFileName + GetChannelText(currentChannel) + ".png",
                            ImageFormat.Png);
                StreamEx s = new StreamEx(savepath + "\\" + refFileName + ".new", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                // 
                s.Position = 8;
                s.Write(new byte[]{
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x8E,0xB0,0x16,0x8C,0x05,0x55,0x01,0xFF});
                s.WriteInt32BigEndian(charHeight);
                s.WriteInt32BigEndian(chars.Count);
                s.WriteInt32BigEndian(totWidth);
                s.WriteInt32BigEndian(totHeight);
                s.WriteInt32BigEndian(0);

                //
                byte[] indexFixData = new byte[8]{
                    0xCF,0xA7,0xE0,0xA1,0x08,0x55,0x55,0xFF};
                foreach (CharUnit cu in charUnits)
                {
                    s.Write(indexFixData);
                    byte[] sjisCod = TextEncoding.ShiftJIS.GetBytes(cu.charVal);
                    s.WriteInt32BigEndian(getCodeInt(sjisCod));
                    byte[] utf8Cod = TextEncoding.UTF8.GetBytes(cu.charVal);
                    s.WriteInt32BigEndian(getCodeInt(utf8Cod));
                    s.WriteInt32BigEndian(cu.x);
                    s.WriteInt32BigEndian(cu.y);
                    s.WriteInt32BigEndian(cu.actrualWidth);
                    s.WriteInt32BigEndian(cu.adjustX);
                    s.WriteInt32BigEndian(cu.visualWidth);
                    s.WriteInt32BigEndian(cu.channel);
                }

                //
                s.Write(new byte[]{
                   0xCC,0x1C,0x19,0x19,0x01,0x01,0xFF,0xFF,
                   0x00,0x00,0x00,0x02,0xFD,0x16,0xE3,0xB5,
                   0x03,0x15,0xFF,0xFF,0x00,0x00,0xC5,0xB8,
                   0x00,0x00,0xC3,0xBB,0xFF,0xFF,0xFF,0xFE,
                   0xFD,0x16,0xE3,0xB5,0x03,0x15,0xFF,0xFF,
                   0x00,0x00,0xC5,0xB8,0x00,0x00,0xC3,0xBC,
                   0xFF,0xFF,0xFF,0xFE,0x69,0xA4,0x8E,0xC4,
                   0x00});
                while (s.Position % 0x10 != 0)
                {
                    s.WriteByte(0xff);
                }

                //
                int endBlockOffset = (int)s.Position;
                s.Write(new byte[]{
                    0x00,0x00,0x00,0x60,0x00,0x00,0x00,0x05,
                    0x00,0x00,0x00,0x40,0x00,0x00,0x00,0x19,
                    0x8E,0xB0,0x16,0x8C,0x00,0x00,0x00,0x00,
                    0xCF,0xA7,0xE0,0xA1,0x00,0x00,0x00,0x04,
                    0xCC,0x1C,0x19,0x19,0x00,0x00,0x00,0x08,
                    0xFD,0x16,0xE3,0xB5,0x00,0x00,0x00,0x10,
                    0x69,0xA4,0x8E,0xC4,0x00,0x00,0x00,0x15,
                    0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                    0x49,0x4E,0x46,0x00,0x43,0x48,0x52,0x00,
                    0x4B,0x45,0x52,0x4E,0x49,0x4E,0x46,0x00,
                    0x4B,0x45,0x52,0x4E,0x00,0x45,0x4E,0x44,
                    0x00,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                    0x01,0x74,0x32,0x62,0xFE,0x01,0x00,0x01,
                    0x01,0x00,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF});
                // 
                s.Position = 0;
                s.WriteInt32BigEndian((int)s.Length / 0x28);
                s.WriteInt32BigEndian(endBlockOffset);
                s.Close();
            }

            return true;
        }
    }
}
