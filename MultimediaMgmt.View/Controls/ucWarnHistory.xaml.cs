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
    /// ucWarnHistory.xaml 的交互逻辑
    /// </summary>
    public partial class ucWarnHistory : UserControl
    {
        private WarnHistoryViewModel warnHistoryViewModel;
        public ucWarnHistory()
        {
            InitializeComponent();
            this.DataContext = warnHistoryViewModel = ViewModelSource.Create<WarnHistoryViewModel>();
        }

    }
}