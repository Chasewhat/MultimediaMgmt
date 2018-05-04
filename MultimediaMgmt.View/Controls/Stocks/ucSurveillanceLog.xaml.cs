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
    /// ucSurveillanceLog.xaml 的交互逻辑
    /// </summary>
    public partial class ucSurveillanceLog : UserControl
    {
        private SurveillanceLogViewModel surveillanceLogViewModel;
        public ucSurveillanceLog()
        {
            InitializeComponent();
            this.DataContext = surveillanceLogViewModel = ViewModelSource.Create<SurveillanceLogViewModel>();
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (surveillanceLogViewModel == null)
                return;
            new PopWindows.wndSurveillanceLogAddEdit(surveillanceLogViewModel.SelectedSurveillanceLog.ID)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            surveillanceLogViewModel.Query();
        }

        private void Add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            new PopWindows.wndSurveillanceLogAddEdit()
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            surveillanceLogViewModel.Query();
        }
    }
}