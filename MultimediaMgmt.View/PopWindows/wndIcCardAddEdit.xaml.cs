using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel.PopWindows;

namespace MultimediaMgmt.View.PopWindows
{
    /// <summary>
    /// Interaction logic for wndIcCardAddEdit.xaml
    /// </summary>
    public partial class wndIcCardAddEdit : DevExpress.Xpf.Core.DXWindow
    {
        private IcCardAddEditViewModel icCardAddEditViewModel;
        public wndIcCardAddEdit(int id = 0)
        {
            InitializeComponent();
            this.DataContext = icCardAddEditViewModel = ViewModelSource.Create(() => new IcCardAddEditViewModel(id));
            icCardAddEditViewModel.CloseWindow = () => { this.Close(); };
            icCardAddEditViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }
    }
}
