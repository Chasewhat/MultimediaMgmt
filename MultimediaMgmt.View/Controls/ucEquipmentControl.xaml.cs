using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Bars;
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
    /// ucEquipmentControl.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentControl : UserControl
    {
        private EquipmentControlViewModel classControlViewModel;

        public delegate void StatusChangedEvent(ClassRoomEx uc);
        public event StatusChangedEvent StatusChanged;
        public int Id = 0;
        public string RoomNum = string.Empty;
        public ClassRoomEx currRoom = null;
        public ucEquipmentControl()
        {
            InitializeComponent();
            this.DataContext = classControlViewModel = ViewModelSource.Create<EquipmentControlViewModel>();
        }

        public void Init(ClassRoomEx cr)
        {
            Id = cr.Id;
            RoomNum = cr.RoomName;
            classControlViewModel.Init(cr);
            currRoom = cr;
        }

        private void StatusChange()
        {
            StatusChangedEvent handler = StatusChanged;
            handler?.Invoke(currRoom);
        }

        private void showDetail(object sender, ItemClickEventArgs e)
        {
            StatusChange();
        }
    }
}