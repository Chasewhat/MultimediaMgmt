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
    /// ucEquipmentLoanLog.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentLoanLog : UserControl
    {
        private EquipmentLoanLogViewModel equipmentLoanLogViewModel;
        public ucEquipmentLoanLog()
        {
            InitializeComponent();
            this.DataContext = equipmentLoanLogViewModel = ViewModelSource.Create<EquipmentLoanLogViewModel>();
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (equipmentLoanLogViewModel.SelectedEquipmentLoanLog == null)
                return;
            new PopWindows.wndEquipmentLoanLogAddEdit(equipmentLoanLogViewModel.SelectedEquipmentLoanLog.ID)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentLoanLogViewModel.Query();
        }

        private void Add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            new PopWindows.wndEquipmentLoanLogAddEdit()
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentLoanLogViewModel.Query();
        }
    }
}