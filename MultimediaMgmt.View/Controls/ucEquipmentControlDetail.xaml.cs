using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Bars;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucEquipmentControlDetail.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentControlDetail : UserControl
    {
        private EquipmentControlDetailViewModel classControlDetailViewModel;
        private int monitorCount = 0, monitorRows = 1, monitorColumns = 1;
        public int Id = 0;
        public ucEquipmentControlDetail()
        {
            InitializeComponent();
            this.DataContext = classControlDetailViewModel = ViewModelSource.Create<EquipmentControlDetailViewModel>();
        }

        public void Init(ClassRoomEx cr)
        {
            Id = cr.Id;
            classControlDetailViewModel.Init(cr);
            MonitorInit(true);
        }

        public void MonitorInit(bool isShow = true)
        {
            if (this.monitorsPanel.Children.Count > 0)
            {
                foreach (var ui in this.monitorsPanel.Children)
                {
                    ucMonitorMeta monitor = ui as ucMonitorMeta;
                    monitor.Dispose();
                }
                this.monitorsPanel.Children.Clear();
            }
            if (!isShow)
                return;
            if (classControlDetailViewModel.CurrClassRoom == null
                || string.IsNullOrEmpty(classControlDetailViewModel.CurrClassRoom.VedioAddress))
                return;
            string[] address = classControlDetailViewModel.CurrClassRoom.VedioAddress.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            monitorCount = address.Length;
            //最多取前四个
            if (monitorCount > 4)
            {
                string[] temp = new string[4];
                Array.Copy(address, temp, 4);
                address = temp;
            }
            if (monitorCount > 2)
            {
                monitorsPanel.Rows = monitorsPanel.Columns = monitorRows = monitorColumns = 2;
            }
            else if (monitorCount > 1)
            {
                monitorsPanel.Rows = monitorRows = 2;
                monitorsPanel.Columns = monitorColumns = 1;
            }
            else
            {
                monitorsPanel.Rows = monitorsPanel.Columns = monitorRows = monitorColumns = 1;
            }
            int i = 0;
            foreach (string ad in address)
            {
                string info = string.Format("{0}{1}",
                    classControlDetailViewModel.CurrClassRoom.BuildingName,
                    classControlDetailViewModel.CurrClassRoom.TerminalId);
                if (i > 0)
                    info += string.Format(" {0}#视频源", i + 1);
                ucMonitorMeta monitor = new ucMonitorMeta(info, ad, classControlDetailViewModel.CurrClassRoom.Id);
                monitor.Margin = new Thickness(5);
                monitor.Width = double.NaN;
                monitor.Height = double.NaN;
                monitor.Tag = i;
                monitor.StatusChanged += StatusChangedExec;
                this.monitorsPanel.Children.Add(monitor);
                i++;
            }
        }

        public void StatusChangedExec(ucMonitorMeta ucc, bool isDetail)
        {
            if (isDetail)
            {
                monitorsPanel.Rows = monitorsPanel.Columns = 1;
                foreach (var child in monitorsPanel.Children)
                {
                    if ((child as ucMonitorMeta).Tag != ucc.Tag)
                        (child as ucMonitorMeta).Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                monitorsPanel.Rows = monitorRows;
                monitorsPanel.Columns = monitorColumns;
                foreach (var child in monitorsPanel.Children)
                {
                    (child as ucMonitorMeta).Visibility = Visibility.Visible;
                }
            }
        }
    }
}