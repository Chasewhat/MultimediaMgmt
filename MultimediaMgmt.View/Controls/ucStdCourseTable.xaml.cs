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
    /// ucStdCourseTable.xaml 的交互逻辑
    /// </summary>
    public partial class ucStdCourseTable : UserControl
    {
        public ucStdCourseTable()
        {
            InitializeComponent();
            classRoomTree.SelectChanged += courseControl.SelectChangedExec;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}