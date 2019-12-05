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
    public partial class CSWin : Window
    {
        // Using a DependencyProperty as the backing store for \w+.  This enables animation, styling, binding, etc...

        public static readonly DependencyProperty CSBorderBrushProperty = DependencyProperty.Register("CSBorderBrush", typeof(Brush), typeof(CSWin), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0x78, 0xD7))));
        public static readonly DependencyProperty CSBorderThicknessProperty = DependencyProperty.Register("CSBorderThickness", typeof(Thickness), typeof(CSWin), new PropertyMetadata(new Thickness(1)));
        public static readonly DependencyProperty CSCornerRadiusProperty = DependencyProperty.Register("CSCornerRadius", typeof(CornerRadius), typeof(CSWin), new PropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty CSWorkareaMarginProperty = DependencyProperty.Register("CSWorkareaMargin", typeof(Thickness), typeof(CSWin), new PropertyMetadata(new Thickness(1)));
        public static readonly DependencyProperty TitleHeightProperty = DependencyProperty.Register("TitleHeight", typeof(GridLength), typeof(CSWin), new PropertyMetadata(new GridLength(24)));
        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(CSWin), new PropertyMetadata(Brushes.White));
        public static readonly DependencyProperty TitleBackgroundProperty = DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(CSWin), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0x50, 0, 0, 0))));
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register("TitleFontSize", typeof(double), typeof(CSWin), new PropertyMetadata(18d));
        public static readonly DependencyProperty TitleHorizontalAlignmentProperty = DependencyProperty.Register("TitleHorizontalAlignment", typeof(HorizontalAlignment), typeof(CSWin), new PropertyMetadata(HorizontalAlignment.Center));
        public static readonly DependencyProperty TitleMaxBtnVisibilityProperty = DependencyProperty.Register("TitleMaxBtnVisibility", typeof(Visibility), typeof(CSWin), new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty TitleMinBtnVisibilityProperty = DependencyProperty.Register("TitleMinBtnVisibility", typeof(Visibility), typeof(CSWin), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty TitleCloseBtnVisibilityProperty = DependencyProperty.Register("TitleCloseBtnVisibility", typeof(Visibility), typeof(CSWin), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty TitleBtnWidthProperty = DependencyProperty.Register("TitleBtnWidth", typeof(double), typeof(CSWin), new PropertyMetadata(32d));
        public static readonly DependencyProperty WorkareaHeightProperty = DependencyProperty.Register("WorkareaHeight", typeof(GridLength), typeof(CSWin), new PropertyMetadata(new GridLength(1, GridUnitType.Star)));
    }
}
