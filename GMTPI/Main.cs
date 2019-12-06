namespace Plugin
{
    class Main:IMain
    {
        public string Name() => "Crape Studio Plugin Core";
        public string Version() => "1.0.0.0";
        public string Summary() => "这是一个支持库库..应该放到Librarys文件夹中";
        public string Inventors() => "舰队的偶像-岛风酱";
        public string Copyright() => "Copyright ©  2019 舰队的偶像-岛风酱 , All Rights Reserved.";
        public string[] Librarys() => new string[]{
        };
        public void Start()
        {
            var w = new Demo.Window1();
            w.Show();
        }
        public PluginInfo Info()
        {
            return new PluginInfo();
        }
    }
}
