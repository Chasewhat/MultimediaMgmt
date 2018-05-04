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
    /// ucEquipmentStockMgmt.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentStockMgmt : UserControl
    {
        public ucEquipmentStockMgmt()
        {
            InitializeComponent();
        }

        private void docGroup_SelectedItemChanged(object sender, DevExpress.Xpf.Docking.Base.SelectedItemChangedEventArgs e)
        {

        }
    }
}