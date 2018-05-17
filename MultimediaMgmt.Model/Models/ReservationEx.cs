using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaMgmt.Model.Models
{
    public class ReservationEx
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public byte ClassOrd { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public int RoomId { get; set; }
        public string RoomNum { get; set; }
        public string PersonId { get; set; }
        public string CourseName { get; set; }
        public int ClassroomReservationId { get; set; }
        public string TerminalId { get; set; }
        public string Name { get; set; }
    }
}
