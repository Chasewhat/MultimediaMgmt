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
    /// ucClassRoom.xaml 的交互逻辑
    /// </summary>
    public partial class ucClassRoom : UserControl
    {
        private ClassRoomViewModel classRoomViewModel;
        public delegate void CheckedChangedEvent(CommonTree uc, bool isChecked);
        public event CheckedChangedEvent CheckedChanged;
        public ucClassRoom()
        {
            InitializeComponent();
            this.DataContext = classRoomViewModel = ViewModelSource.Create<ClassRoomViewModel>();
        }

        private void Tree_NodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.Level == 2 && e.Node.IsChecked!=null && e.Row!=null)
            {
                CheckedChanged(e.Row as CommonTree, e.Node.IsChecked.Value);
            }
        }
    }
}