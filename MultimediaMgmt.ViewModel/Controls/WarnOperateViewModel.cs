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
        public virtual string RoomNum { get; set; }

        public virtual List<KeyValuePair<int, string>> Buildings { get; set; }

        private IRestConnection restConnection = null;
        public Action<string> MessageShow;

        public WarnOperateViewModel()
        {
            Buildings = multimediaEntities.ClassroomBuilding.Select(s => new
            {
                Key = s.Id,
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
            if (!string.IsNullOrEmpty(RoomNum))
                crs = crs.Where(s => s.RoomNum == RoomNum);

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
                                   select new WarnOperate()
                                   {
                                       BuildingId = b.Id,
                                       ClassRoomId = c.Id,
                                       TerminalId = c.TerminalId,
                                       TerminalIp = c.TerminalIp,
                                       BuildingName = b.BuildingName,
                                       RoomNum = c.RoomNum,
                                       Alarm_Control = (t == null ? false : (t.Alarm_Control ?? false)),
                                       Alarm_In1 = (t == null ? false : (t.Alarm_In1 ?? false)),
                                       Alarm_In2 = (t == null ? false : (t.Alarm_In2 ?? false)),
                                       Alarm_In3 = false,
                                       Alarm_In4 = false
                                   };
                        if (BuildingId.HasValue && BuildingId.Value > 0)
                            data = data.Where(s => s.BuildingId == BuildingId.Value);
                        if (!string.IsNullOrEmpty(RoomNum))
                            data = data.Where(s => s.RoomNum == RoomNum);
                        WarnOperates = data.ToSmartObservableCollection();
                    }
                }
            }
            catch(Exception ex) { }
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
            RoomNum = null;
            BuildingId = null;
        }

        [Command]
        public async void AlarmControl(bool status)
        {
            if (SelectedWarnOperate == null)
                return;
            try
            {
                string terminal = SelectedWarnOperate.TerminalId;
                bool result = await GetExec(terminal, status.ToString(), "Alarm_Control");
                WarnOperate wo = WarnOperates.FirstOrDefault(s => s.TerminalId == terminal);
                if (wo != null)
                {
                    WarnOperates.BeginUpdate();
                    wo.ExecStatus = result;
                    wo.ExecResult = string.Format("{0}执行{1}",
                        status ? "布防" : "撤防",
                        result ? "成功" : "失败");
                    WarnOperates.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                MessageShow(ex.Message);
            }
            //WarnOperateQuery();
        }


        [Command]
        public async void AllAlarmControl(bool status)
        {
            if (WarnOperates == null)
                return;
            WarnOperates.BeginUpdate();
            try
            {
                foreach (WarnOperate wo in WarnOperates)
                {
                    bool result = await GetExec(wo.TerminalId, status.ToString(), "Alarm_Control");
                    wo.ExecStatus = result;
                    wo.ExecResult = string.Format("{0}执行{1}",
                        status ? "布防" : "撤防",
                        result ? "成功" : "失败");
                }
            }
            catch (Exception ex)
            {
                MessageShow(ex.Message);
            }
            WarnOperates.EndUpdate();
            //WarnOperateQuery();
        }

        private Task<bool> GetExec(string terminal, string status, string target)
        {
            return Task.Run<bool>(() =>
            {
                try
                {
                    if (restConnection == null)
                        return false;
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("_dc", "1504179824079");
                    parameters.Add("terminalId", terminal);
                    parameters.Add("param", string.Format("{0}={1}", target, status.ToLower()));
                    JObject jo = restConnection.Get("api/TerminalOperate/TerminalSet", parameters);
                    if ((bool)jo["success"])
                        return true;
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}
