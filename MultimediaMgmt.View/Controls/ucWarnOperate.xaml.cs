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
    /// ucWarnOperate.xaml 的交互逻辑
    /// </summary>
    public partial class ucWarnOperate : UserControl
    {
        private WarnOperateViewModel warnOperateViewModel;
        public ucWarnOperate()
        {
            InitializeComponent();
            this.DataContext = warnOperateViewModel = ViewModelSource.Create<WarnOperateViewModel>();
            warnOperateViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            };
        }

    }
}