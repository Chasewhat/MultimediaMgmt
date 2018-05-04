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
    /// ucEquipmentControl.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentControl : UserControl
    {
        private EquipmentControlViewModel classControlViewModel;

        public delegate void StatusChangedEvent(ucEquipmentControl uc, bool isDetail);
        public event StatusChangedEvent StatusChanged;
        private bool isSet = false;

        public ucEquipmentControl()
        {
            InitializeComponent();
            this.DataContext = classControlViewModel = ViewModelSource.Create<EquipmentControlViewModel>();
        }

        private void showDetail_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (statusChange.IsChecked == true)
            {
                this.infoOverview.Visibility = Visibility.Collapsed;
                this.infoDetail.Visibility = Visibility.Visible;
                StatusChange(true);
            }
            else
            {
                this.infoDetail.Visibility = Visibility.Collapsed;
                this.infoOverview.Visibility = Visibility.Visible;
                if (!isSet)
                    StatusChange(false);
                isSet = false;
            }
        }

        private void StatusChange(bool isDetail)
        {
            StatusChangedEvent handler = StatusChanged;
            handler?.Invoke(this, isDetail);
        }

        public void StatusSet()
        {
            isSet = true;
            statusChange.IsChecked = false;
        }

        public void MonitorInit(bool flag)
        {
            this.infoDetail.MonitorInit(flag);
        }
    }
}