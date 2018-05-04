using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel;
using System.Windows.Input;

namespace MultimediaMgmt.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        DispatcherTimer closetime = new DispatcherTimer();
        private LoginViewModel loginViewModel;
        public Login()
        {
            InitializeComponent();
            this.DataContext = loginViewModel = ViewModelSource.Create<LoginViewModel>();
            userName.Focus();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string result = string.Empty;
            if (!loginViewModel.Login(ref result))
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(result, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Storyboard stb = (Storyboard)this.FindResource("Storyboard1");
            stb.Begin(this);
            closetime.Interval = TimeSpan.FromMilliseconds(1800);//设定计时器，当登录动画播放完成后关闭登录窗体
            closetime.Tick += (s, se) => { closetime_Tick(); };
            closetime.Start();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void closetime_Tick()
        {
            closetime.Stop();
            this.DialogResult = true;
            this.Close();
            //main.Show();
        }

        private void Login_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(null, null);
                e.Handled = true;
                return;
            }
        }
    }
}
