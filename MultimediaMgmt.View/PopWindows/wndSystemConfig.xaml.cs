using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel.PopWindows;

namespace MultimediaMgmt.View.PopWindows
{
    /// <summary>
    /// Interaction logic for wndSystemConfig.xaml
    /// </summary>
    public partial class wndSystemConfig : DevExpress.Xpf.Core.DXWindow
    {
        private SystemConfigViewModel systemConfigViewModel;
        public wndSystemConfig(int id = 0)
        {
            InitializeComponent();
            this.DataContext = systemConfigViewModel = ViewModelSource.Create<SystemConfigViewModel>();
        }
    }
}
