﻿using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Plugin.Windows.CSWinbtn
{
    public class MinMax: Button
    {
        public Geometry PathData
        {
            get { return (Geometry)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PathData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathDataProperty =
            DependencyProperty.Register("PathData", typeof(Geometry), typeof(MinMax), new PropertyMetadata(Geometry.Parse("M 0,10 H 20 V 15 H 0")));


        public MinMax()
        {
            var ct = new ControlTemplate(typeof(Button));
            {
                var _border = new FrameworkElementFactory(typeof(Border));
                _border.SetValue(Border.NameProperty, "Bd");
                _border.SetValue(Border.MarginProperty, new Thickness(1));
                _border.SetValue(Border.CornerRadiusProperty, new CornerRadius(0));
                _border.SetValue(Border.BackgroundProperty, new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)));
                _border.SetValue(Border.SnapsToDevicePixelsProperty, true);
                _border.SetBinding(Border.BackgroundProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("Background"),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                {
                    var text = new FrameworkElementFactory(typeof(Path));
                    text.SetValue(Path.NameProperty, "text");
                    text.SetValue(Path.FillProperty, new SolidColorBrush(Color.FromRgb(0x97, 0x97, 0x97)));
                    text.SetValue(Path.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                    text.SetValue(Path.VerticalAlignmentProperty, VerticalAlignment.Center);
                    text.SetBinding(Path.DataProperty, new Binding()
                    {
                        Source = this,
                        Path = new PropertyPath("PathData"),
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
                    _border.AppendChild(text);
                }
                var ctt1 = new Trigger
                {
                    Property = IsMouseOverProperty,
                    Value = true
                };
                {
                    ctt1.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(Color.FromRgb(0x97, 0x97, 0x97))));
                    ctt1.Setters.Add(new Setter(Path.FillProperty, Brushes.White));
                }
                var ctt2 = new Trigger
                {
                    Property = IsPressedProperty,
                    Value = true
                };
                {
                    ctt2.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(Color.FromRgb(0x97, 0x97, 0x97))));
                    ctt2.Setters.Add(new Setter(Path.FillProperty, Brushes.White));
                }

                ct.VisualTree = _border;
                ct.Triggers.Add(ctt1);
                ct.Triggers.Add(ctt2);
            }
            Template = ct;
            Background = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A));
            Foreground = new SolidColorBrush(Color.FromRgb(0x97, 0x97, 0x97));
        }
    }

    public class Close : Button
    {
        public Close()
        {
            var ct = new ControlTemplate(typeof(Button));
            {
                var _border = new FrameworkElementFactory(typeof(Border));
                _border.SetValue(Border.NameProperty, "Bd");
                _border.SetValue(Border.MarginProperty, new Thickness(1));
                _border.SetValue(Border.CornerRadiusProperty, new CornerRadius(0));
                _border.SetValue(Border.BackgroundProperty, new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)));
                _border.SetValue(Border.SnapsToDevicePixelsProperty, true);
                _border.SetBinding(Border.BackgroundProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("Background"),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                {
                    var text = new FrameworkElementFactory(typeof(Path));
                    text.SetValue(Path.NameProperty, "text");
                    text.SetValue(Path.StrokeProperty, new SolidColorBrush(Color.FromRgb(0x97, 0x97, 0x97)));
                    text.SetValue(Path.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                    text.SetValue(Path.VerticalAlignmentProperty, VerticalAlignment.Center);
                    var gg = new GeometryGroup();
                    gg.Children.Add(new LineGeometry(new Point(0, 0), new Point(20, 15)));
                    gg.Children.Add(new LineGeometry(new Point(0, 15), new Point(20, 0)));
                    text.SetValue(Path.DataProperty, gg);
                    _border.AppendChild(text);
                }
                var ctt1 = new Trigger
                {
                    Property = IsMouseOverProperty,
                    Value = true
                };
                {
                    ctt1.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(Color.FromRgb(0x97, 0x97, 0x97))));
                    ctt1.Setters.Add(new Setter(Path.StrokeProperty, Brushes.White));
                }
                var ctt2 = new Trigger
                {
                    Property = IsPressedProperty,
                    Value = true
                };
                {
                    ctt2.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(Color.FromRgb(0x97, 0x97, 0x97))));
                    ctt2.Setters.Add(new Setter(Path.StrokeProperty, Brushes.White));
                }

                ct.VisualTree = _border;
                ct.Triggers.Add(ctt1);
                ct.Triggers.Add(ctt2);
            }
            Template = ct;
            Background = new SolidColorBrush(Color.FromRgb(0xC9, 0, 0));
            Foreground = new SolidColorBrush(Color.FromRgb(0x91, 0, 0));
        }
    }

}
