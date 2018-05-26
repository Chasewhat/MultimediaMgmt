using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel.PopWindows;

namespace MultimediaMgmt.View.PopWindows
{
    /// <summary>
    /// Interaction logic for wndEquipmentRepairLogAddEdit.xaml
    /// </summary>
    public partial class wndEquipmentRepairLogAddEdit : DevExpress.Xpf.Core.DXWindow
    {
        private EquipmentRepairLogAddEditViewModel equipmentRepairLogAddEditViewModel;
        public wndEquipmentRepairLogAddEdit(int id = 0, string serialNum = "")
        {
            InitializeComponent();
            this.DataContext = equipmentRepairLogAddEditViewModel = ViewModelSource.Create(() => new EquipmentRepairLogAddEditViewModel(id, serialNum));
            equipmentRepairLogAddEditViewModel.CloseWindow = () => { this.Close(); };
            equipmentRepairLogAddEditViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            };
        }
    }
}
