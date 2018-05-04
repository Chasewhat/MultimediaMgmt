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
    public class CentralizedControlMgmtViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<CentralizedControlEntity> CentralizedControls { get; set; }
        public virtual CentralizedControlEntity SelectedCentralizedControl { get; set; }
        public virtual SmartObservableCollection<CentralizedControlEntity> SelectedCentralizedControls { get; set; }
            = new SmartObservableCollection<CentralizedControlEntity>();

        public virtual List<string> TechBuilds { get; set; }
        public virtual string SelectedTechBuild { get; set; }

        public virtual bool AllControlSwitch { get; set; }
        public virtual bool AllAirConditionerSwitch { get; set; }
        public virtual bool AllLightingSwitch { get; set; }

        private List<CentralizedControlEntity> ccs = new List<CentralizedControlEntity>();
        public CentralizedControlMgmtViewModel()
        {
            Random r = new Random();
            for (int i = 0; i < 20; i++)
            {
                ccs.Add(new CentralizedControlEntity()
                {
                    Code = "B00" + i,
                    TechBuild = "教五楼",
                    ControlSwitch = r.Next(0, 3) > 1,
                    AirConditionerSwitch = r.Next(0, 3) > 1,
                    LightingSwitch = r.Next(0, 3) > 1
                });
            }
            CentralizedControls = new SmartObservableCollection<CentralizedControlEntity>(ccs);

            TechBuilds = new List<string>() { "教一楼", "教二楼", "教三楼", "教四楼", "教五楼" };
            AllSwitchSet();
        }

        [Command]
        public void Query()
        {
            CentralizedControls = ccs.Where(s => s.TechBuild == SelectedTechBuild).ToSmartObservableCollection();
            AllSwitchSet();
        }

        [Command]
        public void ControlExec()
        {
            if (SelectedCentralizedControls == null || SelectedCentralizedControls.Count <= 0)
                return;
            CentralizedControls.BeginUpdate();
            foreach (var cc in SelectedCentralizedControls)
            {
                //CentralizedControlEntity temp = CentralizedControls.FirstOrDefault
                cc.ExecResult = "执行成功";
            }
            CentralizedControls.EndUpdate();
        }

        [Command]
        public void SelectedAll(int field)
        {
            CentralizedControls.BeginUpdate();
            foreach (CentralizedControlEntity cc in CentralizedControls)
            {
                if (field == 1)
                {
                    cc.ControlSwitch = true;
                }
                if (field == 2)
                {
                    cc.AirConditionerSwitch = true;
                }
                if (field == 3)
                {
                    cc.LightingSwitch = true;
                }
            }
            CentralizedControls.EndUpdate();
        }

        [Command]
        public void UnSelectedAll(int field)
        {
            CentralizedControls.BeginUpdate();
            foreach (CentralizedControlEntity cc in CentralizedControls)
            {
                if (field == 1)
                {
                    cc.ControlSwitch = false;
                }
                if (field == 2)
                {
                    cc.AirConditionerSwitch = false;
                }
                if (field == 3)
                {
                    cc.LightingSwitch = false;
                }
            }
            CentralizedControls.EndUpdate();
        }

        public void AllSwitchSet()
        {
            bool acSwitch = true, aaSwitch = true, alSwitch = true;
            foreach (CentralizedControlEntity cc in CentralizedControls)
            {
                if (!cc.ControlSwitch)
                    acSwitch = false;
                if (!cc.AirConditionerSwitch)
                    aaSwitch = false;
                if (!cc.LightingSwitch)
                    alSwitch = false;
            }
            AllControlSwitch = acSwitch;
            AllAirConditionerSwitch = aaSwitch;
            AllLightingSwitch = alSwitch;
        }
    }
}
