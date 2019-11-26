namespace Plugin
{
    using CrapeStyle;
    class Main
    {
        public string Name() => "Crape StudioUI库";
        public string Version() => "1.000α";
        public string Summary() => "这是一套UI库..应该放到Librarys文件夹中";
        public string Inventors() => "舰队的偶像-岛风酱,小星星";
        public string Copyright() => "Copyright ©  2019 舰队的偶像-岛风酱 , All Rights Reserved.";
        public string[] Librarys() => new string[]{
        };
        public void Start()
        {
            var w = new Window1();
            w.Show();
        }
        public string Info()
        {
            return "UI库";
        }
    }
}
