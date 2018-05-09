using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaMgmt.Model.Models
{
    public class WebTerminalInfo
    {
        public int Id { get; set; }
        public string TerminalId { get; set; }
        public bool? AC_Relay1 { get; set; }
        public bool? Lock_Status { get; set; }
        public byte? Lock_ACT { get; set; }
        public bool? Computer_Status { get; set; }
        public byte? Computer_Control { get; set; }
        public bool? System { get; set; }
        public bool? Projector { get; set; }
        public bool? Projection_Screen { get; set; }
        public byte? Projection_Signal { get; set; }
        public byte? Computer_Signal { get; set; }
        public byte? Volume { get; set; }
        public bool? Volume_Mute { get; set; }
        public bool? DC_Relay2 { get; set; }
        public bool? OC1 { get; set; }
        public bool? OC2 { get; set; }
        public byte? Opereate_Last { get; set; }
        public bool? LAN1 { get; set; }
        public bool? LAN2 { get; set; }
        public bool? LAN3 { get; set; }
        public bool? LAN4 { get; set; }
        public bool? Alarm_Control { get; set; }
        public bool? Alarm_In1 { get; set; }
        public bool? Alarm_In2 { get; set; }
        //public DateTime? LogTime { get; set; }
        public bool IsConnected { get; set; }
    }
}
