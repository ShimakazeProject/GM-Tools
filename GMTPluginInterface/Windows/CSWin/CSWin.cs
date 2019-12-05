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

namespace Plugin.Windows
{
    public partial class CSWin:Window
    {        
        /// <summary>
        /// 构造方法
        /// </summary>
        public CSWin()
        {
            var ctemp = new ControlTemplate(typeof(Window));
            {
                var _border = new FrameworkElementFactory(typeof(Border));
                _border.SetValue(Border.BackgroundProperty, Brushes.Transparent);
                _border.SetValue(Border.SnapsToDevicePixelsProperty, true);
                _border.SetBinding(Border.BorderBrushProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("CSBorderBrush"),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                _border.SetBinding(Border.BorderThicknessProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("CSBorderThickness"),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                _border.SetBinding(Border.CornerRadiusProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("CSCornerRadius"),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                {
                    var _workarea = new FrameworkElementFactory(typeof(Grid));
                    _workarea.SetBinding(Grid.MarginProperty, new Binding()
                    {
                        Source = this,
                        Path = new PropertyPath("CSWorkareaMargin"),
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
                    _workarea.SetBinding(Grid.BackgroundProperty, new Binding()
                    {
                        Source = this,
                        Path = new PropertyPath("Background"),
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
                    {
                        var __waRd1 = new FrameworkElementFactory(typeof(RowDefinition));
                        var __waRd2 = new FrameworkElementFactory(typeof(RowDefinition));
                        __waRd1.SetBinding(RowDefinition.HeightProperty, new Binding()
                        {
                            Source = this,
                            Path = new PropertyPath("TitleHeight"),
                            Mode = BindingMode.TwoWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        });
                        __waRd2.SetBinding(RowDefinition.HeightProperty, new Binding()
                        {
                            Source = this,
                            Path = new PropertyPath("WorkareaHeight"),
                            Mode = BindingMode.TwoWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        });
                        _workarea.AppendChild(__waRd1);
                        _workarea.AppendChild(__waRd2);
                    }
                    {
                        var _grid = new FrameworkElementFactory(typeof(Grid));
                        _grid.SetValue(Grid.RowProperty, 0);
                        _grid.AddHandler(Grid.MouseMoveEvent, new MouseEventHandler(TitleBar_MouseMove));
                        _grid.SetBinding(Grid.BackgroundProperty, new Binding()
                        {
                            Source = this,
                            Path = new PropertyPath("TitleBackground"),
                            Mode = BindingMode.TwoWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        });
                        {
                            var _title = new FrameworkElementFactory(typeof(TextBlock));
                            _title.SetBinding(TextBlock.FontSizeProperty, new Binding()
                            {
                                Source = this,
                                Path = new PropertyPath("TitleFontSize"),
                                Mode = BindingMode.TwoWay,
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            });
                            _title.SetBinding(TextBlock.HorizontalAlignmentProperty, new Binding()
                            {
                                Source = this,
                                Path = new PropertyPath("TitleHorizontalAlignment"),
                                Mode = BindingMode.TwoWay,
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            });
                            _title.SetBinding(TextBlock.TextProperty, new Binding()
                            {
                                Source = this,
                                Path = new PropertyPath("Title"),
                                Mode = BindingMode.TwoWay,
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            });

                            var _dp = new FrameworkElementFactory(typeof(DockPanel));
                            _dp.SetValue(DockPanel.HorizontalAlignmentProperty, HorizontalAlignment.Right);
                            _dp.SetValue(DockPanel.DockProperty, Dock.Right);
                            {
                                var btn_max = new FrameworkElementFactory(typeof(CSWinbtn.MinMax));
                                btn_max.SetValue(CSWinbtn.MinMax.PathDataProperty, Geometry.Parse("M0 0 H20 V15 H0 V4 H2 V13 H18 V4 H0 V0"));
                                btn_max.SetBinding(Button.VisibilityProperty, new Binding()
                                {
                                    Source = this,
                                    Path = new PropertyPath("TitleMaxBtnVisibility"),
                                    Mode = BindingMode.TwoWay,
                                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                });
                                btn_max.SetBinding(Button.WidthProperty, new Binding()
                                {
                                    Source = this,
                                    Path = new PropertyPath("TitleBtnWidth"),
                                    Mode = BindingMode.TwoWay,
                                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                });
                                btn_max.AddHandler(Button.ClickEvent, handler: new RoutedEventHandler(btn_max_Click));

                                var btn_min = new FrameworkElementFactory(typeof(CSWinbtn.MinMax));
                                btn_min.SetBinding(Button.VisibilityProperty, new Binding()
                                {
                                    Source = this,
                                    Path = new PropertyPath("TitleMinBtnVisibility"),
                                    Mode = BindingMode.TwoWay,
                                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                });
                                btn_min.SetBinding(Button.WidthProperty, new Binding()
                                {
                                    Source = this,
                                    Path = new PropertyPath("TitleBtnWidth"),
                                    Mode = BindingMode.TwoWay,
                                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                });
                                btn_min.AddHandler(Button.ClickEvent, handler: new RoutedEventHandler(btn_min_Click));

                                var btn_close = new FrameworkElementFactory(typeof(CSWinbtn.Close));
                                btn_close.SetBinding(Button.VisibilityProperty, new Binding()
                                {
                                    Source = this,
                                    Path = new PropertyPath("TitleCloseBtnVisibility"),
                                    Mode = BindingMode.TwoWay,
                                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                });
                                btn_close.SetBinding(Button.WidthProperty, new Binding()
                                {
                                    Source = this,
                                    Path = new PropertyPath("TitleBtnWidth"),
                                    Mode = BindingMode.TwoWay,
                                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                });
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
            Loaded += _window_Loaded;
        }
    }
}
