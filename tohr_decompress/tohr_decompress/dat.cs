using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly;
using System.IO;

namespace tohr_decompress
{
    class dat
    {
        static public void decompress(string infile,string outfile)
        {
            StreamEx sin = new StreamEx(infile, FileMode.Open, FileAccess.Read);
            StreamEx sms = new StreamEx(new MemoryStream());

            while (sin.Position < sin.Length)
            {
                byte p1 = sin.ReadByte();
                byte p2 = 0;
                byte p3 = 0;

                if (p1 < 0x40)
                {
                    byte F = 0;
                    if (p1 == 0)
                    {
                        //stepFlag = 6;

                        p2 = sin.ReadByte();
                        if (p2 >= 0x40)
                        {
                            F = (byte)(p2 - 0x40);
                        }
                        else
                        {
                            //stepFlag = 7;
                            p3 = sin.ReadByte();
                            p2 = (byte)(p2 * 2 + p3 * 4);
                            if (p2 > 0)
                            {
                                p2 += 2;
                            }
                            F = p2;
                        }
                    }
                    else
                    {
                        //stepFlag = 0;
                        F = p1;
                    }
                    sms.WriteFromStream(sin,F);
                }
                else
                {
                    int S = 0; // 回退量
                    int P = 0; // 复制量
                    // 复制回退
                    if (p1>0x7f)
                    {
                        //stepFlag = 4;
                        p2 = sin.ReadByte();
                        if ((p1 & 0x40) > 0)
                        {
                            //stepFlag = 5;
                            p3 = sin.ReadByte();
                            S = ((p2 & 0x7f) << 8) + p3 + 1;
                            P = (p1 & 0x3f) * 2 + (p2 >> 7) + 4;
                        }
                        else
                        {
                            //stepFlag = 0;
                            S = ((p1 & 0x3) << 8) + p2 + 1;
                            P = ((p1 >> 2) & 0xf) + 3;
                        }
                    }
                    else
                    {
                        //stepFlag = 0;
                        S = (p1 & 0xf) + 1;
                        P = (p1 / 0x10) - 2;
                    }

                    long curpos = sms.Position;
                    while (P > 0)
                    {
                        sms.Position -= S;
                        byte b = sms.ReadByte();
                        sms.Position += S - 1;
                        sms.WriteByte(b);
                        P--;
                    }
                    
                    
                }
            }

            StreamEx sout = new StreamEx(outfile, FileMode.Create, FileAccess.Write);
            sms.Position = 0;
            sout.WriteFromStream(sms, sms.Length);
            sout.Close();
        }
    }
}
