using System;
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
using System.Windows.Shapes;


namespace IniMaker
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Plugin.Controls.CSWin
    {

        public MainWindow()
        {
            InitializeComponent();
            var item = new TreeViewItem();
            item.Selected += selected;
            item.Header = "Master";
            rulesmd.Items.Add(item);
            var item1 = new TreeViewItem();
            item1.Selected += selected;
            item1.Header = "Master";
            artmd.Items.Add(item1);
            item.IsSelected = true;
            // ====
            //edit.SyntaxHighlighting
            System.Xml.XmlReader xml;
            var file = AppDomain.CurrentDomain.BaseDirectory + @"\Config\iniSyntax";
            if (System.IO.File.Exists(file + ".xshd"))
                xml = System.Xml.XmlReader.Create(file + ".xshd");
            else if (System.IO.File.Exists(file + ".xml"))
                xml = System.Xml.XmlReader.Create(file + ".xml");
            else throw new Exception("未找到语法文件");
            edit.SyntaxHighlighting = HL.Manager.HighlightingLoader.Load(xml, new ICSharpCode.AvalonEdit.Highlighting.HighlightingManager());

        }
        private void setHighLight()
        {
            
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in rulesmd.Items)
            {

            }
        }
        private void selected(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as TreeViewItem;
            if (item.DataContext != null)
                edit.Text = item.DataContext as string;
            else
                edit.Text = string.Empty;
        }

        private void edit_TextChanged(object sender, EventArgs e)
        {
            foreach (TreeViewItem item in rulesmd.Items)
            {
                if (item.IsSelected)
                {
                    item.DataContext = edit.Text;
                    return;
                }
            }
            foreach (TreeViewItem item in artmd.Items)
            {
                if (item.IsSelected)
                {
                    item.DataContext = edit.Text;
                    return;
                }
            }
        }
        private void rulesAdd_Click(object sender, RoutedEventArgs e)
        {
            var rname = new NamedWindow();
            var item = new TreeViewItem();
            item.Selected += selected;
            string name = rname.ShowDialog();
            if (name == null) return;
            else
            {
                item.Header = name;
                rulesmd.Items.Add(item);
            }
        }
        private void rulesRNM_Click(object sender, RoutedEventArgs e)
        {
            var changeItem = new TreeViewItem();
            foreach (TreeViewItem item in rulesmd.Items)
            {
                if (item.IsSelected)
                {
                    if ((item.Header as string)!="Master")
                    {
                        changeItem = item;
                    }
                    else
                    {
                        MessageBox.Show("暂不支持修改Master分支名");
                        return;
                    }
                }
            }
            var rname = new NamedWindow();
            string name = rname.ShowDialog();
            if (name == null) return;
            else changeItem.Header = name;
        }
        private void rulesDrop_Click(object sender, RoutedEventArgs e)
        {
            foreach (TreeViewItem item in rulesmd.Items)
            {
                if (item.IsSelected)
                {
                    if ((item.Header as string) != "Master")
                    {
                        rulesmd.Items.Remove(item);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("不允许删除Master分支");
                        return;
                    }
                }
            }
        }
        private void artAdd_Click(object sender, RoutedEventArgs e)
        {
            var rname = new NamedWindow();
            var item = new TreeViewItem();
            item.Selected += selected;
            string name = rname.ShowDialog();
            if (name == null) return;
            else
            {
                item.Header = name;
                artmd.Items.Add(item);
            }
        }
        private void artDrop_Click(object sender, RoutedEventArgs e)
        {
            foreach (TreeViewItem item in artmd.Items)
            {
                if (item.IsSelected)
                {
                    if ((item.Header as string) != "Master")
                    {
                        artmd.Items.Remove(item);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("不允许删除Master分支");
                        return;
                    }
                }
            }
        }
        private void artRNM_Click(object sender, RoutedEventArgs e)
        {
            var changeItem = new TreeViewItem();
            foreach (TreeViewItem item in artmd.Items)
            {
                if (item.IsSelected)
                {
                    if ((item.Header as string) != "Master")
                    {
                        changeItem = item;
                    }
                    else
                    {
                        MessageBox.Show("暂不支持修改Master分支名");
                        return;
                    }
                }
            }
            var rname = new NamedWindow();
            string name = rname.ShowDialog();
            if (name == null) return;
            else changeItem.Header = name;
        }

    }
}
/*

        private static object _lock = new object();
        private int i = 0;
        private void edit_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisc = sender as RichTextBox;
            var doc = thisc.Document;
            //SetStyle(thisc);

        }
        private void RichtxtboxInput(string txt, RichTextBox richtxtbox)
        {
            Run r = new Run(txt);
            Paragraph para = new Paragraph();
            para.Inlines.Add(r);
            richtxtbox.Document.Blocks.Clear();
            richtxtbox.Document.Blocks.Add(para);
        }
        private void SetSectionStyle(RichTextBox thisc)
        {
            var doc = thisc.Document;
            var item = rootmd.SelectedItem as TreeViewItem;
            item.DataContext = doc;
            TextPointer position = doc.ContentStart;
            while (position != null)
            {
                //向前搜索,需要内容为Text       
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //拿出Run的Text        
                    string line = position.GetTextInRun(LogicalDirection.Forward);
                    //可能包含多个keyword,做遍历查找  
                    if (line.StartsWith("[") && line.Contains("]"))
                    {
                        int iend = line.IndexOf(']') + 1;
                        string keyword = line.Substring(0, iend);
                        //添加为新的Range
                        TextPointer start = position.GetPositionAtOffset(0);
                        TextPointer end = start.GetPositionAtOffset(keyword.Length);
                        position = selecta(thisc, start, end, Brushes.Orange, FontWeights.Bold);
                    }

                }
                //文字指针向前偏移   
                position = position.GetNextContextPosition(LogicalDirection.Forward);

            }
            thisc.Selection.Select(doc.ContentEnd, doc.ContentEnd);
        }
        private void SetSummaryStyle(RichTextBox thisc)
        {
            var doc = thisc.Document;
            var item = rootmd.SelectedItem as TreeViewItem;
            item.DataContext = doc;
            TextPointer position = doc.ContentStart;
            while (position != null)
            {
                //向前搜索,需要内容为Text       
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //拿出Run的Text        
                    string line = position.GetTextInRun(LogicalDirection.Forward);
                    System.Diagnostics.Debug.WriteLine(line);
                    if (line.Contains(";"))
                    {
                        //添加为新的Range
                        var text = line.Substring(line.IndexOf(';'), line.IndexOf('\n'));
                        TextPointer start = position.GetPositionAtOffset(line.IndexOf(';'));
                        TextPointer end = start.GetPositionAtOffset(text.Length);
                        position = selecta(thisc, start, end, Brushes.Green, FontWeights.Bold);
                    }

                }
                //文字指针向前偏移   
                position = position.GetNextContextPosition(LogicalDirection.Forward);

            }
            thisc.Selection.Select(doc.ContentEnd, doc.ContentEnd);
        }

        public void SetStyle(RichTextBox thisc)
        {
            //设置文字指针为Document初始位置
            TextPointer position = thisc.Document.ContentStart;
            while (position != null)
            {
                //向前搜索,需要内容为Text
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //拿出Run的Text
                    string line = position.GetTextInRun(LogicalDirection.Forward);
                    System.Diagnostics.Debug.WriteLine(line);
                    if (line.StartsWith("[") && line.Contains("]"))
                    {
                        int iend = line.IndexOf(']') + 1;
                        string keyword = line.Substring(0, iend);
                        //添加为新的Range
                        TextPointer start = position.GetPositionAtOffset(0);
                        TextPointer end = start.GetPositionAtOffset(keyword.Length);
                        position = selecta(thisc, start, end, Brushes.Orange, FontWeights.Bold);
                    }
                    if (line.Contains(";"))
                    {
                        //添加为新的Range
                        TextPointer start = position.GetPositionAtOffset(line.IndexOf(';'));
                        TextPointer end = start.GetPositionAtOffset(line.Length - line.IndexOf(';'));
                        position = selecta(thisc, start, end, Brushes.Green, FontWeights.Bold);
                    }
                    if (line.Contains("="))
                    {
                        var equalloc = line.IndexOf('=');
                        var left = line.Split('=')[0].Trim();
                        var right = line.Split('=')[1].Trim();
                        TextPointer qstart = position.GetPositionAtOffset(equalloc);
                        TextPointer lstart = position.GetPositionAtOffset(line.IndexOf(left));
                        TextPointer lend = lstart.GetPositionAtOffset(left.Length);
                        position = selecta(thisc, qstart, qstart, Brushes.Magenta, FontWeights.Bold);
                        if (RegExp.IsMatch(left, "\\d*|\\+"))
                            position = selecta(thisc, lstart, lend, Brushes.YellowGreen, FontWeights.Bold);
                        else
                            position = selecta(thisc, lstart, lend, Brushes.Blue, FontWeights.Bold);

                    }

                    //position = selecta(thisc, thisc.Document.ContentEnd, thisc.Document.ContentEnd, Brushes.White, FontWeights.Normal);
                }
                if (position == null) break;
                //文字指针向前偏移
                //position = position.GetNextContextPosition(LogicalDirection.Forward);
                
            }
        }

        public void ChangeColor(Brush brush, RichTextBox richBox, string keyword)
        {
            //设置文字指针为Document初始位置           
            //richBox.Document.FlowDirection           
            TextPointer position = richBox.Document.ContentStart;
            while (position != null)
            {
                //向前搜索,需要内容为Text       
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //拿出Run的Text        
                    string text = position.GetTextInRun(LogicalDirection.Forward);
                    //可能包含多个keyword,做遍历查找           
                    int index = 0;
                    index = text.IndexOf(keyword, 0);
                    if (index != -1)
                    {
                        TextPointer start = position.GetPositionAtOffset(index);
                        TextPointer end = start.GetPositionAtOffset(keyword.Length);
                        position = selecta( richBox, start, end,brush,FontWeights.Normal);
                    }

                }
                //文字指针向前偏移   
                position = position.GetNextContextPosition(LogicalDirection.Forward);

            }
        }
        public TextPointer selecta(RichTextBox thisc, TextPointer tpStart, TextPointer tpEnd, Brush brush, FontWeight weights)
        {
            TextRange range = thisc.Selection;
            range.Select(tpStart, tpEnd);
            //高亮选择         

            range.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
            range.ApplyPropertyValue(TextElement.FontWeightProperty, weights);
            range.Select(tpEnd, tpEnd);
            range.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.White);
            range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);

            return tpEnd.GetNextContextPosition(LogicalDirection.Forward);
        }
        private void RefreshEdit_Click(object sender, RoutedEventArgs e)
        {
            var thisc = edit;
            var doc = thisc.Document;
            TextRange textRange = new TextRange(doc.ContentStart, doc.ContentEnd);
            RichtxtboxInput(textRange.Text.TrimEnd(), thisc);
            var pointer = thisc.CaretPosition;
            SetSummaryStyle(thisc);
            SetSectionStyle(thisc);
            thisc.CaretPosition = pointer;
        }

 */
