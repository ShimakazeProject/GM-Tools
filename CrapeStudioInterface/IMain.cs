using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    /// <summary>
    /// 插件Main类的接口
    /// </summary>
    public interface IMain
    {
        /// <summary>
        /// 插件的名称
        /// </summary>
        /// <returns></returns>
        string Name();
        /// <summary>
        /// 插件版本
        /// </summary>
        /// <returns></returns>
        string Version();
        /// <summary>
        /// 插件的简介
        /// </summary>
        /// <returns></returns>
        string Summary();
        /// <summary>
        /// 插件作者信息
        /// </summary>
        /// <returns></returns>
        string Inventors();
        /// <summary>
        /// 插件版权信息
        /// </summary>
        /// <returns></returns>
        string Copyright();
        /// <summary>
        /// 插件所使用的类库
        /// </summary>
        /// <returns></returns>
        string[] Librarys();
        /// <summary>
        /// 插件的启动方法
        /// </summary>
        void Start();
    }
}
