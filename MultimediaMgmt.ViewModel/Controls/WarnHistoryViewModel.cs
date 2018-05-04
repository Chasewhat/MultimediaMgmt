﻿using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;
using MultimediaMgmt.Common.Helper;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class WarnHistoryViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<WarnOperate> WarnHistorys { get; set; }
        public virtual int? BuildingId { get; set; }
        public virtual string TerminalId { get; set; }
        public virtual DateTime BeginDate { get; set; }
        public virtual DateTime EndDate { get; set; }

        public virtual List<KeyValuePair<int,string>> Buildings { get; set; }

        public WarnHistoryViewModel()
        {
            Buildings = multimediaEntities.ClassroomBuilding.Select(s => new
            {
                Key = s.id,
                Value = s.BuildingName
            }).AsEnumerable().Select(s =>
                            new KeyValuePair<int, string>(s.Key, s.Value)).ToList();
            BeginDate = EndDate = DateTime.Now.Date;
        }

        [Command]
        public void WarnOperateQuery()
        {
            var data = from b in multimediaEntities.ClassroomBuilding
                       join c in multimediaEntities.ClassRoom on b.id equals c.BuildingId
                       join t in multimediaEntities.AlarmInfo on c.TerminalId equals t.TerminalId
                       select new WarnOperate()
                       {
                           BuildingId = b.id,
                           ClassRoomId = c.Id,
                           TerminalId = t.TerminalId,
                           BuildingName = b.BuildingName,
                           RoomName = c.RoomName,
                           Alarm_In1 = t.Alarm1,
                           Alarm_In2 = t.Alarm2,
                           Alarm_In3 = t.Alarm3,
                           Alarm_In4 = t.Alarm4,
                           ReportTime = t.ReportTime
                       };
            if (BuildingId.HasValue && BuildingId.Value > 0)
                data = data.Where(s => s.BuildingId == BuildingId.Value);
            if (!string.IsNullOrEmpty(TerminalId))
                data = data.Where(s => s.TerminalId == TerminalId);

            data = data.Where(s => s.ReportTime >= BeginDate && s.ReportTime <= EndDate);

            WarnHistorys = data.ToSmartObservableCollection();
        }
    }
}
