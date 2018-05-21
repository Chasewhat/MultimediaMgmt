using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaMgmt.Model.Models
{
    public class ClassroomReservationEx
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string ReservationPersonId { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> ReservationTime { get; set; }
        public Nullable<byte> ReservationState { get; set; }
        public string ReservationPersonName { get; set; }
        public Nullable<byte> ApprovalState { get; set; }
        /// <summary>
        /// 课程日期
        /// </summary>
        public string Date { get; set; }

        public List<ReservationCourseTableEx> Courses { get; set; }
    }
}
