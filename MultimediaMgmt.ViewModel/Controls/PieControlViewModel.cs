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
    public class PieControlViewModel : BaseViewModel
    {
        public virtual List<DataPie> Rates { get; protected set; }
        public virtual string Title { get; protected set; }

        public PieControlViewModel()
        {
        }

        public void Init(int buildingId, int type)
        {
            ClassroomBuilding building = multimediaEntities.ClassroomBuilding.FirstOrDefault(s => s.id == buildingId);
            if (building == null)
                return;
            int count = 0;
            int tcount = multimediaEntities.ClassRoom.Where(s => s.BuildingId == buildingId).Count();
            switch (type)
            {
                case 1:
                case 2:
                    count = (from c in multimediaEntities.ClassRoom
                             join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.id
                             join t in multimediaEntities.TerminalCurrentInfo on c.TerminalId equals t.TerminalID
                             where b.id == buildingId &&
                               t.System.HasValue && t.System.Value
                             select c).Count();
                    Rates = new List<DataPie>() {
                        new DataPie((type == 1 ? "在线设备" : "上课教室"), count, Brushes.DarkGreen),
                        new DataPie((type == 1 ? "离线设备" : "未上课教室"), tcount-count, Brushes.DarkRed)
                    };
                    break;
                case 3:
                    count = (from e in multimediaEntities.EquipmentRepairLog
                             where !e.RepairDate.HasValue
                             select e).Count();
                    Rates = new List<DataPie>() {
                        new DataPie("在修设备", count, Brushes.DarkGreen),
                        new DataPie("正常设备", tcount-count, Brushes.DarkRed)
                    };
                    break;
            }
            Title = string.Format("{0}:{1}%", building.BuildingName,
                    (tcount == 0 ? 0 : Math.Round((double)count / tcount * 100, 2)));
        }
    }
}
