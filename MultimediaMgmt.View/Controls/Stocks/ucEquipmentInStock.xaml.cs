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
    /// ucEquipmentInStock.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentInStock : UserControl
    {
        private EquipmentInStockViewModel equipmentInStockViewModel;
        public ucEquipmentInStock()
        {
            InitializeComponent();
            this.DataContext = equipmentInStockViewModel = ViewModelSource.Create<EquipmentInStockViewModel>();
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (equipmentInStockViewModel.SelectedEquipmentInStock == null)
                return;
            new PopWindows.wndEquipmentInStockAddEdit(equipmentInStockViewModel.SelectedEquipmentInStock.ID)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentInStockViewModel.Query();
        }

        private void Add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            new PopWindows.wndEquipmentInStockAddEdit()
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentInStockViewModel.Query();
        }
    }
}