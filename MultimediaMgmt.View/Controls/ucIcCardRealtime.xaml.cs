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
    /// ucIcCardRealtime.xaml 的交互逻辑
    /// </summary>
    public partial class ucIcCardRealtime : UserControl
    {
        private IcCardRealtimeViewModel icCardRealtimeViewModel;
        public ucIcCardRealtime()
        {
            InitializeComponent();
            this.DataContext = icCardRealtimeViewModel = ViewModelSource.Create<IcCardRealtimeViewModel>();
        }

    }
}