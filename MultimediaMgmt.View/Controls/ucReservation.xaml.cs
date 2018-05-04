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
    /// ucReservation.xaml 的交互逻辑
    /// </summary>
    public partial class ucReservation : UserControl
    {
        private ReservationViewModel reservationViewModel;
        public ucReservation()
        {
            InitializeComponent();
            this.DataContext = reservationViewModel = ViewModelSource.Create<ReservationViewModel>();
            reservationViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }

    }
}