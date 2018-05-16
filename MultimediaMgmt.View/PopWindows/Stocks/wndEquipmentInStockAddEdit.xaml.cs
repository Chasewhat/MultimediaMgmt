using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel.PopWindows;

namespace MultimediaMgmt.View.PopWindows
{
    /// <summary>
    /// Interaction logic for wndEquipmentInStockAddEdit.xaml
    /// </summary>
    public partial class wndEquipmentInStockAddEdit : DevExpress.Xpf.Core.DXWindow
    {
        private EquipmentInStockAddEditViewModel equipmentInStockAddEditViewModel;
        public wndEquipmentInStockAddEdit(int id = 0)
        {
            InitializeComponent();
            this.DataContext = equipmentInStockAddEditViewModel = ViewModelSource.Create(() => new EquipmentInStockAddEditViewModel(id));
            equipmentInStockAddEditViewModel.CloseWindow = () => { this.Close(); };
            equipmentInStockAddEditViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            };
        }
    }
}
