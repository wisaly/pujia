using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly.Texting;
using Firefly.TextEncoding;

namespace SDBJPY
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() == 0)
            {
                return;
            }

            BSDJPN bsdjpn = new BSDJPN();
            Encoding encoding = Encoding.GetEncoding("gbk");
            string f = args[0];

            if (f.ToLower().EndsWith(".sdbjpn"))
            {
                KeyValuePair<string, string>[] pairs = bsdjpn.Read(f);
                string ff = f.Substring(0, f.Length - 6);
                Agemo.WriteFile(ff + "idx", encoding, from p in pairs select p.Key);
                Agemo.WriteFile(ff + "txt", encoding, from p in pairs select p.Value + "{END}");
            }

            else if (f.ToLower().EndsWith(".txt"))
            {
                string ff = f.Substring(0, f.Length - 3);
                string[] keys = Agemo.ReadFile(ff + "idx", encoding);
                string[] blocks = Agemo.ReadFile(f, encoding);

                KeyValuePair<string, string>[] pairs = keys.Select(
                        (key,idx) => new KeyValuePair<string, string>
                            (key, blocks[idx].Length >= 5 ? blocks[idx].Substring(0, blocks[idx].Length - 5) : blocks[idx]))
                            .ToArray();

                if (args.Count() >= 2)
                {
                    bsdjpn.AddConvertChar(args[1]);
                }

                bsdjpn.Write(ff + "SDBJPN", pairs);
            }

        }
    }
}
