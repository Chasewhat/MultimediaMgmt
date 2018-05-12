using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;
using MultimediaMgmt.Common.Helper;
using MultimediaMgmt.ViewModel.Notice;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class WarnOperateViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<WarnOperate> WarnOperates { get; set; }
        public virtual WarnOperate SelectedWarnOperate { get; set; }
        public virtual int? BuildingId { get; set; }
        public virtual string TerminalId { get; set; }

        public virtual List<KeyValuePair<int, string>> Buildings { get; set; }

        private IRestConnection restConnection = null;

        public WarnOperateViewModel()
        {
            Buildings = multimediaEntities.ClassroomBuilding.Select(s => new
            {
                Key = s.id,
                Value = s.BuildingName
            }).AsEnumerable().Select(s =>
                            new KeyValuePair<int, string>(s.Key, s.Value)).ToList();
            string url = Common.Helper.ConfigHelper.Main.WebUrl;
            if (!string.IsNullOrEmpty(url))
                restConnection = new RestConnection(url);
        }

        [Command]
        public void WarnOperateQuery()
        {
            NOTICE.Publish_Notify(new Notify("警告", "当前为测试弹窗!", 0, NotifyType.Warn));
            #region web获取版本
            if (restConnection == null)
                return;
            var crs = (from c in multimediaEntities.ClassRoom
                       select c).AsEnumerable();
            if (BuildingId.HasValue && BuildingId.Value > 0)
                crs = crs.Where(s => s.BuildingId == BuildingId.Value);
            if (!string.IsNullOrEmpty(TerminalId))
                crs = crs.Where(s => s.TerminalId == TerminalId);

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
                                   join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.id
                                   join t in terminalInfos on c.TerminalId equals t.TerminalId
                                   select new WarnOperate()
                                   {
                                       BuildingId = b.id,
                                       ClassRoomId = c.Id,
                                       TerminalId = t.TerminalId,
                                       TerminalIp = c.TerminalIp,
                                       BuildingName = b.BuildingName,
                                       RoomName = c.RoomName,
                                       Alarm_Control = t.Alarm_Control,
                                       Alarm_In1 = t.Alarm_In1,
                                       Alarm_In2 = t.Alarm_In2
                                       //Alarm_In3 = t.Alarm_In3,
                                       //Alarm_In4 = t.Alarm_In4
                                   };
                        if (BuildingId.HasValue && BuildingId.Value > 0)
                            data = data.Where(s => s.BuildingId == BuildingId.Value);
                        if (!string.IsNullOrEmpty(TerminalId))
                            data = data.Where(s => s.TerminalId == TerminalId);
                        WarnOperates = data.ToSmartObservableCollection();
                    }
                }
            }
            catch { }
            #endregion
            #region 新数据库版本
            //var data = from b in multimediaEntities.ClassroomBuilding
            //           join c in multimediaEntities.ClassRoom on b.id equals c.BuildingId
            //           join t in multimediaEntities.TerminalCurrentInfo on c.TerminalId equals t.TerminalID
            //           select new WarnOperate()
            //           {
            //               BuildingId = b.id,
            //               ClassRoomId = c.Id,
            //               TerminalId = t.TerminalID,
            //               TerminalIp = c.TerminalIp,
            //               BuildingName = b.BuildingName,
            //               RoomName = c.RoomName,
            //               Alarm_Control = t.Alarm_Control,
            //               Alarm_In1 = t.Alarm_In1,
            //               Alarm_In2 = t.Alarm_In2,
            //               Alarm_In3 = t.Alarm_In3,
            //               Alarm_In4 = t.Alarm_In4
            //           };
            //if (BuildingId.HasValue && BuildingId.Value > 0)
            //    data = data.Where(s => s.BuildingId == BuildingId.Value);
            //if (!string.IsNullOrEmpty(TerminalId))
            //    data = data.Where(s => s.TerminalId == TerminalId);

            //WarnOperates = data.ToSmartObservableCollection();
            #endregion
        }

        [Command]
        public void Reset()
        {
            TerminalId = null;
            BuildingId = null;
        }

        [Command]
        public void AlarmControl(bool status)
        {
            if (SelectedWarnOperate == null)
                return;
            string url = string.Format("{0}/Alarm_Control={1}", SelectedWarnOperate.TerminalIp, status);
            string response = WebHelper.Get(url);
            WarnOperateQuery();
        }
    }
}
