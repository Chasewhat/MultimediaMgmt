using DevExpress.Mvvm.POCO;
using MultimediaMgmt.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucMainMgmt.xaml 的交互逻辑
    /// </summary>
    public partial class ucMainMgmt : UserControl
    {
        private MainMgmtViewModel mainMgmtViewModel;

        public ucMainMgmt()
        {
            InitializeComponent();
            this.DataContext = mainMgmtViewModel = ViewModelSource.Create<MainMgmtViewModel>();
        }

        private void buildCb1_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            PiesInit(e.NewValue, e.OldValue, 1);
        }

        private void buildCb2_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            PiesInit(e.NewValue, e.OldValue, 2);
        }

        private void PiesInit(object newb, object oldb, int type)
        {
            switch (type)
            {
                case 1:
                    pies1.Children.Clear();
                    break;
                case 2:
                    pies2.Children.Clear();
                    break;
            }
            if (newb == null)
                return;
            List<int> buildings = ((List<object>)newb).Cast<int>().ToList();
            if (buildings.Count <= 0)
                return;
            double sq = Math.Sqrt(buildings.Count);
            switch (type)
            {
                case 1:
                    pies1.Columns = (int)Math.Ceiling(sq);
                    pies1.Rows = (int)Math.Round(sq);
                    break;
                case 2:
                    pies2.Columns = (int)Math.Ceiling(sq);
                    pies2.Rows = (int)Math.Round(sq);
                    break;
            }
            foreach (int id in buildings)
            {
                ucPieControl pie = new ucPieControl();
                pie.Init(id, type);
                switch (type)
                {
                    case 1:
                        pies1.Children.Add(pie);
                        break;
                    case 2:
                        pies2.Children.Add(pie);
                        break;
                }
            }
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            repairPie.Init(0, 3);
        }
    }
}
