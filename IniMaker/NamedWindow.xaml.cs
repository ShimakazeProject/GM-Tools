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
    /// NamedWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NamedWindow : Plugin.Controls.CSWin
    {
        public string name = null;
        private string _name;
        public NamedWindow()
        {
            InitializeComponent();
        }
        public new string ShowDialog()
        {
            base.ShowDialog();
            return name;
        }

        private void newName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _name = (sender as TextBox).Text;
        }

        private void CSButton_Click(object sender, RoutedEventArgs e)
        {
            name = _name;
            Close();
        }
    }
}
