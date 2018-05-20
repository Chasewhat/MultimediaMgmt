using DevExpress.Mvvm.POCO;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucMonitorMetaMgmt.xaml 的交互逻辑
    /// </summary>
    public partial class ucMonitorMgmt : UserControl
    {
        private MonitorMgmtViewModel monitorMgmtViewModel;
        private int monitorWidth = 300, monitorHeight = 200;
        private int monitorMax = 16;
        private List<ucMonitorMeta> monitors = new List<ucMonitorMeta>();
        private List<ucMonitorMeta> roomMonitors = new List<ucMonitorMeta>();
        private bool isShowRoom = false;
        private KeyValuePair<int, ucMonitorMeta> currMonitor;

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
            monitorMgmtViewModel.IsLoad = true;
            monitorMgmtViewModel.WaitIndiContent = "正在加载...";
            try
            {
                if (isChecked)
                {
                    //新增视频
                    ClassRoomEx cr = monitorMgmtViewModel.GetClassRoom(classRoom.ID.Value);
                    if (cr == null || string.IsNullOrEmpty(cr.VedioAddress))
                        return;
                    string[] address = cr.VedioAddress.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (address.Length <= 0)
                        return;
                    KeyValuePair<string, string> mediaUrls;
                    if (address.Length > 1)
                        mediaUrls = new KeyValuePair<string, string>(address[0], address[1]);
                    else
                        mediaUrls = new KeyValuePair<string, string>(address[0], address[0]);

                    ucMonitorMeta monitor = new ucMonitorMeta(
                        string.Format("{0}{1}", cr.BuildingName, cr.RoomName), mediaUrls, cr.Id);
                    monitor.Margin = new Thickness(2);
                    monitor.Width = double.NaN;
                    monitor.Height = double.NaN;
                    monitor.Tag = -1;
                    monitor.StatusChanged += StatusChangedExec;
                    monitors.Add(monitor);
                    this.overviewPanel.Children.Add(monitor);
                    //monitor.Play();

                }
                else
                {
                    if (isShowRoom && currMonitor.Value.Id == classRoom.ID)
                        RestoreInit();
                    //删除视频
                    ucMonitorMeta rm = monitors.FirstOrDefault(s => s.Id == classRoom.ID);
                    if (rm == null)
                        return;
                    monitors.Remove(rm);
                    this.overviewPanel.Children.Remove(rm);
                    rm.Dispose();
                }
            }
            catch
            { }
            finally
            {
                monitorMgmtViewModel.IsLoad = false;
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
                    this.overviewPanel.Rows = 4;
                    this.overviewPanel.Columns = 8;
                    break;
            }
            monitorMax = count;
        }

        private void RoomDispose()
        {
            if (roomMonitors.Count <= 0)
                return;
            foreach (ucMonitorMeta monitor in roomMonitors)
                monitor.Dispose();
            roomMonitors.Clear();
            this.overviewRoomPanel.Children.Clear();
        }
        private void RoomMonitorInit(int id)
        {
            this.monitorRoom.IsActive = true;
            RoomDispose();
            ClassRoomEx cr = monitorMgmtViewModel.GetClassRoom(id);
            if (cr == null || string.IsNullOrEmpty(cr.VedioAddress))
                return;
            int i = 0, n = 0;
            KeyValuePair<string, string> mediaUrls;
            string[] address = cr.VedioAddress.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ad in address)
            {
                if (n == 0)
                {
                    i++;
                    n++;
                    continue;
                }
                //最多取前四个
                if (n > 3)
                    break;
                if (i % 2 == 0)
                {
                    if (i + 2 > address.Length)
                        mediaUrls = new KeyValuePair<string, string>(address[i], address[i]);
                    else
                        mediaUrls = new KeyValuePair<string, string>(address[i], address[i + 1]);
                    string info = string.Format("{0}{1} {2}#视频源",
                        cr.BuildingName, cr.RoomName, n + 1);
                    ucMonitorMeta monitor = new ucMonitorMeta(info, mediaUrls, cr.Id);
                    monitor.Margin = new Thickness(2);
                    monitor.Width = double.NaN;
                    monitor.Height = double.NaN;
                    monitor.Tag = n - 1;
                    monitor.StatusChanged += RoomStatusChangedExec;
                    roomMonitors.Add(monitor);
                    monitor.Margin = new Thickness(2);
                    n++;
                }
                //monitor.Play();
                i++;
            }
            roomMonitors.ForEach(m =>
            {
                this.overviewRoomPanel.Children.Add(m);
            });
            this.overviewPanel.Children.Remove(currMonitor.Value);
            currMonitor.Value.StatusChanged -= StatusChangedExec;
            currMonitor.Value.StatusChanged += RoomStatusChangedExec;
            isShowRoom = true;
            if (roomMonitors.Count <= 0)
                this.listPanel.ItemWidth = new GridLength(0);
            else
                this.listPanel.ItemWidth = new GridLength(monitorWidth + 40);
        }

        private void RestoreInit()
        {
            this.monitorMain.IsActive = true;
            RoomDispose();
            this.detailPanel.Content = null;
            if (!monitors.Contains(currMonitor.Value))
                return;
            currMonitor.Value.StatusChanged -= RoomStatusChangedExec;
            currMonitor.Value.StatusChanged += StatusChangedExec;
            this.overviewPanel.Children.Insert(0, currMonitor.Value);
        }

        public void StatusChangedExec(ucMonitorMeta ucc, bool isDetail)
        {
            if (isDetail)
            {
                currMonitor = new KeyValuePair<int, ucMonitorMeta>(monitors.IndexOf(ucc), ucc);
                RoomMonitorInit(ucc.Id);
                this.detailPanel.Visibility = Visibility.Visible;
                this.overviewRoomPanel.Columns = 1;
                this.overviewRoomPanel.Rows = roomMonitors.Count;//最大视频数
                this.overviewRoomPanel.Height = ((monitorWidth + 40) * 0.75 + 4) * roomMonitors.Count;
                ucc.Width = double.NaN;
                ucc.Height = double.NaN;
                this.detailPanel.Content = ucc;
            }
            else
            {
                RestoreInit();
            }
        }

        public void RoomStatusChangedExec(ucMonitorMeta ucc, bool isDetail)
        {
            if (isDetail)
            {
                this.detailPanel.Visibility = Visibility.Visible;
                if (this.detailPanel.Content != null)
                {
                    ucMonitorMeta temp = this.detailPanel.Content as ucMonitorMeta;
                    temp.StatusSet();
                    temp.ChangeMedia(false);
                    //temp.Dispose();
                    this.detailPanel.Content = null;
                    int index = (int)temp.Tag;
                    if ((int)temp.Tag != -1 && index == 0)
                        index = 1;
                    if (index == -1)
                        index = 0;
                    this.overviewRoomPanel.Children.Insert(index, temp);
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
        //        (child as ucMonitorMeta).Width = monitorWidth;
        //}

        //private void heightChange_EditValueChanged(object sender, RoutedEventArgs e)
        //{
        //    monitorHeight = (int)heightChange.EditValue;
        //    foreach (var child in this.overviewPanel.Children)
        //        (child as ucMonitorMeta).Height = monitorHeight; ;
        //}
    }
}