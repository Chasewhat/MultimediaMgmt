using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultimediaMgmt.Model.Models
{
    public class ClassRoomEx
    {
        public int Id { get; set; }
        public string TerminalId { get; set; }
        public string TerminalIp { get; set; }
        public string RoomName { get; set; }
        public int Floor { get; set; }
        public int BuildingId { get; set; }
        public string VedioAddress { get; set; }
        public string BuildingName { get; set; }
        public string PersonId { get; set; }
        public string CourseName { get; set; }
        public string PersonName { get; set; }
        public string ClassName { get; set; }
        public int StudentSum { get; set; }
        public int RealStudentSum { get; set; }
        public string Location { get; set; }
        public bool? System { get; set; }
        public bool? FPD { get; set; }
        public bool? ComputerStatus { get; set; }
        public bool? Projector { get; set; }
        public bool? ProjectorScreen { get; set; }
        public bool? Curtain { get; set; }
        public bool? Lamp { get; set; }
        public bool? Volume { get; set; }
        public bool? Record { get; set; }
        public bool? Lock_Status { get; set; }
        public bool? Door_Status { get; set; }
        public bool? ACRelay1 { get; set; }
        public bool? Large_Screen { get; set; }
        public bool? AirConitioner { get; set; }
        public bool? ProjectionSignal { get; set; }
        public bool? ComputerSignal { get; set; }
        public bool? LAN1 { get; set; }
        public bool? LAN2 { get; set; }
        public bool? LAN3 { get; set; }
        public bool? LAN4 { get; set; }
        public bool? Alarm_In1 { get; set; }
        public bool? Alarm_In2 { get; set; }
        public bool? Alarm_In3 { get; set; }
        public bool? Alarm_In4 { get; set; }
        public bool? Alarm_Control { get; set; }
        public bool? IN_STATUS1 { get; set; }
        public bool? IN_STATUS2 { get; set; }
        public bool? IN_STATUS3 { get; set; }
        public string HexCode { get; set; }
        public byte? Opereate_Last { get; set; }
    }
}
