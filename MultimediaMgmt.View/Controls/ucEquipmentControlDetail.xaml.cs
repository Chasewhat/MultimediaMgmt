using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Bars;
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
        public ucEquipmentControlDetail()
        {
            InitializeComponent();
            this.DataContext = classControlDetailViewModel = ViewModelSource.Create<EquipmentControlDetailViewModel>();
        }

        public void MonitorInit(bool isShow = true)
        {
            if (this.monitorsPanel.Children.Count > 0)
            {
                foreach (var ui in this.monitorsPanel.Children)
                {
                    ucMonitor1 monitor = ui as ucMonitor1;
                    monitor.Dispose();
                }
                this.monitorsPanel.Children.Clear();
            }
            if (!isShow)
                return;
            monitorCount = new Random().Next(1, 5);
            if (monitorCount > 2)
            {
                monitorsPanel.Rows = monitorsPanel.Columns = monitorRows = monitorColumns = 2;
            }
            else if (monitorCount > 1)
            {
                monitorsPanel.Rows = monitorRows = 1;
                monitorsPanel.Columns = monitorColumns = 2;
            }
            else
            {
                monitorsPanel.Rows = monitorsPanel.Columns = monitorRows = monitorColumns = 1;
            }

            for (int i = 0; i < monitorCount; i++)
            {
                ucMonitor1 monitor = new ucMonitor1();
                monitor.Margin = new Thickness(5);
                monitor.Tag = i;
                monitor.StatusChanged += StatusChangedExec;
                this.monitorsPanel.Children.Add(monitor);
            }
        }

        public void StatusChangedExec(ucMonitor1 ucc, bool isDetail)
        {
            if (isDetail)
            {
                monitorsPanel.Rows = monitorsPanel.Columns = 1;
                foreach (var child in monitorsPanel.Children)
                {
                    if ((child as ucMonitor1).Tag != ucc.Tag)
                        (child as ucMonitor1).Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                monitorsPanel.Rows = monitorRows;
                monitorsPanel.Columns = monitorColumns;
                foreach (var child in monitorsPanel.Children)
                {
                    (child as ucMonitor1).Visibility = Visibility.Visible;
                }
            }
        }
    }
}