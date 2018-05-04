using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel.PopWindows;

namespace MultimediaMgmt.View.PopWindows
{
    /// <summary>
    /// Interaction logic for wndEquipmentUseDateEdit.xaml
    /// </summary>
    public partial class wndEquipmentUseDateEdit : DevExpress.Xpf.Core.DXWindow
    {
        private EquipmentUseDateEditViewModel equipmentUseDateEditViewModel;
        public wndEquipmentUseDateEdit(int id = 0)
        {
            InitializeComponent();
            this.DataContext = equipmentUseDateEditViewModel = ViewModelSource.Create(() => new EquipmentUseDateEditViewModel(id));
            equipmentUseDateEditViewModel.CloseWindow = () => { this.Close(); };
            equipmentUseDateEditViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }
    }
}
