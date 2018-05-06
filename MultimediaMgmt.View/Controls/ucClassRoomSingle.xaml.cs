using DevExpress.Mvvm.POCO;
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
    /// ucClassRoomSingle.xaml 的交互逻辑
    /// </summary>
    public partial class ucClassRoomSingle : UserControl
    {
        private ClassRoomSingleViewModel classRoomSingleViewModel;
        public delegate void SelectChangedEvent(CommonTree uc);
        public event SelectChangedEvent SelectChanged;
        public ucClassRoomSingle()
        {
            InitializeComponent();
            this.DataContext = classRoomSingleViewModel = ViewModelSource.Create<ClassRoomSingleViewModel>();
        }

        private void Tree_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            CommonTree row = e.NewItem as CommonTree;
            if (row != null && row.Items == null)
                SelectChanged(row);
        }
    }
}