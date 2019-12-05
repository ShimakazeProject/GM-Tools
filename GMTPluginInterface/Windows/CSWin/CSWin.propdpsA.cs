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
        /// <summary>
        /// 窗口边框颜色
        /// </summary>
        public Brush CSBorderBrush
        {
            get { return (Brush)GetValue(CSBorderBrushProperty); }
            set { SetValue(CSBorderBrushProperty, value); }
        }
        /// <summary>
        /// 窗口边框粗细..(大概
        /// </summary>
        public Thickness CSBorderThickness
        {
            get { return (Thickness)GetValue(CSBorderThicknessProperty); }
            set { SetValue(CSBorderThicknessProperty, value); }
        }
        /// <summary>
        /// 窗口边框圆角
        /// </summary>
        public CornerRadius CSCornerRadius
        {
            get { return (CornerRadius)GetValue(CSCornerRadiusProperty); }
            set { SetValue(CSCornerRadiusProperty, value); }
        }
        /// <summary>
        /// 窗口工作区大小
        /// </summary>
        public Thickness CSWorkareaMargin
        {
            get { return (Thickness)GetValue(CSWorkareaMarginProperty); }
            set { SetValue(CSWorkareaMarginProperty, value); }
        }
        /// <summary>
        /// 标题栏高度
        /// </summary>
        public GridLength TitleHeight
        {
            get { return (GridLength)GetValue(TitleHeightProperty); }
            set { SetValue(TitleHeightProperty, value); }
        }
        /// <summary>
        /// 标题栏背景颜色
        /// </summary>
        public Brush TitleBackground
        {
            get { return (Brush)GetValue(TitleBackgroundProperty); }
            set { SetValue(TitleBackgroundProperty, value); }
        }
        /// <summary>
        /// 标题文字颜色
        /// </summary>
        public Brush TitleForeground
        {
            get { return (Brush)GetValue(TitleForegroundProperty); }
            set { SetValue(TitleForegroundProperty, value); }
        }
        /// <summary>
        /// 标题字体大小
        /// </summary>
        public double TitleFontSize
        {
            get { return (double)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }
        /// <summary>
        /// 标题位置
        /// </summary>
        public HorizontalAlignment TitleHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(TitleHorizontalAlignmentProperty); }
            set { SetValue(TitleHorizontalAlignmentProperty, value); }
        }
        /// <summary>
        /// 最大化按钮隐藏
        /// </summary>
        public Visibility TitleMaxBtnVisibility
        {
            get { return (Visibility)GetValue(TitleMaxBtnVisibilityProperty); }
            set { SetValue(TitleMaxBtnVisibilityProperty, value); }
        }
        /// <summary>
        /// 最小化按钮隐藏
        /// </summary>
        public Visibility TitleMinBtnVisibility
        {
            get { return (Visibility)GetValue(TitleMinBtnVisibilityProperty); }
            set { SetValue(TitleMinBtnVisibilityProperty, value); }
        }
        /// <summary>
        /// 关闭按钮隐藏
        /// </summary>
        public Visibility TitleCloseBtnVisibility
        {
            get { return (Visibility)GetValue(TitleCloseBtnVisibilityProperty); }
            set { SetValue(TitleCloseBtnVisibilityProperty, value); }
        }
        /// <summary>
        /// 按钮宽度
        /// </summary>
        public double TitleBtnWidth
        {
            get { return (double)GetValue(TitleBtnWidthProperty); }
            set { SetValue(TitleBtnWidthProperty, value); }
        }
        /// <summary>
        /// 工作区高度
        /// </summary>
        public GridLength WorkareaHeight
        {
            get { return (GridLength)GetValue(WorkareaHeightProperty); }
            set { SetValue(WorkareaHeightProperty, value); }
        }
    }
}
