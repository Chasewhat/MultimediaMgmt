using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MultimediaMgmt.View
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Common.Helper.ConfigHelper.Init();
            //Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Current.MainWindow = new MainWindow();
            bool? dialogResult = new Login().ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                //base.OnStartup(e);
                Current.MainWindow.Show();
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            }
            else
            {
                this.Shutdown();
            }
        }
    }
}
