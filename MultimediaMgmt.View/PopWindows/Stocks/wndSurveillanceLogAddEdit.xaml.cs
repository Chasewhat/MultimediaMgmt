using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel.PopWindows;

namespace MultimediaMgmt.View.PopWindows
{
    /// <summary>
    /// Interaction logic for wndSurveillanceLogAddEdit.xaml
    /// </summary>
    public partial class wndSurveillanceLogAddEdit : DevExpress.Xpf.Core.DXWindow
    {
        private SurveillanceLogAddEditViewModel surveillanceLogAddEditViewModel;
        public wndSurveillanceLogAddEdit(int id = 0)
        {
            InitializeComponent();
            this.DataContext = surveillanceLogAddEditViewModel = ViewModelSource.Create(() => new SurveillanceLogAddEditViewModel(id));
            surveillanceLogAddEditViewModel.CloseWindow = () => { this.Close(); };
            surveillanceLogAddEditViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            };
        }
    }
}
