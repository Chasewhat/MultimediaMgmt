using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel.PopWindows;

namespace MultimediaMgmt.View.PopWindows
{
    /// <summary>
    /// Interaction logic for wndEquipmentLoanLogAddEdit.xaml
    /// </summary>
    public partial class wndEquipmentLoanLogAddEdit : DevExpress.Xpf.Core.DXWindow
    {
        private EquipmentLoanLogAddEditViewModel equipmentLoanLogAddEditViewModel;
        public wndEquipmentLoanLogAddEdit(int id = 0)
        {
            InitializeComponent();
            this.DataContext = equipmentLoanLogAddEditViewModel = ViewModelSource.Create(() => new EquipmentLoanLogAddEditViewModel(id));
            equipmentLoanLogAddEditViewModel.CloseWindow = () => { this.Close(); };
            equipmentLoanLogAddEditViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            };
        }
    }
}
