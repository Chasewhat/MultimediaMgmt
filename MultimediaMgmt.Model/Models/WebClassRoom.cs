using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaMgmt.Model.Models
{
    public class WebClassRoom
    {
        public int Id { get; set; }
        public string RoomNum { get; set; }
        public string TerminalId { get; set; }
        public string TerminalIp { get; set; }
        public string VedioAddress { get; set; }
        public string IdentifyMode { get; set; }
        public ClassroomBuilding ClassroomBuilding { set; get; }
    }
}
