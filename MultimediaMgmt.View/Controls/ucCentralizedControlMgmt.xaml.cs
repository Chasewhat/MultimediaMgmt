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
    /// ucCentralizedControlMgmt.xaml 的交互逻辑
    /// </summary>
    public partial class ucCentralizedControlMgmt : UserControl
    {
        private CentralizedControlMgmtViewModel centralizedControlMgmtViewModel;
        public ucCentralizedControlMgmt()
        {
            InitializeComponent();
            //DataControlBase.AllowInfiniteGridSize = true;
            this.DataContext = centralizedControlMgmtViewModel = ViewModelSource.Create<CentralizedControlMgmtViewModel>();
            centralizedControlMgmtViewModel.MessageShowFail = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            };
            centralizedControlMgmtViewModel.MessageShowSucc = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            };
        }

        private void gridView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "ControlSwitch" ||
                e.Column.FieldName == "AirConditionerSwitch" ||
                e.Column.FieldName == "LightingSwitch")
                centralizedControlMgmtViewModel.AllSwitchSet();
        }
    }
}