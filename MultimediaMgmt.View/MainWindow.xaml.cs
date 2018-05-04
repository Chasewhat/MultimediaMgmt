using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using MultimediaMgmt.View.Controls;
using MultimediaMgmt.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultimediaMgmt.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mainViewModel;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Rect rect = SystemParameters.WorkArea;
            //this.MaxWidth = rect.Width + 10;
            //this.MaxHeight = rect.Height + 10;
            //this.WindowStartupLocation = WindowStartupLocation.Manual;
            //this.Left = rect.Left;
            //this.Top = rect.Top;
        }

        public void OperationSelect(int param)
        {
            switch (param)
            {
                case 1:
                    if (this.operMain.Content == null)
                        this.operMain.Content = new ucMainMgmt();
                    break;
                case 2:
                    if (this.operEquipment.Content == null)
                        this.operEquipment.Content = new ucEquipmentMgmt();
                    break;
                case 3:
                    if (this.operMonitor.Content == null)
                        this.operMonitor.Content = new ucMonitorMgmt();
                    break;
                case 4:
                    if (this.operCentralized.Content == null)
                        this.operCentralized.Content = new ucCentralizedControlMgmt();
                    break;
                case 5:
                    if (this.operWarn.Content == null)
                        this.operWarn.Content = new ucWarnMgmt();
                    break;
                case 6:
                    if (this.operPermit.Content == null)
                        this.operPermit.Content = new ucPermitMgmt();
                    break;
                case 7:
                    if (this.operCard.Content == null)
                        this.operCard.Content = new ucIcCardMgmt();
                    break;
                case 8:
                    if (this.operStock.Content == null)
                        this.operStock.Content = new ucEquipmentStockMgmt();
                    break;
            }
            this.docGroup.SelectedTabIndex = param - 1;
        }

        private void WindowMin(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void WindowMax(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void WindowNormal(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void WindowClose(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.DataContext = mainViewModel = ViewModelSource.Create<MainViewModel>();
            mainViewModel.OperationSelectAction = (para) => { OperationSelect(para); };
            biMain.IsChecked = true;
            mainViewModel.OperationSelect(biMain.CommandParameter.ToString());
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.barMax.IsVisible = true;
                this.barNormal.IsVisible = false;
            }
            if (this.WindowState == WindowState.Maximized)
            {
                this.barNormal.IsVisible = true;
                this.barMax.IsVisible = false;
            }
        }
    }
}
