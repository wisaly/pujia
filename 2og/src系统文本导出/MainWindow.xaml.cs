using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace src系统文本导出
{
    public class FixedDataStruct
    {
        public byte[] prev { get; set; }
        public string text { get; set; }
    }
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Dictionary<string, string> tblArr = new Dictionary<string, string>();

        private void Grid_DragEnter_1(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Grid_Drop_1(object sender, DragEventArgs e)
        {
            var pathArray = ((System.Array)e.Data.GetData(DataFormats.FileDrop));
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s,a) =>
                {
                    for (int i = 0; i < pathArray.Length; ++i)
                    {
                        string path = pathArray.GetValue(i).ToString();
                        string extentions = System.IO.Path.GetExtension(path);
                        if (extentions != ".txt")
                        {
                            Export(path);
                        }
                        else
                        {
                            Import(path);
                        }
                        Dispatcher.Invoke(new Action(() =>
                        {
                            log.Text += string.Format("\t[{0}/{1}]\n", i + 1, pathArray.Length);
                        }));
                    }
                };
            worker.RunWorkerAsync();
        }
        
        //导入，貌似当初写了一两个
        private void Import(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    string source = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path);
                    if (!File.Exists(source))
                    {
                        throw new Exception("");
                    }
                    using (FileStream stream = new FileStream(source, FileMode.Open))
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            UInt32 tag = SwapEnding(reader.ReadUInt32());
                            switch (tag)
                            {
                                case 0x43534220:
                                    var list = getCsbP(reader);
                                    List<string> text = GetText(path);
                                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                                    byte[] data = reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length));
                                    writeCsbFile(list, text, path, data);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void writeCsbFile(List<uint> list, List<string> text, string path, byte[] data)
        {
            string dir = Path.GetDirectoryName(path);
            string name = Path.GetFileNameWithoutExtension(path);
            if(!Directory.Exists(dir + "\\" + "Csb导入"))
            {
                Directory.CreateDirectory(dir + "\\" + "Csb导入");
            }
            using (FileStream stream = new FileStream(dir + "\\" + "Csb导入" + "\\" + name, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    List<uint> addr = new List<uint>();
                    byte zero = 0x0;
                    writer.Write(data);
                    //foreach (var t in text)
                    //{
                    //    int address, offset;
                    //    address = Convert.ToInt32(t.Key, 16);
                    //    string tx = t.Value.Replace("\r\n", "\n");
                    //    List<byte> te = new List<byte>();
                    //    for (int i = 0; i < tx.Length; i++)
                    //    {
                    //        try
                    //        {
                    //            var c = tx[i];
                    //            string s = Convert.ToString(tx[i]);
                    //            var kp = tblArr.First(k => k.Value == s);
                    //            string code = kp.Key;
                    //            if (Convert.ToUInt32(code, 16) < 255u)
                    //            {
                    //                te.Add(Convert.ToByte(code, 16));
                    //            }
                    //            else
                    //            {
                    //                te.Add(Convert.ToByte(code.Substring(0, 2), 16));
                    //                te.Add(Convert.ToByte(code.Substring(2), 16));
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            //System.Windows.MessageBox.Show(string.Format("{0}中的 {1} 在码表中没有对应的编码", tx, tx[i]));
                    //            te.Add(0x20);
                    //        }
                    //    }
                    //    offset = Convert.ToInt32(writer.BaseStream.Position) - 0x20;
                    //    addr.Add(address, offset);
                    //    writer.Write(te.ToArray());
                    //    writer.Write(Convert.ToByte(0));
                    //}
                    foreach (var t in text)
                    {
                        addr.Add(Convert.ToUInt32(writer.BaseStream.Position));
                        for (int i = 0; i < t.Length; i++)
                        {
                            try
                            {
                                var c = t[i];
                                string s = Convert.ToString(t[i]);
                                var kp = tblArr.First(k => k.Value == s);
                                string code = kp.Key;
                                if (Convert.ToUInt32(code, 16) < 255u)
                                {
                                    writer.Write(Convert.ToByte(code, 16));
                                }
                                else
                                {
                                    writer.Write(Convert.ToByte(code.Substring(0, 2), 16));
                                    writer.Write(Convert.ToByte(code.Substring(2), 16));
                                }
                            }
                            catch (Exception ex)
                            {
                                //System.Windows.MessageBox.Show(string.Format("{0}中的 {1} 在码表中没有对应的编码", tx, tx[i]));
                                writer.Write(0x20);
                            }
                        }
                        //writer.Write(Encoding.UTF8.GetBytes(t));
                        writer.Write(zero);
                    }
                    int i = 0;
                    foreach (var item in list)
                    {
                        writer.Seek(Convert.ToInt32(item), SeekOrigin.Begin);
                        writer.Write(SwapEnding(addr[i]));
                        i++;
                    }
                }
            }
        }

        private List<string> GetText(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream,Encoding.UTF8))
                {
                    List<string> text = new List<string>();
                    string para = string.Empty;
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (line.StartsWith("##"))
                        {
                            para = string.Empty;
                        }
                        else if (line.EndsWith("{end}") || line.EndsWith("{END}"))
                        {
                            para += line;
                            para = para.Replace("{end}", "");
                            para = para.Replace("{END}", "");
                            text.Add(para);
                        }
                        else
                        {
                            para += line + "\n";
                        }
                    }
                    return text;
                }
            }
        }


        //导出入口
        private void Export(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    using (FileStream stream = new FileStream(path, FileMode.Open))
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            string fileName = System.IO.Path.GetFileName(path);
                            Dispatcher.Invoke(new Action(() =>
                            {
                                log.Text += string.Format("{0}", fileName);
                            }));
                            UInt32 lpAddr = 0, lpText;
                            List<UInt32> addrArray = null;
                            List<string> textArray = null;
                            List<FixedDataStruct> fixDataArr = null;
                            UInt32 tag = SwapEnding(reader.ReadUInt32());
                            //根据文件开头表示区别处理文件
                            switch(tag)
                            {
                                case 0x4C4F474F:
                                    //0x3C,索引地址
                                    reader.BaseStream.Seek(0x3C, SeekOrigin.Begin);
                                    lpAddr = SwapEnding(reader.ReadUInt32());
                                    //0x4C,文本地址
                                    reader.BaseStream.Seek(0x4C, SeekOrigin.Begin);
                                    lpText = SwapEnding(reader.ReadUInt32());
                                    addrArray = GetAddr(lpAddr, reader);
                                    textArray = GetText(addrArray, lpText, reader);
                                    WriteText(textArray, path);
                                    Dispatcher.Invoke(new Action(() =>
                                    {
                                        log.Text += string.Format("...Done!");
                                    }));
                                    break;
                                case 0x4C444249:
                                    reader.BaseStream.Seek(0x14, SeekOrigin.Begin);
                                    lpAddr = SwapEnding(reader.ReadUInt32());
                                    addrArray = GetStoryAddr(lpAddr, reader);
                                    textArray = GetStoryText(addrArray, reader);
                                    WriteText(textArray, path);
                                    Dispatcher.Invoke(new Action(() =>
                                    {
                                        log.Text += string.Format("...Done!");
                                    }));
                                    break;
                                    //FIXH，FixedData
                                case 0x46495848:
                                    //先判断0x10处是否DATA段
                                    reader.BaseStream.Seek(0x10, SeekOrigin.Begin);
                                    UInt32 t = SwapEnding(reader.ReadUInt32());
                                    if (t == 0x44415441)
                                    {
                                        //计算索引位置
                                        lpAddr = SwapEnding(reader.ReadUInt32()) + Convert.ToUInt32(reader.BaseStream.Position) + 0x8;
                                    }
                                    else
                                    {
                                        //找到DATA段
                                        lpAddr = SwapEnding(reader.ReadUInt32()) + Convert.ToUInt32(reader.BaseStream.Position) + 0x4;
                                        reader.BaseStream.Seek(lpAddr,SeekOrigin.Begin);
                                        lpAddr = SwapEnding(reader.ReadUInt32()) + Convert.ToUInt32(reader.BaseStream.Position) + 0x8;
                                    }
                                    addrArray = GetFixDataAddr(lpAddr, reader);
                                    fixDataArr = GetFixDataText(addrArray, reader);
                                    WriteFixDataText(fixDataArr, path);
                                    Dispatcher.Invoke(new Action(() =>
                                    {
                                        log.Text += string.Format("...Done!");
                                    }));
                                    break;
                                    //CSB
                                case 0x43534220:
                                    //0x1C,LNP段地址
                                    reader.BaseStream.Seek(0x1C, SeekOrigin.Begin);
                                    UInt32 LNPOffset = SwapEnding(reader.ReadUInt32()) + 0x18;
                                    reader.BaseStream.Seek(LNPOffset, SeekOrigin.Begin);
                                    UInt32 LNPTag = SwapEnding(reader.ReadUInt32());
                                    if (LNPTag != 0x4C4E5020)
                                    {
                                        throw new Exception("不是一个LNP段");
                                    }
                                    //LNP段：4字节TAG，4字节LNT偏移地址
                                    UInt32 LNTOffset = SwapEnding(reader.ReadUInt32()) + LNPOffset;
                                    reader.BaseStream.Seek(LNTOffset, SeekOrigin.Begin);
                                    UInt32 LNTTag = SwapEnding(reader.ReadUInt32());
                                    if (LNTTag != 0x4C4E5420)
                                    {
                                        throw new Exception("不是一个LNT段");
                                    }
                                    UInt32 LNTLength = SwapEnding(reader.ReadUInt32());
                                    UInt32 LNTCount = SwapEnding(reader.ReadUInt32());
                                    List<UInt32> LNTArr = GetLNTArray(reader, LNTCount, reader.BaseStream.Position);
                                    textArray = GetCSBArray(reader, LNTArr);
                                    WriteCSBText(textArray, path);
                                    Dispatcher.Invoke(new Action(() =>
                                    {
                                        log.Text += string.Format("...Done!");
                                    }));
                                    break;
                                default:
                                    Dispatcher.Invoke(new Action(() =>
                                    {
                                        log.Text += string.Format("...文件格式不符");
                                    }));
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string fileName = System.IO.Path.GetFileName(path);
                Dispatcher.Invoke(new Action(() =>
                {
                    log.Text += string.Format("读取 {0} 文件出错 ： {1}\n", fileName, ex.Message);
                }));
            }
        }

        private void WriteCSBText(List<string> textArray, string path)
        {
            if (textArray != null && textArray.Count > 0)
            {
                try
                {
                    string npath = path += ".txt";
                    using (FileStream stream = new FileStream(npath, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            int i = 0;
                            foreach (var text in textArray)
                            {
                                writer.WriteLine("#### {0} ####", i);
                                writer.Write("{0}", text.Replace("\n", "\r\n"));
                                writer.WriteLine("{end}");
                                writer.WriteLine();
                                i++;
                            }
                            writer.Flush();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string fileName = System.IO.Path.GetFileName(path);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        log.Text += string.Format("写入 {0} 文件出错 ： {1}\n", fileName, ex.Message);
                    }));
                }
            }
        }

        private List<string> GetCSBArray(BinaryReader reader, List<uint> LNTArr)
        {
            List<string> textArray = new List<string>();
            foreach (var lnt in LNTArr)
            {
                reader.BaseStream.Seek(lnt, SeekOrigin.Begin);
                UInt32 count = SwapEnding(reader.ReadUInt32());
                UInt32 pos = SwapEnding(reader.ReadUInt32());
                List<uint> tmp = GetLNTArray(reader, count, pos);
                foreach(var t in tmp)
                {
                    textArray.Add(GetText(reader,t));
                }
            }
            return textArray;
        }

        private string GetText(BinaryReader reader, uint t)
        {
            reader.BaseStream.Seek(t, SeekOrigin.Begin);
            List<byte> str = new List<byte>();
            while (true)
            {
                byte buff = reader.ReadByte();
                if (buff == 0x0)
                {
                    break;
                }
                str.Add(buff);
            }
            return Encoding.UTF8.GetString(str.ToArray());
        }

        private List<uint> GetLNTArray(BinaryReader reader, uint LNTCount, long p)
        {
            List<UInt32> LNTArr = new List<UInt32>();
            reader.BaseStream.Seek(p, SeekOrigin.Begin);
            for (int i = 0; i < LNTCount; ++i)
            {
                LNTArr.Add(SwapEnding(reader.ReadUInt32()));
            }
            return LNTArr;
        }

        private void WriteFixDataText(List<FixedDataStruct> fixDataArr, string path)
        {
            if (fixDataArr != null && fixDataArr.Count > 0)
            {
                try
                {
                    string npath = path += ".txt";
                    using (FileStream stream = new FileStream(npath, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            int i = 0;
                            foreach (var text in fixDataArr)
                            {
                                ushort num1 = SwapEnding(BitConverter.ToUInt16(text.prev, 0x0));
                                ushort num2 = SwapEnding(BitConverter.ToUInt16(text.prev, 0x2));
                                ushort num3 = SwapEnding(BitConverter.ToUInt16(text.prev, 0x4));
                                writer.WriteLine("#### [{0:X4}{1:X4}{2:X4}]-{3} ####", num1, num2, num3, i);
                                writer.Write("{0}", text.text.Replace("@", "\r\n"));
                                writer.WriteLine("{end}");
                                writer.WriteLine();
                                i++;
                            }
                            writer.Flush();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string fileName = System.IO.Path.GetFileName(path);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        log.Text += string.Format("写入 {0} 文件出错 ： {1}\n", fileName, ex.Message);
                    }));
                }
            }
        }

        ushort SwapEnding(ushort p)
        {
            return BitConverter.ToUInt16(BitConverter.GetBytes(p).Reverse().ToArray(),0);
        }

        private List<FixedDataStruct> GetFixDataText(List<UInt32> addrArray, BinaryReader reader)
        {
            List<FixedDataStruct> textArray = new List<FixedDataStruct>();
            while (true)
            {
                UInt32 tag = SwapEnding(reader.ReadUInt32());
                if (tag == 0x53545249)
                {
                    break;
                }
            }
            reader.BaseStream.Seek(0x8, SeekOrigin.Current);
            UInt32 offset = Convert.ToUInt32(reader.BaseStream.Position);
            if (addrArray != null && addrArray.Count > 0)
            {
                foreach (var addr in addrArray)
                {
                    FixedDataStruct fixedData = new FixedDataStruct();
                    UInt32 realAddr = addr + offset;
                    reader.BaseStream.Seek(realAddr, SeekOrigin.Begin);
                    fixedData.prev = reader.ReadBytes(0x6);
                    List<byte> textBuff = new List<byte>();
                    while (true)
                    {
                        byte buff = reader.ReadByte();
                        if (buff == 0x0)
                        {
                            break;
                        }
                        textBuff.Add(buff);
                    }
                    fixedData.text = Encoding.UTF8.GetString(textBuff.ToArray());
                    textArray.Add(fixedData);
                }
            }
            return textArray;
        }

        private List<UInt32> GetFixDataAddr(UInt32 lpAddr, BinaryReader reader)
        {

            List<UInt32> addrArray = new List<UInt32>();
            reader.BaseStream.Seek(lpAddr, SeekOrigin.Begin);
            //读取索引长度
            UInt32 length = SwapEnding(reader.ReadUInt32());
            //索引数目
            UInt32 count = length / 4;
            for (UInt32 i = 0; i < count; ++i)
            {
                UInt32 addr = SwapEnding(reader.ReadUInt32());
                addrArray.Add(addr);
            }
            return addrArray;
        }

        private List<string> GetStoryText(List<UInt32> addrArray, BinaryReader reader)
        {
            List<string> textAddr = new List<string>();
            if (addrArray != null && addrArray.Count > 0)
            {
                foreach (var addr in addrArray)
                {
                    reader.BaseStream.Seek(addr, SeekOrigin.Begin);
                    List<byte> textBuff = new List<byte>();
                    while (true)
                    {
                        byte buff = reader.ReadByte();
                        if (buff == 0x0)
                        {
                            break;
                        }
                        textBuff.Add(buff);
                    }
                    string text = Encoding.UTF8.GetString(textBuff.ToArray());
                    textAddr.Add(text);
                }
            }
            return textAddr;
        }

        private List<UInt32> GetStoryAddr(UInt32 lpAddr, BinaryReader reader)
        {
            List<UInt32> addrArray = new List<UInt32>();
            reader.BaseStream.Seek(lpAddr, SeekOrigin.Begin);
            while (true)
            {
                UInt32 addr = SwapEnding(reader.ReadUInt32());
                if (addr == 0x0)
                {
                    break;
                }
                addrArray.Add(addr);
            }
            return addrArray;
        }

        private void WriteText(List<string> textArray, string path)
        {
            if (textArray != null && textArray.Count > 0)
            {
                try
                {
                    string npath = path += ".txt";
                    using (FileStream stream = new FileStream(npath, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            int i = 0;
                            foreach (var text in textArray)
                            {
                                writer.WriteLine("#### {0} ####",i);
                                writer.Write("{0}", text.Replace("@", "\r\n"));
                                writer.WriteLine("{end}");
                                writer.WriteLine();
                                i++;
                            }
                            writer.Flush();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string fileName = System.IO.Path.GetFileName(path);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        log.Text += string.Format("写入 {0} 文件出错 ： {1}\n", fileName, ex.Message);
                    }));
                }
            }
        }

        private List<string> GetText(List<UInt32> addrArray, UInt32 lpText, BinaryReader reader)
        {
            List<string> textAddr = new List<string>();
            if(addrArray != null && addrArray.Count > 0)
            {
                foreach(var addr in addrArray)
                {
                    if (addr != addrArray[addrArray.Count - 1])
                    {
                        UInt32 address = addr + lpText;
                        reader.BaseStream.Seek(address, SeekOrigin.Begin);
                        List<byte> textBuff = new List<byte>();
                        while (true)
                        {
                            byte buff = reader.ReadByte();
                            if (buff == 0x0)
                            {
                                break;
                            }
                            textBuff.Add(buff);
                        }
                        string text = Encoding.UTF8.GetString(textBuff.ToArray());
                        textAddr.Add(text);
                    }
                }
            }
            return textAddr;
        }

        private List<UInt32> GetAddr(UInt32 lpAddr, BinaryReader reader)
        {
            List<UInt32> addrArray = new List<UInt32>();
            reader.BaseStream.Seek(0x40,SeekOrigin.Begin);
            UInt32 l1 = SwapEnding(reader.ReadUInt32());
            reader.BaseStream.Seek(0x50, SeekOrigin.Begin);
            UInt32 l2 = SwapEnding(reader.ReadUInt32());
            UInt32 count = l1 + l2;
            reader.BaseStream.Seek(lpAddr,SeekOrigin.Begin);
            for (UInt32 i = 0; i < count; ++i)
            {
                UInt32 addr = SwapEnding(reader.ReadUInt32());
                addrArray.Add(addr);
            }
            return addrArray;
        }

        private UInt32 SwapEnding(UInt32 p)
        {
            return ((p & 0xFF000000) >> 24) |
                    ((p & 0x00FF0000) >> 8) |
                    ((p & 0x0000FF00) << 8) |
                    ((p & 0x000000FF) << 24);
        }

        private void log_PreviewDragEnter_1(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }


        private List<uint> getCsbP(BinaryReader reader)
        {
            List<uint> addr = new List<uint>();
            reader.BaseStream.Seek(0x1C, SeekOrigin.Begin);
            UInt32 LNPOffset = SwapEnding(reader.ReadUInt32()) + 0x18;
            reader.BaseStream.Seek(LNPOffset, SeekOrigin.Begin);
            UInt32 LNPTag = SwapEnding(reader.ReadUInt32());
            if (LNPTag != 0x4C4E5020)
            {
                throw new Exception("不是一个LNP段");
            }
            UInt32 LNTOffset = SwapEnding(reader.ReadUInt32()) + LNPOffset;
            reader.BaseStream.Seek(LNTOffset, SeekOrigin.Begin);
            UInt32 LNTTag = SwapEnding(reader.ReadUInt32());
            if (LNTTag != 0x4C4E5420)
            {
                throw new Exception("不是一个LNT段");
            }
            UInt32 LNTLength = SwapEnding(reader.ReadUInt32());
            UInt32 LNTCount = SwapEnding(reader.ReadUInt32());
            List<uint> addrOff = GetLNTArray(reader, LNTCount, reader.BaseStream.Position);
            foreach (var item in addrOff)
            {
                reader.BaseStream.Seek(item,SeekOrigin.Begin);
                uint count = SwapEnding(reader.ReadUInt32());
                uint pos = SwapEnding(reader.ReadUInt32());
                reader.BaseStream.Seek(pos, SeekOrigin.Begin);
                for (int i = 0; i < count; ++i)
                {
                    addr.Add(Convert.ToUInt32(reader.BaseStream.Position));
                    reader.ReadUInt32();
                }
            }
            return addr;
        }

        private void GetTbl(string tblStr)
        {
            this.tblArr.Clear();
            if (!string.IsNullOrEmpty(tblStr))
            {
                using (FileStream fileStream = new FileStream(tblStr, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Unicode))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            string text = streamReader.ReadLine();
                            if (!string.IsNullOrEmpty(text))
                            {
                                string[] array = text.Split(new char[]
								{
									'='
								});
                                if (!string.IsNullOrEmpty(array[1]))
                                {
                                    this.tblArr.Add(array[0], array[1]);
                                }
                            }
                        }
                        this.tblArr["8140"] = "\n";
                    }
                }
            }
        }
    }
}
