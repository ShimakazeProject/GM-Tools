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
using System.Windows.Shapes;

namespace Plugin.Controls
{
    public partial class CSButton:Button
    {
        public CSButton()
        {
            var ct = new ControlTemplate(typeof(Button));
            var _bor = new FrameworkElementFactory(typeof(Border));
            _bor.SetValue(Border.BorderBrushProperty, new SolidColorBrush(Color.FromRgb(0x8D, 0x8D, 0x8D)));
            _bor.SetValue(Border.BorderThicknessProperty,new Thickness(1));
            {
                var _border = new FrameworkElementFactory(typeof(Border));
                _border.Name = "Bd";
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
                var text = new FrameworkElementFactory(typeof(TextBlock));
                text.Name = "text";
                text.SetValue(TextBlock.MarginProperty, new Thickness(10, 5, 10, 5));
                text.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                text.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                text.SetBinding(TextBlock.TextProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("Content"),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                _border.AppendChild(text);
                var ctt1 = new Trigger
                {
                    Property = IsMouseOverProperty,
                    Value = true
                };
                {
                    ctt1.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(Color.FromRgb(0x97, 0x97, 0x97)), "Bd"));
                    ctt1.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.White, "text"));
                }
                var ctt2 = new Trigger
                {
                    Property = IsPressedProperty,
                    Value = true
                };
                {
                    ctt2.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(Color.FromRgb(0x97, 0x97, 0x97)), "Bd"));
                    ctt2.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.White, "text"));
                }

                _bor.AppendChild(_border);
                ct.VisualTree = _bor;
                ct.Triggers.Add(ctt1);
                ct.Triggers.Add(ctt2);
            }
            Template = ct;
            Background = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A));
            Foreground = new SolidColorBrush(Color.FromRgb(0x97, 0x97, 0x97));
        }
    }
}
