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
    /// ucEquipmentTransferLog.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentTransferLog : UserControl
    {
        private EquipmentTransferLogViewModel equipmentTransferLogViewModel;
        public ucEquipmentTransferLog()
        {
            InitializeComponent();
            this.DataContext = equipmentTransferLogViewModel = ViewModelSource.Create<EquipmentTransferLogViewModel>();
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (equipmentTransferLogViewModel.SelectedEquipmentTransferLog == null)
                return;
            new PopWindows.wndEquipmentTransferLogAddEdit(equipmentTransferLogViewModel.SelectedEquipmentTransferLog.ID)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentTransferLogViewModel.Query();
        }

        private void Add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            new PopWindows.wndEquipmentTransferLogAddEdit()
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentTransferLogViewModel.Query();
        }
    }
}