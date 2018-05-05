using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucPieControl.xaml 的交互逻辑
    /// </summary>
    public partial class ucPieControl : UserControl
    {
        private PieControlViewModel pieControlViewModel;

        public ucPieControl()
        {
            InitializeComponent();
            this.DataContext = pieControlViewModel = ViewModelSource.Create<PieControlViewModel>();
        }

        /// <summary>
        /// 图表初始化
        /// </summary>
        /// <param name="buildingId">教学楼ID</param>
        /// <param name="type">1--设备在线率 2--教室上课率 3--设备在修</param>
        public void Init(int buildingId, int type)
        {
            pieControlViewModel.Init(buildingId, type);
        }
    }
}
