using System;

namespace Crape_Studio_Manager
{
    struct PluginInfo
    {
        public string Name { get; private set; }
        public string Ver { get; private set; }
        public Type Type { get; private set; }
        public string Info { get; private set; }
        public PluginInfo(string name, string ver, Type type, string info)
        {
            Name = name;
            Ver = ver;
            Type = type;
            Info = info;
        }
    }
}
