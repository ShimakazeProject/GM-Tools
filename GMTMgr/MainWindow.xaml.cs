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


namespace GMTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Plugin.Controls.CSWin
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindow_Initialized();

        }

        private void MainWindow_Initialized()
        {
            if (!Directory.Exists(@".\Plugin\")) Directory.CreateDirectory(@".\Plugin\");
            if (!Directory.Exists(@".\Librarys\")) Directory.CreateDirectory(@".\Librarys\");
            DirectoryInfo di = new DirectoryInfo(@".\Plugin\");
            DirectoryInfo libDir = new DirectoryInfo(@".\Librarys\");
            var fis = di.GetFiles();
            List<string> libInfo = new List<string>();
            foreach (var libn in libDir.GetFiles())
            {
                if (libn.Extension.ToLower() != ".dll") continue;
                Assembly ass = Assembly.LoadFrom(libn.FullName);
                libInfo.Add(ass.FullName);
            }
            foreach (var fi in fis)
            {
                if (fi.Extension.ToLower() == ".dll")
                {
                    var pluginInfo = PluginInfo.GetPlugIn(fi.FullName);
                    if (pluginInfo == null) continue;
                    string name = pluginInfo.Name;
                    string ver = pluginInfo.Version;
                    string summary = pluginInfo.Summary;
                    string inventor = pluginInfo.Inventors;
                    string copyright = pluginInfo.Copyright;
                    string[] libs = pluginInfo.Librarys;
                    List<string> noHaveLib = new List<string>();
                    var btn = new Plugin.Controls.CSButton()
                    {
                        DataContext = pluginInfo,
                        Height = 32,
                        FontSize = 16,
                        Margin = new Thickness(10)
                    };
                    if (name == null)
                    {
                        btn.Content = "  暂无名称  ";
                    }
                    else
                    {
                        btn.Content = "  " + name + "  ";
                    }
                    if (libs != null)
                    {
                        foreach (var lib in libs)
                        {
                            if (!libInfo.Contains(lib))
                            {
                                btn.IsEnabled = false;
                                noHaveLib.Add(lib);
                            }
                        }
                    }
                    if (noHaveLib.Count != 0)
                    {
                        string lib = string.Empty;
                        foreach (var item in noHaveLib)
                        {
                            lib += item + "\n";
                        }
                        btn.ToolTip = $"{name} {ver}\n简介:\n{summary}\n缺少运行库{lib}\n\n{inventor}\n{copyright}";
                    }
                    else btn.ToolTip = $"{name} {ver}\n简介:\n{summary}\n\n{inventor}\n{copyright}";
                    btn.Click += Btn_Click;
                    _wp.Children.Add(btn);
                }
            }
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            try
            {
                PluginInfo plugin = (PluginInfo)button.DataContext;
                object obj = Activator.CreateInstance(plugin.Type);// 实例化这个类
                MethodInfo Main = plugin.Type.GetMethod("Start");// 获取类的方法
                Main.Invoke(obj, new object[] { });
            }catch(Exception ex)
            {
                App.Logger.WriteLog(Plugin.CSLogger.LogRank.ERROR, $"{button.Content} Throw an Exception");
                App.Logger.ExceptLog(ex);
            }
        }
    }
}
