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
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Threading;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class CentralizedControlMgmtViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<CentralizedControlEx> CentralizedControls { get; set; }
        public virtual CentralizedControlEx SelectedCentralizedControl { get; set; }
        public virtual SmartObservableCollection<CentralizedControlEx> SelectedCentralizedControls { get; set; }
            = new SmartObservableCollection<CentralizedControlEx>();

        public virtual List<KeyValuePair<int, string>> Buildings { get; set; }
        public virtual int? BuildingId { get; set; }
        public virtual string Floor { get; set; }

        public virtual bool AllControlSwitch { get; set; }
        public virtual bool AllAirConditionerSwitch { get; set; }
        public virtual bool AllLightingSwitch { get; set; }
        //public virtual bool SystemCheck { get; set; }
        //public virtual bool AirConitionerCheck { get; set; }
        //public virtual bool LampCheck { get; set; }
        public Action<string> MessageShowFail;
        public Action<string> MessageShowSucc;

        private IRestConnection restConnection = null;
        private CancellationTokenSource TokenSource;

        public CentralizedControlMgmtViewModel()
        {
            Buildings = multimediaEntities.ClassroomBuilding.Select(s => new
            {
                Key = s.Id,
                Value = s.BuildingName
            }).AsEnumerable().Select(s =>
                            new KeyValuePair<int, string>(s.Key, s.Value)).ToList();

            string url = ConfigHelper.Main.WebUrl;
            if (!string.IsNullOrEmpty(url))
                restConnection = new RestConnection(url);
        }

        [Command]
        public void Query()
        {
            var data = from c in multimediaEntities.ClassRoom
                       join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.Id
                       join t in (from tt in multimediaEntities.TerminalInfo
                                  group tt by new
                                  {
                                      tt.TerminalId
                                  } into g
                                  select g.Where(p => p.LogTime == g.Max(m => m.LogTime)).FirstOrDefault()) on c.TerminalId equals t.TerminalId
                       select new CentralizedControlEx()
                       {
                           Id = c.Id,
                           TerminalId = c.TerminalId,
                           TerminalIp = c.TerminalIp,
                           BuildingId = c.BuildingId,
                           Floor = c.Floor,
                           RoomName = c.RoomNum,
                           BuildingName = b.BuildingName,
                           System = t.System,
                           AirConitioner = null,//t.AirConitioner,
                           Lamp = t.Lamp
                       };
            if (BuildingId.HasValue && BuildingId.Value > 0)
                data = data.Where(s => s.BuildingId == BuildingId.Value);
            int floor = 0;
            if (int.TryParse(Floor, out floor))
                data = data.Where(s => s.Floor == floor);
            CentralizedControls = data.ToSmartObservableCollection();
            AllSwitchSet();
        }

        [Command]
        public void Reset()
        {
            BuildingId = null;
            Floor = null;
        }

        [Command]
        public async void CopyIcCardToTerminal()
        {
            if (SelectedCentralizedControls == null || SelectedCentralizedControls.Count <= 0 ||
                restConnection == null)
                return;
            CentralizedControls.BeginUpdate();
            foreach (var cc in SelectedCentralizedControls)
            {
                if (await CardWrite(cc.TerminalId))
                    cc.ExecResult = "写入执行失败";
                else
                    cc.ExecResult = "写入执行成功";
            }
            CentralizedControls.EndUpdate();
        }

        private Task<bool> CardWrite(string terminal)
        {
            return Task.Run<bool>(() =>
            {
                try
                {
                    dynamic dy = new ExpandoObject();
                    dy.TerminalId = terminal;
                    JObject jo = restConnection.Post("api/TerminalOperate/CopyIcCardToTerminal", dy);
                    if (jo == null || !((bool)jo["success"]))
                        return true;
                    else
                        return false;
                    //string url = string.Format("http://{0}/TERMINAL_STATUS?{1}={2}",
                    //    ip, target, status);
                    //string response = WebHelper.Get(url);
                    //JObject jo = JObject.Parse(response);
                    //if (jo! = null)
                    //    return true;
                    //else
                    //    return false;
                }
                catch
                {
                    return false;
                }
            });
        }
        [Command]
        public void ControlStop()
        {
            TokenSource?.Cancel();
        }

        [Command]
        public async void ControlExec()
        {
            if (SelectedCentralizedControls == null || SelectedCentralizedControls.Count <= 0)
                return;
            TokenSource = new CancellationTokenSource();
            CentralizedControls.BeginUpdate();
            foreach (var cc in SelectedCentralizedControls)
            {
                if (TokenSource.IsCancellationRequested)
                    break;
                StringBuilder temp = new StringBuilder();
                if (cc.System.HasValue && !TokenSource.IsCancellationRequested)
                {
                    temp.AppendFormat("中控{0}", cc.System.Value ? "开" : "关");
                    if (await GetExec(cc.TerminalId, cc.System.Value, "System"))
                        temp.Append("执行成功,");
                    else
                        temp.Append("执行失败,");
                }
                if (cc.AirConitioner.HasValue && !TokenSource.IsCancellationRequested)
                {
                    temp.AppendFormat("空调{0}", cc.System.Value ? "开" : "关");
                    if (await GetExec(cc.TerminalId, cc.AirConitioner.Value, "AirConitioner"))
                        temp.Append("执行成功,");
                    else
                        temp.Append("执行失败,");
                }
                if (cc.Lamp.HasValue && !TokenSource.IsCancellationRequested)
                {
                    temp.AppendFormat("照明{0}", cc.System.Value ? "开" : "关");
                    if (await GetExec(cc.TerminalId, cc.Lamp.Value, "Lamp"))
                        temp.Append("执行成功,");
                    else
                        temp.Append("执行失败,");
                }
                if (temp.Length > 0)
                {
                    temp = temp.Remove(temp.Length - 1, 1);
                    cc.ExecResult = temp.ToString();
                }
            }
            CentralizedControls.EndUpdate();
            TokenSource = null;
        }

        private Task<bool> GetExec(string terminal, bool status, string target)
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
                    parameters.Add("param", string.Format("{0}={1}", target, status));
                    JObject jo = restConnection.Get("api/TerminalOperate/TerminalSet", parameters);
                    if ((bool)jo["success"])
                        return true;
                    else
                        return false;
                    //string url = string.Format("http://{0}/TERMINAL_STATUS?{1}={2}",
                    //    ip, target, status);
                    //string response = WebHelper.Get(url);
                    //JObject jo = JObject.Parse(response);
                    //if (jo! = null)
                    //    return true;
                    //else
                    //    return false;
                }
                catch
                {
                    return false;
                }
            });
        }

        [Command]
        public void SelectedAll(int field)
        {
            CentralizedControls.BeginUpdate();
            foreach (CentralizedControlEx cc in CentralizedControls)
            {
                if (field == 1)
                {
                    cc.System = true;
                }
                if (field == 2)
                {
                    cc.AirConitioner = true;
                }
                if (field == 3)
                {
                    cc.Lamp = true;
                }
            }
            CentralizedControls.EndUpdate();
        }

        [Command]
        public void UnSelectedAll(int field)
        {
            CentralizedControls.BeginUpdate();
            foreach (CentralizedControlEx cc in CentralizedControls)
            {
                if (field == 1)
                {
                    cc.System = false;
                }
                if (field == 2)
                {
                    cc.AirConitioner = false;
                }
                if (field == 3)
                {
                    cc.Lamp = false;
                }
            }
            CentralizedControls.EndUpdate();
        }

        public void AllSwitchSet()
        {
            bool acSwitch = true, aaSwitch = true, alSwitch = true;
            foreach (CentralizedControlEx cc in CentralizedControls)
            {
                if (!cc.System.HasValue || !cc.System.Value)
                    acSwitch = false;
                if (!cc.AirConitioner.HasValue || !cc.AirConitioner.Value)
                    aaSwitch = false;
                if (!cc.Lamp.HasValue || !cc.Lamp.Value)
                    alSwitch = false;
            }
            AllControlSwitch = acSwitch;
            AllAirConditionerSwitch = aaSwitch;
            AllLightingSwitch = alSwitch;
        }
    }
}
