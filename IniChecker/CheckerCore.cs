using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IniChecker
{
    class CheckerCore
    {
        /// <summary>
        /// 全部节
        /// </summary>
        private List<SectionInfo> SectionName = new List<SectionInfo>();
        /// <summary>
        /// 未注册节
        /// </summary>
        private List<SectionInfo> UnRegistSection = new List<SectionInfo>();
        /// <summary>
        /// 重复使用节
        /// </summary>
        private List<SectionInfo> ReUsedSection = new List<SectionInfo>();
        /// <summary>
        /// 未使用节
        /// </summary>
        private List<SectionInfo> UnUsedSection = new List<SectionInfo>();
        /// <summary>
        /// 被使用的节
        /// </summary>
        private List<SectionInfo> UsedSection = new List<SectionInfo>();
        private List<string> Regist = new List<string>();// 注册表头
        private List<string> Key = new List<string>();// 键
        private List<string> ArtKey = new List<string>();// 键
        private List<string> White = new List<string>();// 白名单

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="Regist">注册表Section</param>
        /// <param name="White">白名单Section</param>
        /// <param name="Key">使用标注Key</param>
        public CheckerCore(List<string> Regist, List<string> White, List<string> Key)
        {
            var t1 = new Task(() =>
            {
                foreach (var item in White) this.White.Add(item.Trim().ToLower());
            });
            var t2 = new Task(() =>
            {
                foreach (var item in Regist) this.Regist.Add(item.Trim().ToLower());
            });
            var t3 = new Task(() =>
            {
                foreach (var item in Key) this.Key.Add(item.Trim().ToLower());
            });
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Wait();
            t2.Wait();
            t3.Wait();
            foreach (var white in White) UsedSection.Add(new SectionInfo(white,"White List", "White List", "White List"));

            
            //UsedSection.AddRange(this.White);
        }
        #region 初始化
        /// <summary>
        /// 分析 Rules 并将所得 全部已使用节 写入类字段
        /// </summary>
        /// <param name="paths">文件地址</param>
        public async Task InitiationAsync(string[] paths)
        {
            List<Task> tasks = new List<Task>();
            foreach (string path in paths)
            {
                var t = new Task(() =>
                {
                    string fullName = new FileInfo(path).FullName;
                    var fs = new FileStream(path, FileMode.Open);
                    var sr = new StreamReader(fs);
                    var readLock = false;// 关闭读取
                    for (int i = 1; !sr.EndOfStream; i++)
                    {
                        var line = sr.ReadLine().Trim();// 读入行
                        if (line.StartsWith(";") || string.IsNullOrWhiteSpace(line)) continue;// 忽略注释行
                        line = line.Split(';')[0];// 忽略注释
                        if (line.StartsWith("["))// 按Section 区分
                        {
                            readLock = false;// 关闭读取
                            var section = line.Substring(line.IndexOf('[') + 1, line.IndexOf(']') - 1).Trim();// 取中括号中间内容
                            if (Regist.Contains(section.ToLower())) readLock = true;// 排除注册表节 开启读取
                            else SectionName.Add(new SectionInfo(section, fullName, i.ToString(), path));// 添加到全部节列表
                        }
                        else if (line.Contains("="))// 按Key引用区分
                        {
                            var keyValue = line.Split('=');// 按等号分割
                            if (keyValue.Length < 2) continue;// 异常处理
                            keyValue[0] = keyValue[0].Trim().ToLower();
                            keyValue[1] = keyValue[1].Trim().ToLower();
                            var section = new SectionInfo(keyValue[1], fullName, i.ToString(), path);
                            if (readLock)
                            {
                                if (!UsedSectionsContains(keyValue[1]))
                                {
                                    UsedSection.Add(section);
                                    UnRegistSection.Add(section);
                                }
                            }
                            else if (KeyDist(keyValue[0])) UsedSection.Add(section);
                            continue;
                        }
                    }
                    sr.Close();
                    fs.Close();
                });
                t.Start();
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
            //Task.WaitAll(tasks.ToArray());
        }
        /// <summary>
        /// 分析 Arts 并将所得 全部已使用节 写入类字段
        /// </summary>
        /// <param name="paths">文件地址</param>
        public async Task InitiationArtAsync(string[] paths)
        {
            List<Task> tasks = new List<Task>();
            foreach (string path in paths)
            {
                var t = new Task(() =>
                {
                    string fullName = new FileInfo(path).FullName;
                    var fs = new FileStream(path, FileMode.Open);
                    var sr = new StreamReader(fs);
                    var readLock = false;// 关闭读取
                    for (int i = 1; !sr.EndOfStream; i++)
                    {
                        var line = sr.ReadLine().Trim();// 读入行
                        if (line.StartsWith(";") || string.IsNullOrWhiteSpace(line)) continue;// 忽略注释行
                        line = line.Split(';')[0];// 忽略注释
                        if (line.Contains("="))// 按Key引用区分
                        {
                            var keyValue = line.Split('=');// 按等号分割
                            if (keyValue.Length < 2) continue;// 异常处理
                            keyValue[0] = keyValue[0].Trim().ToLower();
                            keyValue[1] = keyValue[1].Trim().ToLower();
                            var section = new SectionInfo(keyValue[1].Trim().ToLower(), fullName, i.ToString(), path);
                            if (readLock)
                            {
                                if (!UsedSectionsContains(keyValue[1]))
                                {
                                    UsedSection.Add(section);
                                    UnRegistSection.Add(section);
                                }
                            }
                            else if (KeyDist(keyValue[0]))
                            {
                                UsedSection.Add(section);
                            }
                            continue;
                        }
                    }
                    sr.Close();
                    fs.Close();
                });
                t.Start();
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 检查 键 是否存在
        /// </summary>
        private bool KeyDist(string key)
        {
            if (Key.Contains(key)) return true;
            foreach (var Key in Key) if (key.Contains(Key)) return true;
            return false;
        }
        /// <summary>
        /// 检查 Section 是否存在 于 UsedSections
        /// </summary>
        private bool UsedSectionsContains(string sectionInfo)
        {
            //foreach (var item in UsedSection)
            //    if (item.Section.ToLower() == sectionInfo.ToLower())
            //        return true;
            for (int i = 0; i < UsedSection.Count; i++)            
                if (UsedSection[i].Section.ToLower() == sectionInfo.ToLower())
                    return true;
            
            return false;
        }
        private bool UsedSectionsContains(SectionInfo SectionInfo)   
        {
            var sectionInfo = SectionInfo.Section.ToLower().Replace("[", string.Empty).Replace("]", string.Empty);
            if (White.Contains(sectionInfo.ToLower())) return true;
            else if (Regist.Contains(sectionInfo.ToLower())) return true;
            var tmp = new List<SectionInfo>(UsedSection);
            foreach (var item in UsedSection)
                if (item.Section.ToLower() == sectionInfo.ToLower())
                {
                    //tmp.Remove(item);
                    //UsedSection = tmp;
                    return true;
                }
            return false;
        }
        #endregion

        /// <summary>
        /// 运行分析
        /// </summary>
        /// <returns>返回的异常信息</returns>
        public List<Except> Anazlyze()
        {
            var tmpSection = new List<SectionInfo>();// 用来记录重复的临时列表
            foreach (var Section in SectionName)
            {
                if (tmpSection.Contains(Section)) 
                {
                    ReUsedSection.Add(Section); 
                }
                else if (!UsedSectionsContains(Section))
                {
                    UnUsedSection.Add(Section);
                }
                UsedSectionsAutoRemove(Section.Section.ToLower().Replace("[", string.Empty).Replace("]", string.Empty));
                tmpSection.Add(Section);
            }
            return GetExcepts();
        }
        private List<Except> GetExcepts()
        {
            var returnExcept = new List<Except>();
            foreach (var item in ReUsedSection) returnExcept.Add(new Except("重复Section名", item));
            foreach (var item in UnUsedSection) returnExcept.Add(new Except("未注册使用Section", item));
            foreach (var item in UnRegistSection) returnExcept.Add(new Except("注册未使用Section", item));
            if (returnExcept.Count == 0) return null;
            else return returnExcept;
        }
        /// <summary>
        /// 将 Section 从 UnRegistSection 中移除
        /// </summary>
        private void UsedSectionsAutoRemove(string sectionInfo)
        {
            if (White.Contains(sectionInfo.ToLower())) return ;
            else if (Regist.Contains(sectionInfo.ToLower())) return ;
            var tmp = new List<SectionInfo>(UnRegistSection);
            foreach (var item in UnRegistSection) if (item.Section.ToLower() == sectionInfo.ToLower()) tmp.Remove(item);
            UnRegistSection = tmp;
        }

        public struct SectionInfo
        {
            public string Section { get; private set; }
            public string FileName { get; private set; }
            public string LineNumber { get; private set; }
            public string FilePath { get; private set; }
            public SectionInfo(string Section, string FileName, string LineNo, string path)
            {
                this.Section =  Section ;
                this.FileName = FileName;
                LineNumber = LineNo;
                FilePath = path;
            }
        }
        public struct Except
        {
            public string Exception { get; private set; }
            private SectionInfo SectionInfo;
            public string Section { get => SectionInfo.Section; }
            public string FileName { get => SectionInfo.FileName; }
            public string LineNumber { get => SectionInfo.LineNumber;  }
            public string FilePath { get => SectionInfo.FilePath; }
            public Except(string avg1, SectionInfo avg2)
            {
                SectionInfo = avg2;
                string tmp = $"[{avg2.Section}]\t";
                if (avg2.Section.Length < 14) tmp = $"[{avg2.Section}]\t\t";
                if (avg2.Section.Length < 6) tmp = $"[{avg2.Section}]\t\t\t";
                Exception = $"异常:<{avg1}>\t{tmp}在{avg2.FilePath}:{avg2.LineNumber}";
            }
        }
    }
}
