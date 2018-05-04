using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaMgmt.Model.Models
{
    public class EquipmentControllersShowEntity
    {
        //终端ID
        public string TerminalID { get; set; }
        //电视一体机 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool FPDShow { get; set; }
        //电脑 true、false 是否显示九个图标中:Y 是否可操作:N
        public bool Computer_StatusShow { get; set; }
        //电脑操作 0、1、4 是否显示九个图标中:N 是否可操作:Y
        public int Computer_ControlShow { get; set; }
        //投影机 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool ProjectorShow { get; set; }
        //幕布 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Projection_ScreenShow { get; set; }
        //窗帘 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool CurtainShow { get; set; }
        //照明 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool LampShow { get; set; }
        //音响 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool VolumeShow { get; set; }
        //录播 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool RecordShow { get; set; }
        //电控柜门 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Lock_StatusShow { get; set; }
        //门禁 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Lock_ACTShow { get; set; }
        //大屏 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Large_ScreenShow { get; set; }
        //电源 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool AC_Relay1Show { get; set; }
        //空调 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Air_ConditionerShow { get; set; }

    }
}
