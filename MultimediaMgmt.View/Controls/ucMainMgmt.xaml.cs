using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Charts;
using MultimediaMgmt.Common.Helper;
using MultimediaMgmt.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;

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
            ConfigHelper.Main.Buildings1 = string.Join(",", ((List<object>)e.NewValue).Cast<int>().ToArray());
        }

        private void buildCb2_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            PiesInit(e.NewValue, e.OldValue, 2);
            ConfigHelper.Main.Buildings2 = string.Join(",", ((List<object>)e.NewValue).Cast<int>().ToArray());
        }

        private void roomCb_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            SplinesInit(e.NewValue);
            ConfigHelper.Main.ClassRooms = string.Join(",", ((List<object>)e.NewValue).Cast<int>().ToArray());
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
                pie.Tag = id;
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

        private void SplinesInit(object newb)
        {
            if (newb == null)
                return;
            List<int> rooms = ((List<object>)newb).Cast<int>().ToList();
            if (rooms.Count <= 0)
                return;
            powerChart.Series.Clear();
            foreach (int id in rooms)
            {
                SplineSeries2D ss = new SplineSeries2D();
                ss.DisplayName = mainMgmtViewModel.GetRoomNum(id);
                ss.ArgumentScaleType = ScaleType.DateTime;
                ss.ArgumentDataMember = "Date";
                ss.ValueDataMember = "Value";
                ss.DataSource = mainMgmtViewModel.GetPowerData();
                ss.MarkerVisible = true;
                ss.CrosshairLabelPattern = "{S}：{V:F3}KWH";
                LineStyle ls = new LineStyle(1);
                ls.DashCap = System.Windows.Media.PenLineCap.Triangle;
                ss.LineStyle = ls;
                powerChart.Series.Add(ss);
            }
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            repairPie.Init(0, 3);
            string config = ConfigHelper.Main.Buildings1;
            if (!string.IsNullOrEmpty(config))
                this.buildCb1.EditValue = config.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).Cast<object>().ToList();
            config = ConfigHelper.Main.Buildings2;
            if (!string.IsNullOrEmpty(config))
                this.buildCb2.EditValue = config.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).Cast<object>().ToList();
            config = ConfigHelper.Main.ClassRooms;
            if (!string.IsNullOrEmpty(config))
                this.roomCb.EditValue = config.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).Cast<object>().ToList();
            //每隔3秒刷新一次
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
            timer.Interval = TimeSpan.FromSeconds(int.Parse(System.Configuration.ConfigurationManager.AppSettings["RefreshInterval"]));
            timer.Tick += (s, se) => { Refresh(); };
            timer.Start();
        }

        private void Refresh()
        {
            foreach (var p1 in pies1.Children)
            {
                ucPieControl p = p1 as ucPieControl;
                if (p != null)
                    p.Init(int.Parse(p.Tag.ToString()), 1);
            }
            foreach (var p2 in pies2.Children)
            {
                ucPieControl p = p2 as ucPieControl;
                if (p != null)
                    p.Init(int.Parse(p.Tag.ToString()), 2);
            }
            repairPie.Init(0, 3);
        }
    }
}
