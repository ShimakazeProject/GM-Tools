using System.Collections.Generic;
using System.IO;

namespace IniChecker
{
    public class IniCheck
    {
        private List<SectionInfo> SectionName = new List<SectionInfo>();// 全部节
        private List<SectionInfo> ReUsedSection = new List<SectionInfo>();// 重复使用节
        private List<SectionInfo> UnUsedSection = new List<SectionInfo>();// 未使用节
        private List<string> UsedSection = new List<string>();// 被使用的节
        private List<string> Regist = new List<string>();// 注册表头
        private List<string> Key = new List<string>();// 键
        private List<string> White = new List<string>();// 白名单
        public struct Except
        {
            public string Exception { get; private set; }
            public string Section { get; private set; }
            public string FileName { get; private set; }
            public string LineNumber { get; private set; }
            public string FilePath { get; private set; }
            public Except(string avg1, string avg2, string avg3, string avg4,string path)
            {
                Exception = avg1;
                Section = "[" + avg2 + "]";
                FileName = avg3;
                LineNumber = avg4;
                FilePath = path;
            }
            public Except(string avg1, SectionInfo avg2)
            {
                Exception = avg1;
                Section = avg2.Section;
                FileName = avg2.FileName;
                LineNumber = avg2.LineNumber;
                FilePath = avg2.FilePath;
            }
        }
        public struct SectionInfo
        {
            public string Section { get; private set; }
            public string FileName { get; private set; }
            public string LineNumber { get; private set; }
            public string FilePath { get; private set; }
            public SectionInfo( string avg2, string avg3, string avg4, string path)
            {
                Section = "[" + avg2 + "]";
                FileName = avg3;
                LineNumber = avg4;
                FilePath = path;
            }
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="Regist">注册表Section</param>
        /// <param name="White">白名单Section</param>
        /// <param name="Key">使用标注Key</param>
        public IniCheck(List<string> Regist, List<string> White, List<string> Key)
        {
            foreach (var item in White) this.White.Add(item.Trim().ToLower());
            foreach (var item in Regist) this.Regist.Add(item.Trim().ToLower());
            foreach (var item in Key) this.Key.Add(item.Trim().ToLower());
            UsedSection.AddRange(this.White);
        }
        /// <summary>
        /// 分析并将所得结果写入类字段
        /// </summary>
        /// <param name="paths">文件地址</param>
        public void Initiation(string[] paths)
        {
            foreach (string path in paths)
            {
                string fullName = new FileInfo(path).FullName;
                var fs = new FileStream(path, FileMode.Open);
                var sr = new StreamReader(fs);
                var readLock = false;// 关闭读取
                for (int i = 1; !sr.EndOfStream; i++)
                {
                    var tmpline = sr.ReadLine().Trim();
                    if (tmpline.StartsWith(";") || string.IsNullOrWhiteSpace(tmpline)) continue;
                    var line = tmpline.Split(';')[0];
                    if (line.StartsWith("["))// 按Section 注册表区分
                    {
                        readLock = false;// 关闭读取
                        var section = line.Substring(line.IndexOf('[') + 1, line.IndexOf(']') - 1).Trim();
                        if (Regist.Contains(section.ToLower())) readLock = true;// 排除注册表节 开启读取
                        else SectionName.Add(new SectionInfo(section, fullName, i.ToString(), path));
                    }
                    else if (line.Contains("="))// 按Key引用区分
                    {
                        var keyValue = line.Split('=');
                        keyValue[0] = keyValue[0].Trim().ToLower();
                        keyValue[1] = keyValue[1].Trim().ToLower();
                        if (keyValue.Length < 2) continue;
                        if (readLock)
                        {
                            if (!UsedSection.Contains(keyValue[1])) UsedSection.Add(keyValue[1]);
                        }
                        else if (KeyDist(keyValue[0])) UsedSection.Add(keyValue[1]);
                        continue;
                    }
                }
            }
        }
        /// <summary>
        /// 分析并将所得结果写入类字段
        /// </summary>
        /// <param name="paths">文件地址</param>
        public void InitiationArt(string[] paths)
        {
            foreach (string path in paths)
            {
                string fullName = new FileInfo(path).FullName;
                var fs = new FileStream(path, FileMode.Open);
                var sr = new StreamReader(fs);
                var readLock = false;// 关闭读取
                for (int i = 1; !sr.EndOfStream; i++)
                {
                    var tmpline = sr.ReadLine().Trim();
                    if (tmpline.StartsWith(";") || string.IsNullOrWhiteSpace(tmpline)) continue;
                    var line = tmpline.Split(';')[0];
                    if (line.Contains("="))// 按Key引用区分
                    {
                        var keyValue = line.Split('=');
                        keyValue[0] = keyValue[0].Trim().ToLower();
                        keyValue[1] = keyValue[1].Trim().ToLower();
                        if (keyValue.Length < 2) continue;
                        if (readLock)
                        {
                            if (!UsedSection.Contains(keyValue[1])) UsedSection.Add(keyValue[1]);
                        }
                        else if (KeyDist(keyValue[0])) UsedSection.Add(keyValue[1]);
                        continue;
                    }
                }
            }
        }
        /// <summary>
        /// 运行分析
        /// </summary>
        /// <returns>返回的异常信息</returns>
        public List<Except> Anazlyze()
        {
            var tmpSection = new List<SectionInfo>();
            foreach (var Section in SectionName)
            {
                if (tmpSection.Contains(Section)) ReUsedSection.Add(Section);
                else if (!UsedSection.Contains(Section.Section.ToLower().Replace("[", string.Empty).Replace("]", string.Empty))) UnUsedSection.Add(Section);
                tmpSection.Add(Section);
            }
            return GetExcepts();
        }
        private List<Except> GetExcepts()
        {
            var returnExcept = new List<Except>();
            foreach (var item in UnUsedSection) returnExcept.Add(new Except("未使用", item));
            foreach (var item in ReUsedSection) returnExcept.Add(new Except("重复使用", item));
            if (returnExcept.Count == 0) return null;
            else return returnExcept;
        }
        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="key">要检查的键</param>
        /// <returns>是否存在</returns>
        private bool KeyDist(string key)
        {
            if (Key.Contains(key)) return true;
            foreach (var Key in Key) if (key.Contains(Key)) return true;
            return false;
        }
    }
}
