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

namespace CSFEditor
{
    /// <summary>
    /// FileInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FileInfoWindow : Window
    {
        public FileInfoWindow()
        {
            InitializeComponent();
        }
        public FileInfoWindow(Header header) : this()
        {
            _tbFlag.Text = header.Flag;
            _tbVersion.Text = header.Version.ToString();
            _tbNumLabel.Text = header.NumLabel.ToString();
            _tbNumString.Text = header.NumString.ToString();

            string result = string.Empty;
            foreach (var b in header.Message)//逐字节变为16进制字符
            {
                result += Convert.ToString(b, 16) + " ";
            }
            _tbUnknow.Text = result;
            _tbLanguage.Text = header.Language.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
