using System;
using System.Windows;
using DevExpress.Xpf.Core;
using MultimediaMgmt.ViewModel;
using DevExpress.Mvvm.POCO;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using MultimediaMgmt.ViewModel.Notice;


namespace MultimediaMgmt.View
{
    /// <summary>
    /// Interaction logic for wndNotify.xaml
    /// </summary>
    public partial class wndNotify : DXWindow
    {
        private bool isclose = false;
        private int showSeconds = 5;
        public wndNotify(Notify notify)
        {
            InitializeComponent();
            this.DataContext = ViewModelSource.Create(() => new NotifyViewModel(notify));
            this.showSeconds = notify.ShowSeconds;
            this.Loaded += (s, e) => { wndNotify_Loaded(); };
            this.Closing += (s, e) => { wndNotify_Closing(s, e); };
            //this.MouseLeftButtonDown += (s, e) => { DragWindow(); };
        }

        private void wndNotify_Loaded()
        {
            this.UpdateLayout();
            //SystemSounds.Asterisk.Play();//播放提示声

            double bottom = System.Windows.SystemParameters.PrimaryScreenHeight;
            double right = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Left = right - this.ActualWidth;
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));//NotifyTimeSpan是自己定义的一个int型变量，用来设置动画的持续时间
            animation.From = bottom;
            animation.To = bottom - this.ActualHeight;//设定通知从下往上弹出
            this.BeginAnimation(Window.TopProperty, animation);//设定动画应用于窗体的Top属性

            if (showSeconds > 0)
            {
                Task.Factory.StartNew(delegate
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(showSeconds));
                    //Invoke到主进程中去执行
                    this.Dispatcher.Invoke(delegate
                        {
                            this.Close();
                        });
                });
            }
        }

        public void ImmediatelyClose()
        {
            try
            {
                isclose = true;
                this.Close();
            }
            catch { }
        }

        public void DragWindow()
        {
            this.DragMove();
        }
        private void wndNotify_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isclose)
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

                animation.Completed += (s, a) => { isclose = true; this.Close(); };
                animation.From = System.Windows.SystemParameters.PrimaryScreenHeight - this.ActualHeight;
                animation.To = System.Windows.SystemParameters.PrimaryScreenHeight;
                this.BeginAnimation(Window.TopProperty, animation);
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}
