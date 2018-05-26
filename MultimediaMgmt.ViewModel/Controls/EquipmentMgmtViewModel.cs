using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class EquipmentMgmtViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<ClassRoomEx> ClassRoomExs { get; set; }
        public virtual ClassRoomEx SelectedClassRoomEx { get; set; }
        public virtual Dictionary<byte, string> Signals { get; set; }

        public virtual string RoomTotal { get; set; }
        public virtual string WaitIndiContent { get; set; }
        public virtual bool IsLoad { get; set; }
        public virtual bool SortVisible { get; set; }
        public List<int> ids = new List<int>();
        private IRestConnection restConnection = null;
        public EquipmentMgmtViewModel()
        {
            string url = Common.Helper.ConfigHelper.Main.WebUrl;
            if (!string.IsNullOrEmpty(url))
                restConnection = new RestConnection(url);
            Signals = Constants.Signals;
            //ClassRoomListRefresh();
            IsLoad = false;
            SortVisible = true;
        }

        public void ClassRoomListRefresh()
        {
            if (ids.Count <= 0)
                return;

            #region web获取版本
            if (restConnection == null)
                return;
            var crs = (from c in multimediaEntities.ClassRoom
                       where ids.Contains(c.Id)
                       select c).AsEnumerable();

            ICollection<WebClassRoom> classrooms = crs.Select(
                s => new WebClassRoom() { TerminalId = s.TerminalId }).ToList();
            try
            {
                JObject jo = restConnection.Post("api/TerminalInfo/QueryLastTerminalInfos", classrooms);
                if (jo.Value<bool>("success"))
                {
                    JArray ja = jo.Value<JArray>("data");
                    if (ja != null)
                    {
                        Collection<WebTerminalInfo> terminalInfos = ja.ToObject<Collection<WebTerminalInfo>>();
                        var data = from c in crs
                                   join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.Id
                                   join te in terminalInfos on c.TerminalId equals te.TerminalId into temp
                                   from t in temp.DefaultIfEmpty()
                                   where ids.Contains(c.Id)
                                   orderby c.RoomNum
                                   select new ClassRoomEx()
                                   {
                                       Id = c.Id,
                                       TerminalId = c.TerminalId,
                                       TerminalIp = c.TerminalIp,
                                       RoomName = c.RoomNum,
                                       BuildingId = c.BuildingId,
                                       BuildingName = b.BuildingName,
                                       Location = b.Location,
                                       Floor = c.Floor,
                                       VedioAddress = c.VedioAddress,
                                       System = (t == null ? false : (t.System ?? false)),
                                       FPD = false,
                                       ComputerStatus = (t == null ? false : (t.Computer_Status ?? false)),
                                       Projector = (t == null ? false : (t.Projector ?? false)),
                                       ProjectorScreen = (t == null ? false : (t.Projection_Screen ?? false)),
                                       Curtain = false,
                                       Lamp = false,
                                       Volume = (t == null ? false : (t.Volume_Mute ?? false)),
                                       Record = false,
                                       Lock_Status = (t == null ? false : (t.Lock_Status ?? false)),
                                       Door_Status = false,
                                       ACRelay1 = (t == null ? false : (t.AC_Relay1 ?? false)),
                                       Large_Screen = false,
                                       AirConitioner = false,
                                       ProjectionSignal = (t?.Projection_Signal),
                                       ComputerSignal = (t?.Computer_Signal),
                                       LAN1 = (t == null ? false : (t.LAN1 ?? false)),
                                       LAN2 = (t == null ? false : (t.LAN2 ?? false)),
                                       LAN3 = (t == null ? false : (t.LAN3 ?? false)),
                                       LAN4 = (t == null ? false : (t.LAN4 ?? false)),
                                       Alarm_In1 = (t == null ? false : (t.Alarm_In1 ?? false)),
                                       Alarm_In2 = (t == null ? false : (t.Alarm_In2 ?? false)),
                                       Alarm_In3 = false,
                                       Alarm_In4 = false,
                                       Alarm_Control = (t == null ? false : (t.Alarm_Control ?? false)),
                                       IN_STATUS1 = false,
                                       IN_STATUS2 = false,
                                       IN_STATUS3 = false,
                                       //HexCode =(t?.HexCode),
                                       Opereate_Last = (t?.Opereate_Last),
                                       IsConnected = (t == null ? false : t.IsConnected),
                                       //暂时不知如何获取  取默认值
                                       //PersonId = "11111",
                                       //PersonName = "张三丰",
                                       //CourseName = "数据库系统概论",
                                       //ClassName = "2017级自动化1班",
                                       //StudentSum = 30,
                                       //RealStudentSum = 25,
                                       //Temperature = "25℃"
                                   };
                        ClassRoomExs = data.ToSmartObservableCollection();
                    }
                }
            }
            catch { }
            if (ClassRoomExs != null)
                RoomTotal = string.Format("已选教室数量:{0}", ClassRoomExs.Count);
            #endregion
            #region 最新数据库版本
            //var data = from c in multimediaEntities.ClassRoom
            //           join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.id
            //           join t in multimediaEntities.TerminalCurrentInfo on c.TerminalId equals t.TerminalID
            //           //join g in multimediaEntities.ClassGrade on c.Id equals g.ClassID
            //           where ids.Contains(c.Id)
            //           select new ClassRoomEx()
            //           {
            //               Id = c.Id,
            //               TerminalId = c.TerminalId,
            //               TerminalIp = c.TerminalIp,
            //               RoomName = c.RoomName,
            //               BuildingId = c.BuildingId,
            //               BuildingName = b.BuildingName,
            //               Location = b.Location,
            //               Floor = c.Floor,
            //               VedioAddress = c.VedioAddress,
            //               System = t.System,
            //               FPD = t.FPD,
            //               ComputerStatus = t.ComputerStatus,
            //               Projector = t.Projector,
            //               ProjectorScreen = t.ProjectorScreen,
            //               Curtain = t.Curtain,
            //               Lamp = t.Lamp,
            //               Volume = t.Volume,
            //               Record = t.Record,
            //               Lock_Status = t.Lock_Status,
            //               Door_Status = t.Door_Status,
            //               ACRelay1 = t.ACRelay1,
            //               Large_Screen = t.Large_Screen,
            //               AirConitioner = t.AirConitioner,
            //               ProjectionSignal = t.ProjectionSignal,
            //               ComputerSignal = t.ComputerSignal,
            //               LAN1 = t.LAN1,
            //               LAN2 = t.LAN2,
            //               LAN3 = t.LAN3,
            //               LAN4 = t.LAN4,
            //               Alarm_In1 = t.Alarm_In1,
            //               Alarm_In2 = t.Alarm_In2,
            //               Alarm_In3 = t.Alarm_In3,
            //               Alarm_In4 = t.Alarm_In4,
            //               Alarm_Control = t.Alarm_Control,
            //               IN_STATUS1 = t.IN_STATUS1,
            //               IN_STATUS2 = t.IN_STATUS2,
            //               IN_STATUS3 = t.IN_STATUS3,
            //               HexCode = t.HexCode,
            //               Opereate_Last = t.Opereate_Last,
            //               //暂时不知如何获取  取默认值
            //               PersonId = "11111",
            //               PersonName = "张三丰",
            //               CourseName = "数据库系统概论",
            //               ClassName = "2017级自动化1班",
            //               StudentSum = 30,
            //               RealStudentSum = 25,
            //               Temperature = "25℃"
            //           };
            ////获取正在上的课及老师
            ////int weekDay = (int)DateTime.Now.DayOfWeek;
            ////var course = multimediaEntities.StdCourseTable;
            ////var person = multimediaEntities.Person;
            ////DateTime now = DateTime.Now;
            ////foreach (ClassRoomEx cr in data)
            ////{
            ////    var temp = (from s in course
            ////                join p in person on s.PersonId equals p.PersonId
            ////                where s.DayOfWeek == weekDay &&
            ////                DateTime.ParseExact(s.BeginTime, "HH:mm", null) <= now &&
            ////                DateTime.ParseExact(s.BeginTime, "HH:mm", null) <= now
            ////                select new
            ////                {
            ////                    s.CourseName,
            ////                    p.PersonId,
            ////                    p.Name
            ////                }).FirstOrDefault();
            ////    if (temp != null)
            ////    {
            ////        cr.PersonId = temp.PersonId;
            ////        cr.PersonName = temp.Name;
            ////        cr.CourseName = temp.CourseName;
            ////    }
            ////}
            //ClassRoomExs = data.ToSmartObservableCollection();
            #endregion

        }
    }
}
