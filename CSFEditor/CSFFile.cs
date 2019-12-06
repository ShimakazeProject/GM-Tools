using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFEditor
{
    class CSFFile
    {
        public bool IsReady { get; private set; }
        #region 文件属性
        /// <summary>
        /// 文件头
        /// </summary>
        public Header Header { get; private set; }
        /// <summary>
        /// 标签列表
        /// </summary>
        public List<Label> Label { get; private set; }
        #endregion
        /// <summary>
        /// 修改文件头
        /// </summary>
        /// <param name="header">文件头</param>
        public void ChangeHeader(Header header)
        {
            Header = header;
        }
        /// <summary>
        /// 修改一个标签
        /// </summary>
        /// <param name="label">文件标签</param>
        /// <param name="index">索引</param>
        public void ChangeLabel(Label label,int index)
        {
            Label[index] = label;
        }
        /// <summary>
        /// 增加一个标签
        /// </summary>
        /// <param name="label">标签</param>
        public void AddLabel(Label label)
        {
            Label.Add(label);
            Header.AddLabel(label.StringNum);
        }
        /// <summary>
        /// 删除一个标签
        /// </summary>
        /// <param name="label"></param>
        public void DropLabel(Label label)
        {
            Label.Remove(label);
            Header.DropLabel(label.StringNum);
        }
        /// <summary>
        /// ! 清空文件标签列表
        /// </summary>
        public void CleanLabels()
        {
            Label = new List<Label>();
            Label.Clear();
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="Path">文件路径</param>
        public async Task LoadFromFile(string Path)
        {
            IsReady = false;
            Label = new List<Label>();// 初始化
            FileStream fs = new FileStream(Path, FileMode.Open);
            // =================== 文件头 =================
            byte[] FlagBytes = new byte[4];// 文件头
            byte[] VersionBytes = new byte[4];// 版本
            byte[] NumLabelsBytes = new byte[4];// 标签数
            byte[] NumStrings = new byte[4];// 字符串数
            byte[] NullBytes = new byte[4];// 无用内容
            byte[] LanguageBytes = new byte[4];// 语言
            // ========= 异步读取字节数组 ===========
            await fs.ReadAsync(FlagBytes, 0, 4);
            await fs.ReadAsync(VersionBytes, 0, 4);
            await fs.ReadAsync(NumLabelsBytes, 0, 4);
            await fs.ReadAsync(NumStrings,0, 4);
            await fs.ReadAsync(NullBytes, 0, 4);
            await fs.ReadAsync(LanguageBytes, 0, 4);
            // ========= 数据处理 =========
            string Flag = Encoding.ASCII.GetString(FlagBytes);
            int headVersion= BitConverter.ToInt32(VersionBytes, 0);
            int headLabelNum = BitConverter.ToInt32(NumLabelsBytes, 0);
            int headStringNum = BitConverter.ToInt32(NumStrings, 0);
            int headLanguage = BitConverter.ToInt32(LanguageBytes, 0);
            // 文件头信息写入
            Header = new Header(Flag, headVersion, headLabelNum, headStringNum, NullBytes, headLanguage);
            // ===============================
            int stringnum = 0;
            for (int labelno = 0; labelno < headLabelNum; labelno++)
            {
                // =========== 先声明一堆看起来很厉害的字段
                byte[] lblTag = new byte[4];// 标签标记
                byte[] lblNum = new byte[4];// 标签字符串数
                byte[] lblLen = new byte[4];// 标签长
                byte[] lblVal = new byte[4];// 标签字符串
                byte[] vleTag = new byte[4];// 值标记
                byte[] vleLen = new byte[4];// 值字符串长度
                byte[] vleStr = new byte[4];// 值字符串
                byte[] vleEle = new byte[4];// 额外值长度
                byte[] vleEst = new byte[4];// 额外值字符串
                // ========= 标签
                await fs.ReadAsync(lblTag, 0, 4);// 异步读文件字节 标签头
                await fs.ReadAsync(lblNum, 0, 4);// 异步读文件字节 字符串对数
                await fs.ReadAsync(lblLen, 0, 4);// 异步读文件字节 标签长
                string ltag = Encoding.ASCII.GetString(lblTag);// 字节数组到字符串
                int lnum = BitConverter.ToInt32(lblNum, 0);// 字节数组到整数
                int llen = BitConverter.ToInt32(lblLen, 0);// byte[] => int
                lblVal = new byte[llen];// 重实例化标签内容 修正长度
                await fs.ReadAsync(lblVal, 0, llen);// 异步读
                string lstr = Encoding.ASCII.GetString(lblVal);// byte[]=>string
                // ========= 值
                await fs.ReadAsync(vleTag, 0, 4);
                await fs.ReadAsync(vleLen, 0, 4);
                string vtag = Encoding.ASCII.GetString(vleTag);
                List<int> vlens = new List<int>();
                List<string> vstrs = new List<string>();
                for (int i = 0; i < lnum; i++)
                {
                    int vlen = BitConverter.ToInt32(vleLen, 0);
                    vleStr = new byte[2 * vlen];
                    await fs.ReadAsync(vleStr, 0, 2 * vlen);
                    string vstr = Encoding.Unicode.GetString(Decoding(vlen, vleStr));
                    vlens.Add(vlen);
                    vstrs.Add(vstr);
                    stringnum++;
                }
                if (vtag == "WRTS")
                {
                    await fs.ReadAsync(vleEle, 0, 4);
                    int vele = BitConverter.ToInt32(vleEle, 0);
                    vleEst = new byte[vele];
                    await fs.ReadAsync(vleEst, 0, vele);
                    string vest = Encoding.ASCII.GetString(vleEst);

                // ==================== 将结果写入属性 =========================
                    Label.Add(new Label(ltag, lnum, llen, lstr, vtag, vlens.ToArray(), vstrs.ToArray(), vele, vest));
                }
                else Label.Add(new Label(ltag, lnum, llen, lstr, vtag, vlens.ToArray(), vstrs.ToArray()));
                //// 调试输出
                //System.Diagnostics.Debug.Write("\n" + labelno.ToString() + "\t");
                //System.Diagnostics.Debug.Write(vstrs[0]);
            }
            IsReady = true;
            // ====== 可能出现的异常 ======
            if (Flag != " FSC") System.Diagnostics.Debug.Write("不是有效的csf文件"); // 不是有效的csf文件 尚未定义异常
            if (stringnum != headStringNum) System.Diagnostics.Debug.Write("喵喵喵???");// 字符串数对不上 尚未定义异常
            fs.Close();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="Path">文件路径</param>
        public async Task SaveToFile(string Path)
        {
            FileStream fs = new FileStream(Path, FileMode.Create);
            byte[] FlagBytes = Encoding.ASCII.GetBytes(Header.Flag);
            byte[] VersionBytes = BitConverter.GetBytes(Header.Version);
            byte[] NumLabelsBytes = BitConverter.GetBytes(Header.NumLabel);
            byte[] NumStrings = BitConverter.GetBytes(Header.NumString);
            byte[] NullBytes = Header.Message;
            byte[] LanguageBytes = BitConverter.GetBytes(Header.Language);
            var tmplist = new List<byte>();
            tmplist.AddRange(FlagBytes);
            tmplist.AddRange(VersionBytes);
            tmplist.AddRange(NumLabelsBytes);
            tmplist.AddRange(NumStrings);
            tmplist.AddRange(NullBytes);
            tmplist.AddRange(LanguageBytes);
            if (tmplist.Count!=24) System.Diagnostics.Debug.Write("错误的文件头"); // 错误的文件头 尚未定义异常
            await fs.WriteAsync(tmplist.ToArray(), 0, tmplist.Count);
            // ====== 文件头写完了
            foreach (var label in Label)
            {
                byte[] LabelTag = Encoding.ASCII.GetBytes(label.LabelTag);
                byte[] StringNum = BitConverter.GetBytes(label.StringNum);
                byte[] LabelLength = BitConverter.GetBytes(label.LabelLength);
                byte[] LabelString = Encoding.ASCII.GetBytes(label.LabelString);
                byte[] ValueTag = Encoding.ASCII.GetBytes(label.ValueTag);
                byte[] VLVSs = new byte[0];
                byte[] ExtraValueLength = new byte[0];
                byte[] ExtraValue = new byte[0];
                for (int i = 0; i < label.ValueLength.Length; i++)
                {
                    var tmp = VLVSs.ToList();
                    byte[] vl = BitConverter.GetBytes(label.ValueLength[i]);
                    byte[] vs = Decoding(label.ValueLength[i], Encoding.Unicode.GetBytes(label.ValueString[i]));                    
                    tmp.AddRange(vl);
                    tmp.AddRange(vs);
                    VLVSs = tmp.ToArray();
                }
                if (label.ExtraValueLength!=null)
                {
                    ExtraValueLength = BitConverter.GetBytes((int)label.ExtraValueLength);
                    ExtraValue = Encoding.ASCII.GetBytes(label.ExtraValue);
                }
                tmplist = new List<byte>();
                tmplist.AddRange(LabelTag);
                tmplist.AddRange(StringNum);
                tmplist.AddRange(LabelLength);
                tmplist.AddRange(LabelString);
                tmplist.AddRange(ValueTag);
                tmplist.AddRange(VLVSs);
                tmplist.AddRange(ExtraValueLength);
                tmplist.AddRange(ExtraValue);
                await fs.WriteAsync(tmplist.ToArray(), 0, tmplist.Count);
            }
            fs.Close();
        }
        /// <summary>
        /// 导出txt
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public async Task SaveAsText(string Path)
        {
            FileStream fs = new FileStream(Path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            await sw.WriteLineAsync("=============== Header ==============");
            await sw.WriteLineAsync("Flag :" + Header.Flag);
            await sw.WriteLineAsync("Version:" + Header.Version.ToString());
            await sw.WriteLineAsync("LabelNumber:" + Header.NumLabel.ToString());
            await sw.WriteLineAsync("StringNumber:" + Header.NumString.ToString());
            string result = string.Empty;
            foreach(var b in Header.Message)//逐字节变为16进制字符
            {
                result += Convert.ToString(b, 16) + " ";
            }
            await sw.WriteLineAsync("Message:" + result);
            await sw.WriteLineAsync("Language:" + Header.Language);
            await sw.WriteLineAsync("=============== Header ==============");
            // ====== 文件头写完了
            foreach (var label in Label)
            {
                await sw.WriteAsync(label.LabelString + " | ");
                foreach (var str in label.ValueString) await sw.WriteAsync(str + "|");
                await sw.WriteLineAsync();
            }
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        /// <summary>
        /// 搜索字符串标签
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public int[] SearchLabel(string label)
        {
            List<int> returni1 = new List<int>();
            List<int> returni2 = new List<int>();
            for (int i = 0; i < Label.Count; i++)
            {
                if (Label[i].LabelString.ToUpper() == label.ToUpper())// 全字匹配
                {
                    returni1.Add(i);
                    continue;
                }
                else if (Label[i].LabelString.ToUpper().Contains(label.ToUpper()))// 关键字匹配
                {
                    returni2.Add(i);
                    continue;
                }                
            }
            returni1.AddRange(returni2);
            return returni1.ToArray();
        }
        /// <summary>
        /// 搜索字符串内容
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public int[] SearchString(string @string)
        {
            List<int> returni1 = new List<int>();
            List<int> returni2 = new List<int>();
            for (int i = 0; i < Label.Count; i++)
            {
                foreach (var str in Label[i].ValueString)
                {
                    if (str.ToUpper() == @string.ToUpper())// 全字匹配
                    {
                        returni1.Add(i);
                        break;
                    }
                    else if (str.ToUpper().Contains(@string.ToUpper()))// 关键字匹配
                    {
                        returni2.Add(i);
                        break;
                    }
                }
            }
            returni1.AddRange(returni2);
            return returni1.ToArray();
        }



        /// <summary>
        /// 编/解码
        /// </summary>
        /// <param name="ValueLength">长度</param>
        /// <param name="ValueData">内容</param>
        /// <returns>编/解码后的数组</returns>
        private byte[] Decoding(int ValueLength,byte[] ValueData)
        {
            int ValueDataLength = ValueLength << 1;
            for(int i = 0; i < ValueDataLength; ++i){
                ValueData[i] =(byte)~ValueData[i];
            }
            return ValueData;
        }
    }
    /// <summary>
    /// CSF文件信息头
    /// </summary>
    public struct Header
    {
        public string Flag { get; private set; }
        public int Version { get; private set; }
        public int NumLabel { get; private set; }
        public int NumString { get; private set; }
        public byte[] Message { get; private set; }
        public int Language { get; private set; }
            /*
             * 0 =美国（英语）*
             * 1 =英国（英语）
             * 2 =德语*
             * 3 =法语*
             * 4 =西班牙语
             * 5 =义大利语
             * 6 =日语
             * 7 =贾伯沃基 
             * 8 =韩文*
             * 9 =中文*
             * > 9 =未知
             */
        public void AddLabel(int AddStrNum)
        {
            NumLabel++;
            NumString += AddStrNum;
        }
        public void DropLabel(int AddStrNum)
        {
            NumLabel--;
            NumString -= AddStrNum;
        }
        public Header(string flag, int version, int numLable, int numString, byte[] message, int language)
        {
            Flag = flag;
            Version = version;
            NumLabel = numLable;
            NumString = numString;
            Message = message;
            Language = language;
        }
    }
    /// <summary>
    /// CSF 文件内容 >标签
    /// </summary>
    public struct Label
    {
        public string LabelTag { get; private set; }
        public int StringNum { get; private set; }
        public int LabelLength { get; private set; }
        public string LabelString { get; private set; }
        public string ValueTag { get; private set; }
        public int[] ValueLength { get; private set; }
        public string[] ValueString { get; private set; }
        public int? ExtraValueLength { get; private set; }
        public string ExtraValue { get; private set; }
        public Label(string LabelTag,int StringNum,int LabelLength,string LabelString, string ValueTag, int[] ValueLength, string[] ValueString, int? ExtraValueLength = null, string ExtraValue = null)
        {
            this.LabelTag = LabelTag;
            this.StringNum = StringNum;
            this.LabelLength = LabelLength;
            this.LabelString = LabelString;
            this.ValueTag = ValueTag;
            this.ValueLength = ValueLength;
            this.ValueString = ValueString;
            this.ExtraValueLength = ExtraValueLength;
            this.ExtraValue = ExtraValue;
        }
    }
}
