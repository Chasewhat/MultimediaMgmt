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
        public virtual bool TitleVisible { get; set; }

        public PieControlViewModel()
        {
            TitleVisible = true;
        }

        public void Init(int buildingId, int type)
        {
            int count = 0, tcount = 0;
            switch (type)
            {
                case 1:
                case 2:
                    ClassroomBuilding building = multimediaEntities.ClassroomBuilding.FirstOrDefault(s => s.id == buildingId);
                    if (building == null)
                        return;
                    tcount = multimediaEntities.ClassRoom.Where(s => s.BuildingId == buildingId).Count();
                    count = (from c in multimediaEntities.ClassRoom
                             join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.id
                             join t in multimediaEntities.TerminalCurrentInfo on c.TerminalId equals t.TerminalID
                             where b.id == buildingId &&
                               t.System.HasValue && t.System.Value
                             select c.Id).Count();
                    Rates = new List<DataPie>() {
                        new DataPie((type == 1 ? "在线设备" : "上课教室"), count, Brushes.DarkGreen),
                        new DataPie((type == 1 ? "离线设备" : "未上课教室"), tcount-count, Brushes.DarkRed)
                    };
                    Title = string.Format("{0}:{1}%", building.BuildingName,
                        (tcount == 0 ? 0 : Math.Round((double)count / tcount * 100, 2)));
                    break;
                case 3:
                    TitleVisible = false;
                    tcount = multimediaEntities.ClassRoom.Count();
                    EquipmentType etype = multimediaEntities.EquipmentType.AsEnumerable().FirstOrDefault(
                        s => Encoding.Default.GetString(s.EquipmentCategory) == "中控");
                    if (etype == null)
                    {
                        count = 0;
                    }
                    else
                    {
                        string typeName = Encoding.Default.GetString(etype.EquipmentName);
                        count = (from e in multimediaEntities.EquipmentRepairLog
                                 join i in multimediaEntities.EquipmentInStock on e.SerialNumber equals i.SerialNumber
                                 where !e.RepairDate.HasValue && i.Name == typeName
                                 select e.ID).Count();
                    }
                    Rates = new List<DataPie>() {
                        new DataPie("正常设备", tcount-count, Brushes.DarkGreen),
                        new DataPie("在修设备", count, Brushes.DarkRed),
                    };
                    break;
            }
        }
    }
}
