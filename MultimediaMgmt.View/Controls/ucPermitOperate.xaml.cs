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
    /// ucPermitOperate.xaml 的交互逻辑
    /// </summary>
    public partial class ucPermitOperate : UserControl
    {
        private PermitOperateViewModel permitOperateViewModel;
        public ucPermitOperate()
        {
            InitializeComponent();
            this.DataContext = permitOperateViewModel = ViewModelSource.Create<PermitOperateViewModel>();
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (permitOperateViewModel.SelectedPermit == null)
                return;
            new PopWindows.wndPermitAddEdit(permitOperateViewModel.SelectedPermit.Id)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            permitOperateViewModel.Query();
        }

        private void Add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            new PopWindows.wndPermitAddEdit()
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            permitOperateViewModel.Query();
        }
    }
}