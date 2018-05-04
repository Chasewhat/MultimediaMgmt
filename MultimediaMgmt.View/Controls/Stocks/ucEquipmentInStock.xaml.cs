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

    }
}