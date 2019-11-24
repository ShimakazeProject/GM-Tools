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


            Loaded += _window_Loaded;
            MouseDown += _window_MouseDown;
        }

        private void MainWindow_Initialized()
        {
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
                    Button btn = new Button()
                    {
                        DataContext = pluginInfo,
                        Height = 32,
                        FontSize = 24,
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


            //Assembly ass = Assembly.LoadFrom(fi.FullName);// 获取DLL
            //string info = ass.FullName;
            //string[] infos = info.Split(',');
            //Type type = ass.GetType("Plugin.Main");// 获取DLL命名空间中的类
            //object obj = Activator.CreateInstance(type);// 实例化这个类
            //MethodInfo Info = type.GetMethod("Info");// 获取类的方法
            //string pluginInfo = "";
            //try
            //{
            //    pluginInfo = (string)Info.Invoke(obj, new object[] { });
            //}
            //catch (NullReferenceException)
            //{
            //    pluginInfo = "暂无简介";
            //    System.Diagnostics.Debug.WriteLine(fi.FullName + " 未找到Info信息");
            //}

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

        private void _close_Click(object sender, RoutedEventArgs e) => Close();
        private void _minimized_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void _maximized_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Maximized; 
        private void _title_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
        #region 窗口毛玻璃
        private void _window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
        }
        internal void EnableBlur()
        {
            var windowHelper = new System.Windows.Interop.WindowInteropHelper(this);

            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
        private void _window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        #endregion

    }
    #region 结构和类
    internal enum AccentState
    {
        ACCENT_DISABLED = 1,
        ACCENT_ENABLE_GRADIENT = 0,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_INVALID_STATE = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        // ...
        WCA_ACCENT_POLICY = 19
        // ...
    }
    #endregion
}
