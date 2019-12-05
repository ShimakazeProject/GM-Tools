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
        /// 向Manager传递插件信息
        /// </summary>
        /// <returns>插件信息</returns>
        PluginInfo Info();

        /// <summary>
        /// 插件的启动方法
        /// </summary>
        void Start();


    }
}
