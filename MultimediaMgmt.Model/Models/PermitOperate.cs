using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultimediaMgmt.Model.Models
{
    public class PermitOperate
    {
        public int Id { get; set; }
        public int ClassRoomId { get; set; }
        public int BuildingId { get; set; }
        public string TerminalId { get; set; }
        public string RoomName { get; set; }
        public string BuildingName { get; set; }
        public string PersonId { get; set; }
        public string PermitTime { get; set; }
        public string PersonName { get; set; }
    }
}
