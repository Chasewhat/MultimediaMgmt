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
    /// ucClassroomReservationApproval.xaml 的交互逻辑
    /// </summary>
    public partial class ucClassroomReservationApproval : UserControl
    {
        private ClassroomReservationApprovalViewModel classroomReservationApprovalViewModel;
        public ucClassroomReservationApproval()
        {
            InitializeComponent();
            this.DataContext = classroomReservationApprovalViewModel = ViewModelSource.Create<ClassroomReservationApprovalViewModel>();
        }

        private void gridView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            classroomReservationApprovalViewModel.Edit();
        }
    }
}