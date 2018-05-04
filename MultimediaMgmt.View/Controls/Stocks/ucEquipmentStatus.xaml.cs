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
    /// ucEquipmentStatus.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentStatus : UserControl
    {
        private EquipmentStatusViewModel equipmentStatusViewModel;
        public ucEquipmentStatus()
        {
            InitializeComponent();
            this.DataContext = equipmentStatusViewModel = ViewModelSource.Create<EquipmentStatusViewModel>();
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (equipmentStatusViewModel.SelectedEquipmentStatus == null)
                return;
            new PopWindows.wndEquipmentUseDateEdit(equipmentStatusViewModel.SelectedEquipmentStatus.ID)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentStatusViewModel.Query();
        }
    }
}