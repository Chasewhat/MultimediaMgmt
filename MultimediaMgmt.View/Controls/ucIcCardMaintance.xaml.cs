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
    /// ucIcCardMaintance.xaml 的交互逻辑
    /// </summary>
    public partial class ucIcCardMaintance : UserControl
    {
        private IcCardMaintanceViewModel icCardMaintanceViewModel;
        public ucIcCardMaintance()
        {
            InitializeComponent();
            this.DataContext = icCardMaintanceViewModel = ViewModelSource.Create<IcCardMaintanceViewModel>();
            icCardMaintanceViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }

        private void Add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            new PopWindows.wndIcCardAddEdit()
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            icCardMaintanceViewModel.Query();
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (icCardMaintanceViewModel.SelectedIcCard == null)
                return;
            new PopWindows.wndIcCardAddEdit(icCardMaintanceViewModel.SelectedIcCard.Id)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            icCardMaintanceViewModel.Query();
        }
    }
}