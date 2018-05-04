using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
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
    /// ucEquipmentMgmt.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentMgmt : UserControl
    {
        private EquipmentMgmtViewModel classRoomMgmtViewModel;
        public ucEquipmentMgmt()
        {
            InitializeComponent();
            this.DataContext = classRoomMgmtViewModel = ViewModelSource.Create<EquipmentMgmtViewModel>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            classRoomTree.CheckedChanged += CheckedChangedExec;
            for (int i = 0; i < 20; i++)
            {
                ucEquipmentControl ucc = new ucEquipmentControl();
                ucc.Margin = new Thickness(5);
                ucc.Width = 180;
                ucc.Height = 200;
                ucc.Tag = i;
                ucc.StatusChanged += StatusChangedExec;
                this.overviewPanel.Children.Add(ucc);
            }
            this.detailPanel.CloseCommand = new DelegateCommand(() =>
             {
                 this.detailPanel.Visibility = Visibility.Collapsed;
                 this.listPanel.ItemWidth = new GridLength(1, GridUnitType.Star);
             });
        }

        public void CheckedChangedExec(CommonTree classRoom, bool isChecked)
        {
            if (isChecked)
            { //新增设备
            }
            else
            { //删除设备
            }
        }

        public void StatusChangedExec(ucEquipmentControl ucc, bool isDetail)
        {
            if (isDetail)
            {
                this.listPanel.ItemWidth = new GridLength(220);
                this.detailPanel.Visibility = Visibility.Visible;
                this.detailPanel.ShowCaption = false;
                DetailClear(true);
                this.overviewPanel.Children.Remove(ucc);
                ucc.Width = double.NaN;
                ucc.Height = double.NaN;
                this.detailPanel.Content = ucc;
                ucc.MonitorInit(true);
            }
            else
            {
                this.listPanel.ItemWidth = new GridLength(1, GridUnitType.Star);
                this.detailPanel.Visibility = Visibility.Collapsed;
                DetailClear();
            }
        }

        private void MatrixShow(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.listInfo.Visibility = Visibility.Collapsed;
            this.matrixInfo.Visibility = Visibility.Visible;
        }

        private void ListShow(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.matrixInfo.Visibility = Visibility.Collapsed;
            this.listInfo.Visibility = Visibility.Visible;
        }

        private void DetailClear(bool isSet = false)
        {
            if (this.detailPanel.Content == null)
                return;
            if (this.detailPanel.Content is ucEquipmentControl)
            {
                ucEquipmentControl temp = this.detailPanel.Content as ucEquipmentControl;
                if (isSet)
                    temp.StatusSet();
                temp.MonitorInit(false);
                this.detailPanel.Content = null;
                temp.Width = 180;
                temp.Height = 200;
                this.overviewPanel.Children.Insert((int)temp.Tag, temp);
            }
            if (this.detailPanel.Content is ucEquipmentControlDetail)
            {
                ucEquipmentControlDetail temp = this.detailPanel.Content as ucEquipmentControlDetail;
                temp.MonitorInit(false);
                this.detailPanel.Content = null;
            }
        }

        private void listView_RowDoubleClick(object sender, DevExpress.Xpf.Grid.RowDoubleClickEventArgs e)
        {
            DetailClear();
            this.listPanel.ItemWidth = new GridLength(220);
            this.detailPanel.Visibility = Visibility.Visible;
            this.detailPanel.Caption = "一号教学楼101";
            this.detailPanel.ShowCaption = true;
            ucEquipmentControlDetail detail = new ucEquipmentControlDetail();
            this.detailPanel.Content = detail;
            detail.MonitorInit(true);
        }
    }
}