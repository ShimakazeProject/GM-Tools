using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Crape_Studio_Manager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindow_Initialized();
        }

        private void MainWindow_Initialized()
        {
            DirectoryInfo di = new DirectoryInfo(@".\Plugin\");
            var fis = di.GetFiles();
            foreach (var fi in fis)
            {
                if (fi.Extension.ToLower() == ".dll")
                {
                    Assembly ass = Assembly.LoadFrom(fi.FullName);// 获取DLL
                    string info = ass.FullName;
                    string[] infos = info.Split(',');
                    Type type = ass.GetType("Plugin.Main");// 获取DLL命名空间中的类
                    object obj = Activator.CreateInstance(type);// 实例化这个类
                    MethodInfo Info = type.GetMethod("Info");// 获取类的方法
                    string pluginInfo = "";
                    try
                    {
                        pluginInfo = (string)Info.Invoke(obj, new object[] { });
                    }
                    catch (NullReferenceException)
                    {
                        pluginInfo = "暂无简介";
                        System.Diagnostics.Debug.WriteLine(fi.FullName + " 未找到Info信息");
                    }

                    var plugin = new PluginInfo(infos[0].Trim(), infos[1].Split('=')[1].Trim(), type, pluginInfo);
                    Button btn = new Button()
                    {
                        Content = "  " + plugin.Name + "  ",
                        DataContext = plugin,
                        ToolTip = plugin.Name + " " + plugin.Ver + "\n" + pluginInfo,
                        Height = 32,
                        FontSize = 24,
                        Margin = new Thickness(10)
                    };
                    btn.Click += Btn_Click;
                    _wp.Children.Add(btn);
                }
            }
            /*
            di = new DirectoryInfo(@".\PluginC\");
            fis = di.GetFiles();
            foreach (var fi in fis)
            {
                //1. 动态加载C++ Dll
                int hModule = NativeMethod.LoadLibrary(fi.FullName);
                if (hModule == 0) continue;
                //2. 读取函数指针
                IntPtr intPtr = NativeMethod.GetProcAddress(hModule, "Start");
                //3. 将函数指针封装成委托
                Start addFunction = (Start)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(Start));

                var plugin = new PluginInfo(fi.Name.Split('.')[0], "Unknow", type, "Unknow");
                Button btn = new Button()
                {
                    Content = "  " + plugin.Name + "  ",
                    DataContext = plugin,
                    ToolTip = plugin.Name + " " + plugin.Ver + "\n" + "这是一个C++的DLL库",
                    Height = 32,
                    FontSize = 24,
                    Margin = new Thickness(10)
                };
                btn.Click += Btn_Click;
                _wp.Children.Add(btn);
            }
            */
        }
        /// <summary>
        /// 函数指针
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate int Start();
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            PluginInfo plugin = (PluginInfo)button.DataContext;
            object obj = Activator.CreateInstance(plugin.Type);// 实例化这个类
            MethodInfo Main = plugin.Type.GetMethod("Start");// 获取类的方法
            Main.Invoke(obj, new object[] { });
        }
    }
}
