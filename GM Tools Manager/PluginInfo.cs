using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GMTools
{
    public class PluginInfo
    {
        public string Name { get; private set; }
        public string Version { get; private set; }
        public string[] Librarys { get; private set; }
        public string Summary { get; private set; }
        public string Inventors { get; private set; }
        public string Copyright { get; private set; }
        public Type Type { get; private set; }

        public static PluginInfo GetPlugIn(string DllPath)
        {
            var pluginInfo = new PluginInfo();
            Assembly assembly = Assembly.LoadFrom(DllPath);// 获取DLL
            try
            {
                pluginInfo.Type = assembly.GetType("Plugin.Main", true);// 获取DLL命名空间中的类
                object obj = Activator.CreateInstance(pluginInfo.Type);// 实例化这个类
                MethodInfo GetName = pluginInfo.Type.GetMethod("Name");// 获取类的方法
                MethodInfo GetVersion = pluginInfo.Type.GetMethod("Version");// 获取类的方法
                MethodInfo GetLibrarys = pluginInfo.Type.GetMethod("Librarys");// 获取类的方法
                MethodInfo GetSummary = pluginInfo.Type.GetMethod("Summary");// 获取类的方法
                MethodInfo GetInventors = pluginInfo.Type.GetMethod("Inventors");// 获取类的方法
                MethodInfo GetCopyright = pluginInfo.Type.GetMethod("Copyright");// 获取类的方法
                try
                {
                    pluginInfo.Name = (string)GetName.Invoke(obj, new object[] { });
                    pluginInfo.Version = (string)GetVersion.Invoke(obj, new object[] { });
                    pluginInfo.Summary = (string)GetSummary.Invoke(obj, new object[] { });
                    pluginInfo.Inventors = (string)GetInventors.Invoke(obj, new object[] { });
                    pluginInfo.Copyright = (string)GetCopyright.Invoke(obj, new object[] { });
                    pluginInfo.Librarys = (string[])GetLibrarys.Invoke(obj, new object[] { });
                }
                catch (NullReferenceException)
                {
                    System.Diagnostics.Debug.WriteLine(DllPath + " 未找到Info信息");
                }
            }
            catch (TypeLoadException)
            {
                return null;
            }
            return pluginInfo;
        }
    }
}
