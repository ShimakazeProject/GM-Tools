namespace Plugin
{
    using IniMaker;
    class Main:IMain
    {
        public string Name() => "INI 制作工具";
        public string Version() => "1.000α";
        public string Summary() => "";
        public string Inventors() => "舰队的偶像-岛风酱";
        public string Copyright() => "Copyright ©  2019 舰队的偶像-岛风酱 , All Rights Reserved.";
        public string[] Librarys() => new string[]{
        };
        
            //"HL.dll",
            //"ICSharpCode.AvalonEdit.dll"
        public void Start()
        {
            var w = new MainWindow();
            w.Show();
        }
        public PluginInfo Info()
        {
            return new PluginInfo();
        }
    }
}
