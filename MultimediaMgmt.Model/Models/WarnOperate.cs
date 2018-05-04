using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultimediaMgmt.Model.Models
{
    public class WarnOperate
    {
        public int ClassRoomId { get; set; }
        public int BuildingId { get; set; }
        public string TerminalId { get; set; }
        public string TerminalIp { get; set; }
        public string RoomName { get; set; }
        public string BuildingName { get; set; }
        public bool? Alarm_In1 { get; set; }
        public bool? Alarm_In2 { get; set; }
        public bool? Alarm_In3 { get; set; }
        public bool? Alarm_In4 { get; set; }
        public bool? Alarm_Control { get; set; }
        public DateTime ReportTime { get; set; }
    }
}
