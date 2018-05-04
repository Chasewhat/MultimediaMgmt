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
    public class EquipmentControlDetailViewModel : BaseViewModel
    {
        public virtual List<int> SignalSources { get; set; }
        public virtual List<DataStandard> EnergyConsumptions { get; protected set; }

        public EquipmentControlDetailViewModel()
        {
            SignalSources = new List<int>() { 1, 2, 3, 4 };
            List<DataStandard> temp = new List<DataStandard>();
            Random rd = new Random();
            for (int i = 50; i > 0; i--)
            {
                temp.Add(new DataStandard(DateTime.Now.AddDays(-i), rd.Next(10, 50) + rd.NextDouble()));
            }
            EnergyConsumptions = temp;
        }
    }
}
