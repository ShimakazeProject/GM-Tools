using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
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

namespace IniChecker
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent(); try
            {
                var json = (JsonObject)JsonValue.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Config\IniChecker.json"));
                if (json.TryGetValue("White List", out JsonValue JV))
                {
                    var array = (JsonArray)JV;
                    foreach (string jv in array)
                    {
                        _tb.Text += $"{jv.Replace("\r", string.Empty).Replace("\n", string.Empty)};";
                    }
                }
                if (json.TryGetValue("Regist List", out JV))
                {
                    var array = (JsonArray)JV;
                    foreach (string jv in array)
                    {
                        _tb2.Text += $"{jv.Replace("\r", string.Empty).Replace("\n", string.Empty)};";
                    }
                }
                if (json.TryGetValue("Key List", out JV))
                {
                    var array = (JsonArray)JV;
                    foreach (string jv in array)
                    {
                        _tb3.Text += $"{jv.Replace("\r", string.Empty).Replace("\n", string.Empty)};";
                    }
                }
            }
            catch (DirectoryNotFoundException) { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var json = new JsonObject();
            var array = new JsonArray();
            _tb.Text = _tb.Text.Replace("\r", string.Empty).Replace("\n", string.Empty);
            _tb2.Text = _tb2.Text.Replace("\r", string.Empty).Replace("\n", string.Empty);
            while (true)
            {
                if (_tb.Text.Contains(";;")) _tb.Text = _tb.Text.Replace(";;", string.Empty);
                else if (_tb2.Text.Contains(";;")) _tb2.Text = _tb2.Text.Replace(";;", string.Empty);
                else if (_tb3.Text.Contains(";;")) _tb3.Text = _tb3.Text.Replace(";;", string.Empty);
                else break;
            }
            foreach (var word in _tb.Text.Split(';'))
            {
                array.Add(word);
            }
            json.Add("White List", array);
            array = new JsonArray();
            foreach (var word in _tb2.Text.Split(';'))
            {
                array.Add(word);
            }
            json.Add("Regist List", array);
            array = new JsonArray();
            foreach (var word in _tb3.Text.Split(';'))
            {
                array.Add(word);
            }
            json.Add("Key List", array);
            try
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"Config\IniChecker.json", json.ToString());
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Config");
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"Config\IniChecker.json", json.ToString());
            }
            Close();
        }
    }
}
