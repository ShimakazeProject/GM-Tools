using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace CrapeStyle
{
    public class EBWin:Window
    {
        public EBWin()
        {
            MouseDown += _window_MouseDown;
            Loaded += _window_Loaded;
            var ctemp = new ControlTemplate(typeof(Window));
            {
                var _border = new FrameworkElementFactory(typeof(Border));
                _border.SetValue(Border.BorderBrushProperty, String2Brush("#FF0078D7"));
                _border.SetValue(Border.BackgroundProperty, Brushes.Transparent);
                _border.SetValue(Border.BorderThicknessProperty, new Thickness(1));
                _border.SetValue(Border.CornerRadiusProperty, new CornerRadius(0));
                _border.SetValue(Border.SnapsToDevicePixelsProperty, true);
                {
                    var _workarea = new FrameworkElementFactory(typeof(Grid));
                    _workarea.SetValue(Grid.BackgroundProperty, Brushes.Transparent);
                    _workarea.SetValue(Grid.MarginProperty, new Thickness(3));
                    {
                        var __waRd1 = new FrameworkElementFactory(typeof(RowDefinition));
                        __waRd1.SetValue(RowDefinition.HeightProperty, new GridLength(24));


                        var __waRd2 = new FrameworkElementFactory(typeof(RowDefinition));
                        __waRd2.SetValue(RowDefinition.HeightProperty, new GridLength(1, GridUnitType.Star));

                        _workarea.AppendChild(__waRd1);
                        _workarea.AppendChild(__waRd2);
                    }
                    {
                        var _grid = new FrameworkElementFactory(typeof(Grid));
                        _grid.AddHandler(Grid.MouseMoveEvent, new MouseEventHandler(TitleBar_MouseMove));
                        _grid.SetValue(Grid.BackgroundProperty, String2Brush("50000000"));
                        _grid.SetValue(Grid.RowProperty, 0);
                        {
                            var _title = new FrameworkElementFactory(typeof(TextBlock));
                            _title.SetValue(TextBlock.FontSizeProperty, (double)18);
                            _title.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);

                            var binding = new Binding()
                            {
                                Source = this,
                                Path = new PropertyPath("Title"),
                                Mode = BindingMode.TwoWay,
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            };
                            //_title.SetValue(TextBlock.TextProperty, binding);
                            _title.SetBinding(TextBlock.TextProperty, binding);

                            var _dp = new FrameworkElementFactory(typeof(DockPanel));
                            _dp.SetValue(DockPanel.HorizontalAlignmentProperty, HorizontalAlignment.Right);
                            _dp.SetValue(DockPanel.DockProperty, Dock.Right);
                            {
                                var btn_max = new FrameworkElementFactory(typeof(Button));
                                btn_max.SetValue(Button.ContentProperty, "▢");
                                btn_max.SetValue(Button.VisibilityProperty, Visibility.Collapsed);
                                btn_max.SetValue(Button.WidthProperty, (double)32);
                                btn_max.AddHandler(Button.ClickEvent, handler: new RoutedEventHandler(btn_max_Click));

                                var btn_min = new FrameworkElementFactory(typeof(Button));
                                btn_min.SetValue(Button.ContentProperty, "_");
                                btn_min.SetValue(Button.WidthProperty, (double)32);
                                btn_min.AddHandler(Button.ClickEvent, handler: new RoutedEventHandler(btn_min_Click));

                                var btn_close = new FrameworkElementFactory(typeof(Button));
                                btn_close.SetValue(Button.ContentProperty, "×");
                                btn_close.SetValue(Button.WidthProperty, (double)32);
                                btn_close.AddHandler(Button.ClickEvent, handler: new RoutedEventHandler(btn_close_Click));

                                _dp.AppendChild(btn_min);
                                _dp.AppendChild(btn_max);
                                _dp.AppendChild(btn_close);
                            }
                            var content = new FrameworkElementFactory(typeof(ContentPresenter));
                            content.SetValue(Grid.RowProperty, 1);

                            _grid.AppendChild(_title);
                            _grid.AppendChild(_dp);

                            _workarea.AppendChild(content);
                        }
                        _workarea.AppendChild(_grid);
                    }
                    _border.AppendChild(_workarea);
                }
                ctemp.VisualTree = _border;
            }
            this.Template = ctemp;
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
        }
        #region 窗口事件方法
        /// <summary>
        /// 窗口移动事件
        /// </summary>
        protected void TitleBar_MouseMove(object sender,MouseEventArgs e)
        {
            if (e.LeftButton==MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        /// <summary>
        /// 窗口最大化 还原
        /// </summary>
        protected virtual void btn_max_Click(object sender,RoutedEventArgs e)
        {
            if (this.WindowState==WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }
        /// <summary>
        /// 窗口最小化
        /// </summary>
        protected virtual void btn_min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;            
        }
        /// <summary>
        /// 窗口关闭
        /// </summary>
        protected virtual void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        int i = 0;
        /// <summary>
        /// 双击
        /// </summary>
        protected virtual void dpTitle_MouseDown(object sender,MouseButtonEventArgs e)
        {
            if (e.RightButton==MouseButtonState.Pressed)
            {
                return;                
            }
            i += 1;
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += (s, e1) =>
            {
                timer.IsEnabled = false;
                i = 0;
            };
            timer.IsEnabled = true;
            if (i%2!=0)
            {
                return;
            }
            timer.IsEnabled = false;
            i = 0;
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
        #endregion


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

        /// <summary>
        /// 将Html颜色字符串转换为Brush
        /// </summary>
        /// <param name="color">HTML颜色</param>
        /// <returns>Brush</returns>
        public static Brush String2Brush(string color)
        {
            System.Drawing.Color clr = System.Drawing.ColorTranslator.FromHtml(color);
            return new SolidColorBrush(Color.FromArgb(clr.A, clr.R, clr.G, clr.B));
        }
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
