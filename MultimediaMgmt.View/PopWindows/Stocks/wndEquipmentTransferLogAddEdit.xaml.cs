using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel.PopWindows;

namespace MultimediaMgmt.View.PopWindows
{
    /// <summary>
    /// Interaction logic for wndEquipmentTransferLogAddEdit.xaml
    /// </summary>
    public partial class wndEquipmentTransferLogAddEdit : DevExpress.Xpf.Core.DXWindow
    {
        private EquipmentTransferLogAddEditViewModel equipmentTransferLogAddEditViewModel;
        public wndEquipmentTransferLogAddEdit(int id = 0)
        {
            InitializeComponent();
            this.DataContext = equipmentTransferLogAddEditViewModel = ViewModelSource.Create(() => new EquipmentTransferLogAddEditViewModel(id));
            equipmentTransferLogAddEditViewModel.CloseWindow = () => { this.Close(); };
            equipmentTransferLogAddEditViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            };
        }
    }
}
