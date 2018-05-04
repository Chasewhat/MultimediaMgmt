using MultimediaMgmt.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
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
            if (!Directory.Exists(Constants.LogPath))
                Directory.CreateDirectory(Constants.LogPath);
            //Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Current.MainWindow = new MainWindow();
            bool? dialogResult = new Login().ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                //base.OnStartup(e);
                Current.MainWindow.Show();
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                //未捕获异常处理
                Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            }
            else
            {
                this.Shutdown();
            }
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Common.Helper.LogHelper.Write(string.Format("【DispatcherUnhandledException】Message:{0}\nStackTrace:{1}\nInnerException:{2}\n{3}",
                e.Exception.Message, e.Exception.StackTrace,
                e.Exception.InnerException == null ? "" : e.Exception.InnerException.Message,
                e.Exception.InnerException == null ? "" : e.Exception.InnerException.StackTrace));
            DevExpress.Xpf.Core.DXMessageBox.Show(
                string.Format("很抱歉，当前应用程序遇到一些问题，该操作已经终止，请进行重试，如果问题继续存在，请联系管理员.\nException:{0}", e.Exception.Message),
                "意外的操作", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;//该异常被处理了，不再作为UnhandledException抛出了。
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            DevExpress.Xpf.Core.DXMessageBox.Show(
                string.Format("很抱歉，当前应用程序遇到一些问题，该操作已经终止，请进行重试，如果问题继续存在，请联系管理员.\nException:{0}",
                ex == null ? "" : ex.Message),
                "未捕获的异常", MessageBoxButton.OK, MessageBoxImage.Error);
            if (ex != null)
                Common.Helper.LogHelper.Write(string.Format("【UnhandledException】Message:{0}\nStackTrace:{1}\nInnerException:{2}\n{3}",
                ex.Message, ex.StackTrace,
                ex.InnerException == null ? "" : ex.InnerException.Message,
                ex.InnerException == null ? "" : ex.InnerException.StackTrace));
        }
    }
}
