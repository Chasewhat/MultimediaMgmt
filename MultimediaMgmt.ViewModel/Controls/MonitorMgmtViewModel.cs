using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class MonitorMgmtViewModel:BaseViewModel
    {
        public Action<int> ShowCountExec;
        public MonitorMgmtViewModel()
        {

        }

        [Command]
        public void ShowCount(int count)
        {
            ShowCountExec?.Invoke(count);
        }

        public ClassRoomEx GetClassRoom(int id)
        {
            return (from c in multimediaEntities.ClassRoom
                       join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.id
                       select new ClassRoomEx()
                       {
                           Id = c.Id,
                           TerminalId = c.TerminalId,
                           TerminalIp = c.TerminalIp,
                           RoomName = c.RoomName,
                           BuildingId = c.BuildingId,
                           BuildingName = b.BuildingName,
                           Location = b.Location,
                           Floor = c.Floor,
                           VedioAddress = c.VedioAddress
                       }).FirstOrDefault(s=>s.Id==id);
        }
    }
}
