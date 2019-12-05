using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    /// <summary>
    /// 插件信息
    /// </summary>
    public class PluginInfo
    {
        /// <summary>
        /// 插件的名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 插件的简介
        /// </summary>
        public string Summary { get; private set; }
        /// <summary>
        /// 插件版本
        /// </summary>
        public string Version { get; private set; }
        /// <summary>
        /// 插件作者信息
        /// </summary>
        public string Inventors { get; private set; }
        /// <summary>
        /// 插件版权信息
        /// </summary>
        public string Copyright { get; private set; }
        /// <summary>
        /// 插件所使用的类库
        /// </summary>
        public string[] Librarys { get; private set; }
    }
}
