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
    /// ucIcCardHistory.xaml 的交互逻辑
    /// </summary>
    public partial class ucIcCardHistory : UserControl
    {
        private IcCardHistoryViewModel icCardHistoryViewModel;
        public ucIcCardHistory()
        {
            InitializeComponent();
            this.DataContext = icCardHistoryViewModel = ViewModelSource.Create<IcCardHistoryViewModel>();
        }

    }
}