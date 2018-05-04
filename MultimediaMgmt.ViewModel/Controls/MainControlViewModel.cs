using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;
using System.Windows.Media;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class MainControlViewModel : BaseViewModel
    {
        public virtual List<DataStandard> EnergyConsumptions { get; protected set; }
        public virtual List<DataPie> EquipmentOnlineRates { get; protected set; }
        public virtual List<DataPie> EquipmentRepairRates { get; protected set; }
        public virtual List<DataPie> ClassroomRates { get; protected set; }

        public MainControlViewModel()
        {
            List<DataStandard> temp = new List<DataStandard>();
            Random rd = new Random();
            for (int i = 20; i > 0; i--)
            {
                temp.Add(new DataStandard(DateTime.Now.AddDays(-i), rd.Next(10, 50) + rd.NextDouble()));
            }
            
            EnergyConsumptions = temp;

            List<DataPie> temp1 = new List<DataPie>();
            List<DataPie> temp2 = new List<DataPie>();
            List<DataPie> temp3 = new List<DataPie>();
            temp1.Add(new DataPie() { Key = "在线设备", Value = 50, PColor = Brushes.DarkGreen });
            temp1.Add(new DataPie() { Key = "离线设备", Value = 30, PColor = Brushes.DarkRed });
            EquipmentOnlineRates = temp1;

            temp2.Add(new DataPie() { Key = "正常设备", Value = 70, PColor = Brushes.DarkGreen });
            temp2.Add(new DataPie() { Key = "在修设备", Value = 10, PColor = Brushes.DarkRed });
            EquipmentRepairRates = temp2;

            temp3.Add(new DataPie() { Key = "上课教室", Value = 60, PColor = Brushes.DarkGreen });
            temp3.Add(new DataPie() { Key = "未上课教室", Value = 20, PColor = Brushes.DarkRed });
            ClassroomRates = temp3;
        }
    }

    public class DataPie
    {
        public string Key { get; set; }

        public int Value { get; set; }

        public SolidColorBrush PColor { get; set; }
    }
}
