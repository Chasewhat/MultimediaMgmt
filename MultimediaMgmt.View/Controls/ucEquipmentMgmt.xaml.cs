using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Windows.Threading;

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucEquipmentMgmt.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentMgmt : UserControl
    {
        private EquipmentMgmtViewModel classRoomMgmtViewModel;
        private List<ucEquipmentControl> equipments = new List<ucEquipmentControl>();
        public ucEquipmentMgmt()
        {
            InitializeComponent();
            this.DataContext = classRoomMgmtViewModel = ViewModelSource.Create<EquipmentMgmtViewModel>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            classRoomTree.CheckedChanged += CheckedChangedExec;
            this.detailPanel.CloseCommand = new DelegateCommand(() =>
             {
                 DetailClear();
                 this.detailPanel.Visibility = Visibility.Collapsed;
                 this.listPanel.ItemWidth = new GridLength(1, GridUnitType.Star);
             });
            //每隔60秒刷新一次
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, se) => { Refresh(); };
            timer.Start();
        }

        public void Refresh()
        {
            classRoomMgmtViewModel.ClassRoomListRefresh();
            if (classRoomMgmtViewModel.ClassRoomExs == null)
                return;
            foreach (ucEquipmentControl ucc in equipments)
            {
                ClassRoomEx cr = classRoomMgmtViewModel.ClassRoomExs.FirstOrDefault(s => s.Id == ucc.Id);
                ucc.Init(cr);
            }
            if (detailPanel.Content != null)
            {
                try
                {
                    if (this.detailPanel.Content is ucEquipmentControlDetail)
                    {
                        ucEquipmentControlDetail temp = this.detailPanel.Content as ucEquipmentControlDetail;
                        ClassRoomEx cr = classRoomMgmtViewModel.ClassRoomExs.FirstOrDefault(s => s.Id == temp.Id);
                        temp.Init(cr, true);
                    }
                    else if (this.detailPanel.Content is ucEquipmentControl)
                    {
                        ucEquipmentControl temp = this.detailPanel.Content as ucEquipmentControl;
                        ClassRoomEx cr = classRoomMgmtViewModel.ClassRoomExs.FirstOrDefault(s => s.Id == temp.Id);
                        temp.DetailInit(cr);
                    }
                }
                catch { }
            }
        }

        public void CheckedChangedExec(CommonTree classRoom, bool isChecked)
        {
            classRoomMgmtViewModel.IsLoad = true;
            classRoomMgmtViewModel.WaitIndiContent = "正在加载...";
            try
            {
                if (isChecked)
                {
                    //新增设备
                    if (!classRoomMgmtViewModel.ids.Contains(classRoom.ID.Value))
                        classRoomMgmtViewModel.ids.Add(classRoom.ID.Value);
                    classRoomMgmtViewModel.ClassRoomListRefresh();
                    if (classRoomMgmtViewModel.ClassRoomExs == null)
                        return;
                    ClassRoomEx cr = classRoomMgmtViewModel.ClassRoomExs.FirstOrDefault(s => s.Id == classRoom.ID);
                    if (cr == null)
                        return;
                    ucEquipmentControl ucc = new ucEquipmentControl();
                    ucc.Margin = new Thickness(5);
                    ucc.Width = 180;
                    ucc.Height = 200;
                    ucc.StatusChanged += StatusChangedExec;
                    equipments.Add(ucc);
                    this.overviewPanel.Children.Insert(0, ucc);
                    ucc.Init(cr);
                    //if (this.listPanel.ItemWidth == new GridLength(0))
                    //    this.listPanel.ItemWidth = new GridLength(220);
                }
                else
                {
                    //删除设备
                    ucEquipmentControl eq = equipments.FirstOrDefault(s => s.Id == classRoom.ID);
                    if (eq == null)
                        return;
                    equipments.Remove(eq);
                    this.overviewPanel.Children.Remove(eq);
                    //if (this.overviewPanel.Children.Count <= 0)
                    //    this.listPanel.ItemWidth = new GridLength(0);
                }
            }
            catch
            { }
            finally
            {
                classRoomMgmtViewModel.IsLoad = false;
            }
        }

        public void StatusChangedExec(ucEquipmentControl ucc, bool isDetail)
        {
            if (isDetail)
            {
                this.overviewPanel.Children.Remove(ucc);
                this.listPanel.ItemWidth = new GridLength(0);
                //if (this.overviewPanel.Children.Count <= 0)
                //    this.listPanel.ItemWidth = new GridLength(0);
                //else
                //    this.listPanel.ItemWidth = new GridLength(220);
                this.detailPanel.Visibility = Visibility.Visible;
                this.detailPanel.ShowCaption = false;
                DetailClear(true);
                ucc.Width = double.NaN;
                ucc.Height = double.NaN;
                this.detailPanel.Content = ucc;
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
                this.overviewPanel.Children.Insert(0, temp);
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
            if (classRoomMgmtViewModel.SelectedClassRoomEx == null)
                return;
            DetailClear();
            //this.listPanel.ItemWidth = new GridLength(220);
            this.listPanel.ItemWidth = new GridLength(0);
            this.detailPanel.Visibility = Visibility.Visible;
            this.detailPanel.Caption = string.Format("{0}{1}",
                    classRoomMgmtViewModel.SelectedClassRoomEx.BuildingName,
                    classRoomMgmtViewModel.SelectedClassRoomEx.TerminalId);
            this.detailPanel.ShowCaption = true;
            ucEquipmentControlDetail detail = new ucEquipmentControlDetail();
            this.detailPanel.Content = detail;
            detail.Init(classRoomMgmtViewModel.SelectedClassRoomEx);
        }
    }
}