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
    /// ucCourseControl.xaml 的交互逻辑
    /// </summary>
    public partial class ucCourseControl : UserControl
    {
        private CourseControlViewModel courseControlViewModel;
        public ucCourseControl()
        {
            InitializeComponent();
            this.DataContext = courseControlViewModel = ViewModelSource.Create<CourseControlViewModel>();
        }

        public void SelectChangedExec(CommonTree classRoom)
        {
            courseControlViewModel.RoomId = classRoom.ID;
            courseControlViewModel.Query();
        }

        public void NotChange()
        {
            courseControlViewModel.NotChange();
        }
    }
}