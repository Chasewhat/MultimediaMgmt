using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel.PopWindows;

namespace MultimediaMgmt.View.PopWindows
{
    /// <summary>
    /// Interaction logic for wndEquipmentScrapLogAddEdit.xaml
    /// </summary>
    public partial class wndEquipmentScrapLogAddEdit : DevExpress.Xpf.Core.DXWindow
    {
        private EquipmentScrapLogAddEditViewModel equipmentScrapLogAddEditViewModel;
        public wndEquipmentScrapLogAddEdit(int id = 0)
        {
            InitializeComponent();
            this.DataContext = equipmentScrapLogAddEditViewModel = ViewModelSource.Create(() => new EquipmentScrapLogAddEditViewModel(id));
            equipmentScrapLogAddEditViewModel.CloseWindow = () => { this.Close(); };
            equipmentScrapLogAddEditViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            };
        }
    }
}
