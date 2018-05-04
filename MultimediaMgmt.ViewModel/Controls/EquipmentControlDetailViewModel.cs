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
        public virtual ClassRoomEx CurrClassRoom { get; set; }
        public virtual Dictionary<int, string> Signals { get; set; }
        public virtual List<DataStandard> EnergyConsumptions { get; protected set; }

        public EquipmentControlDetailViewModel()
        {
            List<DataStandard> temp = new List<DataStandard>();
            Random rd = new Random();
            for (int i = 50; i > 0; i--)
            {
                temp.Add(new DataStandard(DateTime.Now.AddDays(-i), rd.Next(10, 50) + rd.NextDouble()));
            }
            EnergyConsumptions = temp;

            Signals = Constants.Signals;
        }

        public void Init(ClassRoomEx cr)
        {
            CurrClassRoom = cr;
        }
    }
}
