namespace Plugin
{
    using IniChecker;
    class Main:IMain
    {
        public string Name() => "INI检查器";
        public string Version() => "1.005α";
        public string Summary() => "INI检查器可以对INI文件进行检查, 可以找出未被使用过的Section";
        public string Inventors() => "舰队的偶像-岛风酱,小星星";
        public string Copyright() => "Copyright ©  2019 舰队的偶像-岛风酱 , All Rights Reserved.";
        public string[] Librarys() => new string[]{
            "System.Json, Version=2.0.8.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
        };
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
