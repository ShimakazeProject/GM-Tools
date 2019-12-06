using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace CSFEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Plugin.Controls.CSWin
    {
        CSFFile CSFFile;// 编辑器核心 CSF文件
        CSFFile SearchReturn;// 搜索返回的文件
        string FilePath;// 文件地址

        /// <summary>
        /// 构造方法
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 标签列表显示的结构
        /// </summary>
        struct Label
        {
            public string LTag { get; private set; }
            public List<LKV> LKV { get; private set; }
            public Label(string tag, LKV lkv)
            {
                LTag = tag;
                LKV = new List<LKV>();
                LKV.Add(lkv);
            }
            /// <summary>
            /// 多字符串时使用
            /// </summary>
            /// <param name="lkv"></param>
            public void Add(LKV lkv)
            {
                LKV.Add(lkv);
            }
        }
        /// <summary>
        /// 值列表显示的结构
        /// </summary>
        struct LKV
        {
            public string Key { get; set; }
            public string[] Value { get; set; }
            public string EValue { get; set; }
            public LKV(string key, string[] value, string evalue)
            {
                Key = key;
                Value = value;
                EValue = evalue;
            }
        }
        private void ListUpdata(CSFFile CSFFile)
        {
            _dgTagList.Items.Clear();
            int i = 0;
            foreach (var Label in CSFFile.Label)
            {
                System.Diagnostics.Debug.WriteLine(i.ToString());
                var tag = Label.LabelString.Split(':');
                string lbl = string.Empty;
                if (tag.Length != 1) lbl = tag[0];
                else lbl = "(default)";
                ListAdd(lbl.ToUpper(), new LKV(Label.LabelString, Label.ValueString, Label.ExtraValue));
                i++;
            }
        }
        private void ListAdd(string Label, LKV lkv)
        {
            if (_dgTagList.Items.Count != 0)
            {
                for (int i = 0; i < _dgTagList.Items.Count; i++)
                {
                    Label item = (Label)_dgTagList.Items[i];
                    if (item.LTag == Label)
                    {
                        item.LKV.Add(lkv);
                        return;
                    }
                }
                _dgTagList.Items.Add(new Label(Label, lkv));
            }
            else _dgTagList.Items.Add(new Label(Label, lkv));
        }
        private async Task LoadFile()
        {
            CSFFile = new CSFFile();
            await CSFFile.LoadFromFile(FilePath);
            ListUpdata(CSFFile);
        }
        private async void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            var ofret = of.ShowDialog();
            if (ofret == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = of.FileName;
                await LoadFile();
            }

        }

        private void InfoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var fiw = new FileInfoWindow(CSFFile.Header);
            fiw.Show();
        }
        //
        private void F5MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ListUpdata(CSFFile);
        }
        private async void F5FileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            await LoadFile();
        }

        private void _dgTagList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _dgValueList.Items.Clear();
            if (_dgTagList.SelectedItem != null)
            {
                var lbl = (Label)_dgTagList.SelectedItem;
                foreach (var item in lbl.LKV)
                {
                    _dgValueList.Items.Add(item);
                }
            }
        }

        private async void OutTXTMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "文本文件(*.txt)|*.txt";
            var retsfd = sfd.ShowDialog();
            if (retsfd == System.Windows.Forms.DialogResult.OK)
            {
                await CSFFile.SaveAsText(sfd.FileName);
            }
            MessageBox.Show("导出完成", "提示");
            sfd.Dispose();
        }

        private void SearchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!_miSearch.IsChecked)
            {
                _rdSearch.Height = new GridLength(64);
                _miSearch.IsChecked = true;
            }
            else
            {
                _rdSearch.Height = new GridLength(0);
                _miSearch.IsChecked = false;
            }
        }


        private async void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            await CSFFile.SaveToFile(FilePath);
            MessageBox.Show("保存完成", "提示");
        }

        private async void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = "ra2md.csf",
                Filter = "红色警戒2 字符串文件(*.csf)|*.csf"
            };
            var retsfd = sfd.ShowDialog();
            if (retsfd == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = sfd.FileName;
                await CSFFile.SaveToFile(FilePath);
            }
            MessageBox.Show("保存完成", "提示");
            sfd.Dispose();
        }

        private void ExitMI_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddMI_Click(object sender, RoutedEventArgs e)
        {
            var add = new AddLabelWindow();
            var ret = add.ShowDialog(new CSFEditor.Label(), AddLabelWindow.EditMode.Add);
            CSFFile.AddLabel(ret);

        }

        private void ChangeMI_Click(object sender, RoutedEventArgs e)
        {
            var add = new AddLabelWindow();
            try
            {
                var a = (LKV)_dgValueList.SelectedItem;
                int kvid = -1;
                for (int i = 0; i < CSFFile.Label.Count; i++)
                {
                    if (CSFFile.Label[i].LabelString == a.Key)
                    {
                        kvid = i;
                    }
                }
                if (kvid == -1) throw new NullReferenceException();
                CSFFile.ChangeLabel(add.ShowDialog(CSFFile.Label[kvid], AddLabelWindow.EditMode.Change), kvid);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("未选择");
                return;
            }
        }

        private void DropMI_Click(object sender, RoutedEventArgs e)
        {
            var add = new AddLabelWindow();
            try
            {
                var a = (LKV)_dgValueList.SelectedItem;
                int kvid = -1;
                for (int i = 0; i < CSFFile.Label.Count; i++)
                {
                    if (CSFFile.Label[i].LabelString == a.Key)
                    {
                        kvid = i;
                    }
                }
                if (kvid == -1) throw new NullReferenceException();
                CSFFile.DropLabel(CSFFile.Label[kvid]);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("未选择");
                return;
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            search();
        }

        private void _tbGoSearch_TChange(object sender, TextChangedEventArgs e)
        {
            //search();
        }
        private void search()
        {
            SearchReturn = new CSFFile();
            SearchReturn.CleanLabels();
            int[] a = Array.Empty<int>(), b = Array.Empty<int>();
            if (_cbSLabel.IsChecked == true)
            {
                a = CSFFile.SearchLabel(_tbSearchBox.Text);
            }
            if (_cbSString.IsChecked == true)
            {
                b = CSFFile.SearchString(_tbSearchBox.Text);
            }

            List<int> tmp = b.ToList();
            tmp.AddRange(a.ToList());
            foreach (var i in tmp)
            {
                SearchReturn.Label.Add(CSFFile.Label[i]);
            }
            ListUpdata(SearchReturn);
        }
    }
}
