using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaMgmt.Model.Models
{
    public class EquipmentControllersEntity
    {
        //中控 true、false 是否显示九个图标中:在设备监控九个图标中标配显示 是否可操作:Y
        public bool System { get; set; }
        //电视一体机 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool FPD { get; set; }
        //电脑 true、false 是否显示九个图标中:Y 是否可操作:N
        public bool Computer_Status { get; set; }
        //电脑操作 0、1、4 是否显示九个图标中:N 是否可操作:Y
        public int Computer_Control { get; set; }
        //投影机 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Projector { get; set; }
        //幕布 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Projection_Screen { get; set; }
        //窗帘 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Curtain { get; set; }
        //照明 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Lamp { get; set; }
        //音响 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Volume { get; set; }
        //录播 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Record { get; set; }
        //电控柜门 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Lock_Status { get; set; }
        //门禁 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Lock_ACT { get; set; }
        //大屏 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Large_Screen { get; set; }
        //电源 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool AC_Relay1 { get; set; }
        //空调 true、false 是否显示九个图标中:Y 是否可操作:Y
        public bool Air_Conditioner { get; set; }
        //投影机信号源 1到4 是否显示九个图标中:N 是否可操作:Y
        public int Projection_Signal { get; set; }
        //电脑信号源 1到4 是否显示九个图标中:N 是否可操作:Y
        public int Computer_Signal { get; set; }
        //内网口1 true、false 是否显示九个图标中:N 是否可操作:Y
        public bool LAN1 { get; set; }
        //内网口2 true、false 是否显示九个图标中:N 是否可操作:Y
        public bool LAN2 { get; set; }
        //内网口3 true、false 是否显示九个图标中:N 是否可操作:Y
        public bool LAN3 { get; set; }
        //内网口4 true、false 是否显示九个图标中:N 是否可操作:Y
        public bool LAN4 { get; set; }
        //安防报警 true、false 是否显示九个图标中:N 是否可操作:N
        public bool Alarm_In1 { get; set; }
        //报警2 true、false 是否显示九个图标中:N 是否可操作:N
        public bool Alarm_In2 { get; set; }
        //布防 true、false 是否显示九个图标中:N 是否可操作:Y
        public bool Alarm_Control { get; set; }
        //光耦输入1 true、false 是否显示九个图标中:N 是否可操作:N
        public bool IN_STATUS1 { get; set; }
        //光耦输入2 true、false 是否显示九个图标中:N 是否可操作:N
        public bool IN_STATUS2 { get; set; }
        //光耦输入3 true、false 是否显示九个图标中:N 是否可操作:N
        public bool IN_STATUS3 { get; set; }

    }
}
