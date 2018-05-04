using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using MultimediaMgmt.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucEquipmentRepairLog.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentRepairLog : UserControl
    {
        private EquipmentRepairLogViewModel equipmentRepairLogViewModel;
        public ucEquipmentRepairLog()
        {
            InitializeComponent();
            this.DataContext = equipmentRepairLogViewModel = ViewModelSource.Create<EquipmentRepairLogViewModel>();
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (equipmentRepairLogViewModel.SelectedEquipmentRepairLog == null)
                return;
            new PopWindows.wndEquipmentRepairLogAddEdit(equipmentRepairLogViewModel.SelectedEquipmentRepairLog.ID)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentRepairLogViewModel.Query();
        }

        private void Add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            new PopWindows.wndEquipmentRepairLogAddEdit()
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentRepairLogViewModel.Query();
        }
    }
}