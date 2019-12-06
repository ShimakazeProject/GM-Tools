namespace Plugin
{
    using CSFEditor;
    class Main : IMain
    {
        public string Name() => "CSF编辑器";
        public string Version() => "1.001α";
        public string Summary() => "一个很普通的CSF编辑器";
        public string Inventors() => "舰队的偶像-岛风酱";
        public string Copyright() => "Copyright ©  2019 舰队的偶像-岛风酱 , All Rights Reserved.";
        public string[] Librarys() => new string[]{
        };

        public void Start()
        {
            var w = new MainWindow();
            w.Show();
        }
        public PluginInfo Info()
        {
            var info = new PluginInfo();
            info.Copyright = "Copyright ©  2019 舰队的偶像-岛风酱 , All Rights Reserved.";
            info.Inventors = "舰队的偶像-岛风酱";
            info.Summary= "一个很普通的CSF编辑器";
            info.Version= "1.001α";
            info.Name= "CSF编辑器";
            info.Librarys = new string[]
            {
                "PluginCore"
            };
            return info;
        }
    }
}
