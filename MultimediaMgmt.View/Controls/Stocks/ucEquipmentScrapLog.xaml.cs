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
    /// ucEquipmentScrapLog.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentScrapLog : UserControl
    {
        private EquipmentScrapLogViewModel equipmentScrapLogViewModel;
        public ucEquipmentScrapLog()
        {
            InitializeComponent();
            this.DataContext = equipmentScrapLogViewModel = ViewModelSource.Create<EquipmentScrapLogViewModel>();
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (equipmentScrapLogViewModel.SelectedEquipmentScrapLog == null)
                return;
            new PopWindows.wndEquipmentScrapLogAddEdit(equipmentScrapLogViewModel.SelectedEquipmentScrapLog.ID)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentScrapLogViewModel.Query();
        }

        private void Add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            new PopWindows.wndEquipmentScrapLogAddEdit()
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentScrapLogViewModel.Query();
        }
    }
}