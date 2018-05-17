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
                Key = s.id,
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
                       join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.id
                       join t in multimediaEntities.TerminalCurrentInfo on c.TerminalId equals t.TerminalID
                       select new CentralizedControlEx()
                       {
                           Id = c.Id,
                           TerminalId = c.TerminalId,
                           TerminalIp = c.TerminalIp,
                           BuildingId = c.BuildingId,
                           Floor = c.Floor,
                           RoomName = c.RoomName,
                           BuildingName = b.BuildingName,
                           System = t.System,
                           AirConitioner = t.AirConitioner,
                           Lamp = t.Lamp
                       };
            if (BuildingId.HasValue && BuildingId.Value > 0)
                data = data.Where(s => s.BuildingId == BuildingId.Value);
            int floor = 0;
            if(int.TryParse(Floor,out floor))
                data= data.Where(s => s.Floor == floor);
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
        public void CopyIcCardToTerminal()
        {
            if (SelectedCentralizedControls == null || SelectedCentralizedControls.Count <= 0 ||
                restConnection == null)
                return;
            foreach (var cc in SelectedCentralizedControls)
            {
                dynamic dy = new ExpandoObject();
                dy.TerminalId = cc.TerminalId;
                JObject jo = restConnection.Post("api/TerminalOperate/CopyIcCardToTerminal", dy);
                if (jo == null)
                {
                    MessageShowFail("写入未返回结果");
                    return;
                }
                string result = jo["message"].ToString();
                MessageShowSucc(result);
            }
        }
        [Command]
        public void ControlStop()
        {
            TokenSource?.Cancel();
        }

        [Command]
        public void ControlExec()
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
                    if (GetExec(cc.TerminalIp, cc.System.Value, "System"))
                        temp.Append("中控执行成功,");
                    else
                        temp.Append("中控执行失败,");
                }
                if (cc.AirConitioner.HasValue && !TokenSource.IsCancellationRequested)
                {
                    if (GetExec(cc.TerminalIp, cc.AirConitioner.Value, "AirConitioner"))
                        temp.Append("空调执行成功,");
                    else
                        temp.Append("空调执行失败,");
                }
                if (cc.Lamp.HasValue && !TokenSource.IsCancellationRequested)
                {
                    if (GetExec(cc.TerminalIp, cc.Lamp.Value, "Lamp"))
                        temp.Append("照明执行成功,");
                    else
                        temp.Append("照明执行失败,");
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

        private bool GetExec(string ip, bool status, string target)
        {
            try
            {
                string url = string.Format("http://{0}/TERMINAL_STATUS?{1}={2}",
                    ip, target, status);
                string response = WebHelper.Get(url);
                JObject jo = JObject.Parse(response);
                if ((bool)jo["success"])
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
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
