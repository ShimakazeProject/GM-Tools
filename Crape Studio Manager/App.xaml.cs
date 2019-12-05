using Plugin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Crape_Studio_Manager
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static CSLogger Logger = new CSLogger("CS Manager");

        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            Logger.WriteLog(CSLogger.LogRank.INFO, "Application Exit");
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.ExceptLog(e.Exception);
        }
    }
}
