using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace IniChecker
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Plugin.Controls.CSWin
    {
        public MainWindow()
        {
            InitializeComponent();
            FileInfo fi = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"Config\IniChecker.json");
            if (!fi.Exists)
            {
                string Regist = "Countries;InfantryTypes;VehicleTypes;AircraftTypes;BuildingTypes;SmudgeTypes;OverlayTypes;Animations;VoxelAnims;Particles;ParticleSystems;SuperWeaponTypes;Warheads;Tiberiums;TerrainTypes;VariableNames";
                string White = "Sleep;Harmless;Sticky;Attack;Move;Patrol;QMove;Retreat;Guard;Enter;Eaten;Capture;Harvest;Area Guard;Return;Stop;Ambush;Hunt;Unload;Sabotage;Construction;Selling;Repair;Rescue;Missile;Open;Clear;Rough;Rough;Road;Water;Rock;Wall;Tiberium;Weeds;Beach;Ice;Railroad;Tunnel;General;JumpjetControls;SpecialWeapons;AudioVisual;CrateRules;CombatDamage;Radiation;ElevationModel;WallModel;Sides;MultiplayerDialogSettings;Maximums;AI;AIGenerals;IQ;Colors;ColorAdd;Easy;Normal;Difficult;Powerups";
                string Key = "Primary;Secondary;Weapon;Warhead;Projectile";
                var json = new JsonObject();
                var array = new JsonArray();
                foreach (var word in White.Split(';'))
                {
                    array.Add(word);
                }
                json.Add("White List", array);
                array = new JsonArray();
                foreach (var word in Regist.Split(';'))
                {
                    array.Add(word);
                }
                json.Add("Regist List", array);
                array = new JsonArray();
                foreach (var word in Key.Split(';'))
                {
                    array.Add(word);
                }
                json.Add("Key List", array);
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Config");
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"Config\IniChecker.json", json.ToString());
            }

            
        }

        private void AddMI_Click(FileType type)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.DereferenceLinks = true;
            switch (type)
            {
                case FileType.Rules:
                    ofd.Filter = "Ra2规则配置文件(*.ini)|*.ini";
                    break;
                case FileType.Art:
                    ofd.Filter = "Ra2美术配置文件(*.ini)|*.ini";
                    break;
                default:
                    return;
            }
            var ofdret = ofd.ShowDialog();
            if (ofdret == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var item in ofd.FileNames)
                {
                    switch (type)
                    {
                        case FileType.Rules:
                            _dgFileList.Items.Add(new FileList(item));
                            break;
                        case FileType.Art:
                            _dgArtList.Items.Add(new FileList(item));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private void AddArtMI_Click(object sender, RoutedEventArgs e) => AddMI_Click(FileType.Art);

        private void AddRuleMI_Click(object sender, RoutedEventArgs e) => AddMI_Click(FileType.Rules);

        enum FileType
        {
            Rules,Art
        }

        struct FileList
        {
            public string FilePath { get; set; }
            public FileList(string path)
            {
                FilePath = path;
            }
        }

        private async Task Analyze()
        {
            _dgExceptionList.Items.Clear();
            var White = new List<string>();
            var Regist = new List<string>();
            var Key = new List<string>();
            try
            {
                var json = (JsonObject)JsonValue.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Config\IniChecker.json"));
                if (json.TryGetValue("White List", out JsonValue JV))
                {
                    var array = (JsonArray)JV;
                    foreach (string jv in array)
                    {
                        White.Add(jv.Replace("\r", string.Empty).Replace("\n", string.Empty));
                    }
                }
                if (json.TryGetValue("Regist List", out JV))
                {
                    var array = (JsonArray)JV;
                    foreach (string jv in array)
                    {
                        Regist.Add(jv.Replace("\r", string.Empty).Replace("\n", string.Empty));
                    }
                }
                if (json.TryGetValue("Key List", out JV))
                {
                    var array = (JsonArray)JV;
                    foreach (string jv in array)
                    {
                        Key.Add(jv.Replace("\r", string.Empty).Replace("\n", string.Empty));
                    }
                }
            }
            catch (DirectoryNotFoundException) { }
            List<CheckerCore.Except> es = new List<CheckerCore.Except>();
            var Check = new CheckerCore(Regist, White, Key);
            var art = new List<string>();
            var paths = new List<string>();
            foreach (FileList fl in _dgFileList.Items) paths.Add(fl.FilePath);
            foreach (FileList fl in _dgArtList.Items) art.Add(fl.FilePath);


            await Check.InitiationArtAsync(art.ToArray());
            await Check.InitiationAsync(paths.ToArray());
            es.AddRange(Check.Anazlyze());

            if (es.Count == 0) return;
            foreach (var item in es) _dgExceptionList.Items.Add(item);
            _lbl.Content = $"文件个数:{_dgFileList.Items.Count}, 共检查出{_dgExceptionList.Items.Count}个异常";
            //Debug.WriteLine($"文件个数:{paths.Count}, 共检查出{es.Count}个异常");

        }

        private void GoToMI_Click(object sender, RoutedEventArgs e)
        {
            var item = (IniCheck.Except)_dgExceptionList.SelectedItem;
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe"; //启动的应用程序名称
            startInfo.Arguments = "/c code -g " + item.FilePath + ":" + item.LineNumber;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(startInfo);
        }

        private void RemoveMI_Click(FileType type)
        {
            object a;
            switch (type)
            {
                case FileType.Rules:
                    a = _dgFileList.SelectedItem;
                    if (a != null)
                    {
                        (_dgFileList).Items.Remove(a);
                    }
                    break;
                case FileType.Art:
                    a = _dgArtList.SelectedItem;
                    if (a != null)
                    {
                        (_dgArtList).Items.Remove(a);
                    }
                    break;
                default:
                    break;
            }
        }
        private void RemoveRuleMI_Click(object sender, RoutedEventArgs e) => RemoveMI_Click(FileType.Rules);
        private void RemoveArtMI_Click(object sender, RoutedEventArgs e) => RemoveMI_Click(FileType.Art);

        private void ExitMI_Click(object sender, RoutedEventArgs e) => Close();

        private void SettingMI_Click(object sender, RoutedEventArgs e)
        {
            new SettingWindow().ShowDialog();
        }

        private async void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.Filter = "文本文件(*.txt)|*.txt";
            if (sfd.ShowDialog()== System.Windows.Forms.DialogResult.OK)
            {
                var fs = new FileStream(sfd.FileName, FileMode.Create);
                var sw = new StreamWriter(fs);
                await sw.WriteLineAsync($"异常信息\t|异常节\t\t|行号\t|异常文件");
                foreach (IniCheck.Except item in _dgExceptionList.Items)
                {
                    string section = item.Section;
                    if (section.Length <= 9) section += "\t";
                    await sw.WriteLineAsync($"{item.Exception}\t|{section}\t|{item.LineNumber}\t|{item.FilePath}");
                }
                await sw.FlushAsync();
                await fs.FlushAsync();
                sw.Close();
                fs.Close();
            }
            
            MessageBox.Show("导出完成");
        }

        private async void AnaMI_Click(object sender, RoutedEventArgs e)
        {
            _lbl.Content = "正在进行分析, 请稍后...";
            try
            {
                await Analyze();
            }
            catch (ArgumentNullException) { }
        }
    }
}
