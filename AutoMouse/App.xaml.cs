using AutoMouse.Controls;
using AutoMouse.InputHandler;
using AutoMouse.Properties;
using System;
using System.Windows;
using System.Windows.Threading;

namespace AutoMouse
{
    public partial class App : Application
    {
        public Clicker ClickExecutor { get; private set; }


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ClickExecutor = new Clicker();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ClickExecutor.Dispose();
            Settings.Default.Save();
        }
    }
}
