﻿using DevExpress.Mvvm.DataAnnotations;
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
    public class EquipmentMgmtViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<ClassRoomEx> ClassRoomExs { get; set; }
        public virtual ClassRoomEx SelectedClassRoomEx { get; set; }
        public virtual Dictionary<int, string> Signals { get; set; }
        public List<int> ids = new List<int>();
        public EquipmentMgmtViewModel()
        {
            Signals = Constants.Signals;
            ClassRoomListRefresh();
        }

        public void ClassRoomListRefresh()
        {
            if (ids.Count <= 0)
                return;
            var data = from c in multimediaEntities.ClassRoom
                       join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.id
                       join t in multimediaEntities.TerminalCurrentInfo on c.TerminalId equals t.TerminalID
                       where ids.Contains(c.Id)
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
                           VedioAddress = c.VedioAddress,
                           System = t.System,
                           FPD = t.FPD,
                           ComputerStatus = t.ComputerStatus,
                           Projector = t.Projector,
                           ProjectorScreen = t.ProjectorScreen,
                           Curtain = t.Curtain,
                           Lamp = t.Lamp,
                           Volume = t.Volume,
                           Record = t.Record,
                           Lock_Status = t.Lock_Status,
                           Door_Status = t.Door_Status,
                           ACRelay1 = t.ACRelay1,
                           Large_Screen = t.Large_Screen,
                           AirConitioner = t.AirConitioner,
                           ProjectionSignal = t.ProjectionSignal,
                           ComputerSignal = t.ComputerSignal,
                           LAN1 = t.LAN1,
                           LAN2 = t.LAN2,
                           LAN3 = t.LAN3,
                           LAN4 = t.LAN4,
                           Alarm_In1 = t.Alarm_In1,
                           Alarm_In2 = t.Alarm_In2,
                           Alarm_In3 = t.Alarm_In3,
                           Alarm_In4 = t.Alarm_In4,
                           Alarm_Control = t.Alarm_Control,
                           IN_STATUS1 = t.IN_STATUS1,
                           IN_STATUS2 = t.IN_STATUS2,
                           IN_STATUS3 = t.IN_STATUS3,
                           HexCode = t.HexCode,
                           Opereate_Last = t.Opereate_Last
                       };
            //获取正在上的课及老师
            int weekDay = (int)DateTime.Now.DayOfWeek;
            var course = multimediaEntities.StdCourseTable;
            var person = multimediaEntities.Person;
            DateTime now = DateTime.Now;
            foreach (ClassRoomEx cr in data)
            {
                var temp = (from s in course
                            join p in person on s.PersonId equals p.PersonId
                            where s.DayOfWeek == weekDay &&
                            DateTime.ParseExact(s.BeginTime, "HH:mm", null) <= now &&
                            DateTime.ParseExact(s.BeginTime, "HH:mm", null) <= now
                            select new
                            {
                                s.CourseName,
                                p.PersonId,
                                p.Name
                            }).FirstOrDefault();
                if (temp != null)
                {
                    cr.PersonId = temp.PersonId;
                    cr.PersonName = temp.Name;
                    cr.CourseName = temp.CourseName;
                }
            }
            ClassRoomExs = data.ToSmartObservableCollection();
        }
    }
}
