using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly;

namespace fontpkg
{
    class Pkg
    {
        public static void Unpack(string input)
        {
            StreamEx s = new StreamEx(input, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            // 52141C7E
            Int32 magicNumber = s.ReadInt32();

            Int32 fileCount = s.ReadInt32();

            string[] fileName = new string[fileCount];
            Int32[] strangeNumber = new Int32[fileCount];
            Int32[] fileLength = new Int32[fileCount];
            Int32[] fileOffset = new Int32[fileCount];
            for (int i = 0; i < fileCount;i ++ )
            {
                fileName[i] = s.ReadSimpleString(0x40);
                strangeNumber[i] = s.ReadInt32();
                fileLength[i] = s.ReadInt32();
                fileOffset[i] = s.ReadInt32();
                Int32 one = s.ReadInt32();
            }

            for (int i = 0; i < fileCount;i++ )
            {
                s.Position = fileOffset[i];
                Int32 strangeNumberAgain = s.ReadInt32();
                Int32 fileLengthAgain = s.ReadInt32();
                Int32 oneAgain = s.ReadInt32();
                byte[] fileContent = s.Read(fileLength[i] - 12);

                // unpack
                StreamEx unpackFile = new StreamEx(fileName[i], System.IO.FileMode.Create, System.IO.FileAccess.Write);
                unpackFile.Write(fileContent);
                unpackFile.Close();
            }
        }
    }
}
