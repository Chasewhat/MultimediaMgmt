using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel.PopWindows;

namespace MultimediaMgmt.View.PopWindows
{
    /// <summary>
    /// Interaction logic for wndPermitAddEdit.xaml
    /// </summary>
    public partial class wndPermitAddEdit : DevExpress.Xpf.Core.DXWindow
    {
        private PermitAddEditViewModel permitAddEditViewModel;
        public wndPermitAddEdit(int id = 0)
        {
            InitializeComponent();
            this.DataContext = permitAddEditViewModel = ViewModelSource.Create(() => new PermitAddEditViewModel(id));
        }
    }
}
