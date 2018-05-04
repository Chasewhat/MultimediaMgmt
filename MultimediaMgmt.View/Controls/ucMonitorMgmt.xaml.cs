using DevExpress.Mvvm.POCO;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucMonitorMgmt.xaml 的交互逻辑
    /// </summary>
    public partial class ucMonitorMgmt : UserControl
    {
        private MonitorMgmtViewModel monitorMgmtViewModel;
        private int monitorWidth = 300, monitorHeight = 200;
        private int monitorMax = 16;
        private List<ucMonitor1> monitors = new List<ucMonitor1>();
        private List<ucMonitor1> roomMonitors = new List<ucMonitor1>();
        private bool isShowRoom = false;
        private KeyValuePair<int, ucMonitor1> currMonitor;

        public ucMonitorMgmt()
        {
            InitializeComponent();
            this.DataContext = monitorMgmtViewModel = ViewModelSource.Create<MonitorMgmtViewModel>();
            monitorMgmtViewModel.ShowCountExec = (count) => { ShowCountExec(count); };
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            classRoomTree.CheckedChanged += CheckedChangedExec;
        }

        public void CheckedChangedExec(CommonTree classRoom, bool isChecked)
        {
            if (isChecked)
            { //新增视频
                ucMonitor1 monitor = new ucMonitor1();
                monitor.Margin = new Thickness(2);
                monitor.Width = double.NaN;
                monitor.Height = double.NaN;
                monitor.Tag = 0;
                monitor.StatusChanged += StatusChangedExec;
                monitors.Add(monitor);
                this.overviewPanel.Children.Add(monitor);
            }
            else
            { //删除视频
            }
        }

        public void ShowCountExec(int count)
        {
            switch (count)
            {
                case 1:
                    this.overviewPanel.Rows = this.overviewPanel.Columns = 1;
                    break;
                case 4:
                    this.overviewPanel.Rows = this.overviewPanel.Columns = 2;
                    break;
                case 9:
                    this.overviewPanel.Rows = this.overviewPanel.Columns = 3;
                    break;
                case 16:
                    this.overviewPanel.Rows = this.overviewPanel.Columns = 4;
                    break;
                case 32:
                    this.overviewPanel.Rows = 8;
                    this.overviewPanel.Columns = 4;
                    break;
            }
            monitorMax = count;
        }

        private void RoomDispose()
        {
            if (roomMonitors.Count <= 0)
                return;
            foreach (ucMonitor1 monitor in roomMonitors)
                monitor.Dispose();
            roomMonitors.Clear();
            this.overviewRoomPanel.Children.Clear();
        }
        private void RoomMonitorInit()
        {
            this.monitorRoom.IsActive = true;
            RoomDispose();
            for (int i = 0; i < 3; i++)
            {
                ucMonitor1 roomMonitor = new ucMonitor1();
                roomMonitor.Margin = new Thickness(2);
                roomMonitor.Width = double.NaN;
                roomMonitor.Height = double.NaN;
                roomMonitor.Tag = i + 1;
                roomMonitor.StatusChanged += RoomStatusChangedExec;
                roomMonitors.Add(roomMonitor);
            }
            roomMonitors.ForEach(m =>
            {
                this.overviewRoomPanel.Children.Add(m);
            });
            this.overviewPanel.Children.Remove(currMonitor.Value);
            currMonitor.Value.StatusChanged -= StatusChangedExec;
            currMonitor.Value.StatusChanged += RoomStatusChangedExec;
            isShowRoom = true;
        }

        private void RestoreInit()
        {
            this.monitorMain.IsActive = true;
            RoomDispose();
            this.detailPanel.Content = null;
            currMonitor.Value.StatusChanged -= RoomStatusChangedExec;
            currMonitor.Value.StatusChanged += StatusChangedExec;
            this.overviewPanel.Children.Insert(currMonitor.Key, currMonitor.Value);
        }

        public void StatusChangedExec(ucMonitor1 ucc, bool isDetail)
        {
            if (isDetail)
            {
                currMonitor = new KeyValuePair<int, ucMonitor1>(monitors.IndexOf(ucc), ucc);
                RoomMonitorInit();
                this.listPanel.ItemWidth = new GridLength(monitorWidth + 40);
                this.detailPanel.Visibility = Visibility.Visible;
                this.overviewRoomPanel.Columns = 1;
                this.overviewRoomPanel.Rows = roomMonitors.Count;//最大视频数
                ucc.Width = double.NaN;
                ucc.Height = double.NaN;
                this.detailPanel.Content = ucc;
            }
            else
            {
                RestoreInit();
            }
        }

        public void RoomStatusChangedExec(ucMonitor1 ucc, bool isDetail)
        {
            if (isDetail)
            {
                this.detailPanel.Visibility = Visibility.Visible;
                this.listPanel.ItemWidth = new GridLength(monitorWidth + 40);
                if (this.detailPanel.Content != null)
                {
                    ucMonitor1 temp = this.detailPanel.Content as ucMonitor1;
                    temp.StatusSet();
                    temp.Dispose();
                    this.detailPanel.Content = null;
                    this.overviewRoomPanel.Children.Insert((int)temp.Tag, temp);
                }
                this.overviewRoomPanel.Children.Remove(ucc);
                ucc.Width = double.NaN;
                ucc.Height = double.NaN;
                this.detailPanel.Content = ucc;
            }
            else
            {
                RestoreInit();
            }
        }

        //private void widthChange_EditValueChanged(object sender, RoutedEventArgs e)
        //{
        //    monitorWidth = (int)widthChange.EditValue;
        //    foreach (var child in this.overviewPanel.Children)
        //        (child as ucMonitor1).Width = monitorWidth;
        //}

        //private void heightChange_EditValueChanged(object sender, RoutedEventArgs e)
        //{
        //    monitorHeight = (int)heightChange.EditValue;
        //    foreach (var child in this.overviewPanel.Children)
        //        (child as ucMonitor1).Height = monitorHeight; ;
        //}
    }
}