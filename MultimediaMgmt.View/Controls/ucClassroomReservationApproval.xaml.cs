using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using MultimediaMgmt.Model.Models;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            classRoomTree.SelectChanged += SelectChangedExec;
        }

        public void SelectChangedExec(CommonTree classRoom)
        {
            courseControl.SelectChangedExec(classRoom);
            classroomReservationApprovalViewModel.RoomId = classRoom.ID;
        }
    }
}