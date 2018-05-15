using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultimediaMgmt.Model.Models
{
    public class CentralizedControlEx
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public int Floor { get; set; }
        public string TerminalId { get; set; }
        public string TerminalIp { get; set; }
        public string RoomName { get; set; }
        public string BuildingName { get; set; }
        public bool? System { get; set; }
        public bool? AirConitioner { get; set; }
        public bool? Lamp { get; set; }
        public string ExecResult { get; set; }
    }
}
