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
    /// AddLabel.xaml 的交互逻辑
    /// </summary>
    public partial class AddLabelWindow : Window
    {
        bool IsOK = false;
        public AddLabelWindow()
        {
            InitializeComponent();
        }
        public Label ShowDialog(Label label, EditMode mode)
        {
            switch (mode)
            {
                case EditMode.Add:
                    break;
                case EditMode.Change:
                    _tbValue.Text = label.ValueString[0];
                    _tbLabel.Text = label.LabelString;
                    if (string.IsNullOrEmpty(label.ExtraValue))
                        _tbEValue.Text = label.ExtraValue;
                    break;
                default:
                    break;
            }
            base.ShowDialog();
            if (IsOK)
                if (string.IsNullOrWhiteSpace(_tbEValue.Text)) return new Label(" LBL", 1, _tbLabel.Text.Trim().Length, _tbLabel.Text.Trim(), "WRTS", new int[] { _tbValue.Text.Trim().Length }, new string[] { _tbValue.Text.Trim() }, _tbEValue.Text.Trim().Length, _tbEValue.Text.Trim());
                else return new Label(" LBL", 1, _tbLabel.Text.Trim().Length, _tbLabel.Text.Trim(), " RTS", new int[] { _tbValue.Text.Trim().Length }, new string[] { _tbValue.Text.Trim() });
            else return label;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsOK = true;
            Close();
        }
        public enum EditMode
        {
            Add, Change
        }
    }
}
