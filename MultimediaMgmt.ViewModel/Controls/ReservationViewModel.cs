using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;
using MultimediaMgmt.Common.Helper;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class ReservationViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<ReservationEx> Reservations { get; set; }
        public virtual ReservationEx SelectedReservation { get; set; }
        public virtual string PersonId { get; set; }
        public virtual DateTime? BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual string RoomNum { get; set; }

        public Action<string> MessageShow;

        public ReservationViewModel()
        {
        }

        [Command]
        public void Query()
        {
            var data = (from b in multimediaEntities.ReservationCourseTable
                       join c in multimediaEntities.ClassRoom on b.RoomId equals c.Id
                       join p in multimediaEntities.IcCard on b.PersonId equals p.PersonId
                       select new ReservationEx(){
                           Id = b.Id,
                           Date = b.Date,
                           ClassOrd = b.ClassOrd,
                           BeginTime=b.BeginTime,
                           EndTime = b.EndTime,
                           RoomId = b.RoomId,
                           RoomNum = c.RoomNum,
                           PersonId = b.PersonId,
                           CourseName = b.CourseName,
                           ClassroomReservationId = b.ClassroomReservationId,
                           TerminalId = c.TerminalId,
                           Name = p.Name
                       }).AsEnumerable();
            if (!string.IsNullOrEmpty(PersonId))
                data = data.Where(s => s.PersonId == PersonId);
            if (!string.IsNullOrEmpty(RoomNum))
                data = data.Where(s => s.RoomNum == RoomNum);
            if (BeginDate.HasValue && BeginDate.Value != default(DateTime))
                data = data.Where(s => s.Date.ToDateTime("yyyy-MM-dd") >= BeginDate);
            if (EndDate.HasValue && EndDate.Value != default(DateTime))
                data = data.Where(s => s.Date.ToDateTime("yyyy-MM-dd") <= EndDate);

            Reservations = data.ToSmartObservableCollection();
        }

        [Command]
        public void Reset()
        {
            RoomNum = PersonId = null;
            BeginDate = EndDate = null;
        }

        [Command]
        public void Delete()
        {
            if (SelectedReservation == null)
                return;
            if (string.Format("{0}",
                SelectedReservation.Date).ToDateTime("yyyy-MM-dd")
                <= DateTime.Now.Date)
            {
                MessageShow("只能删除今日之后的预约记录");
                return;
            }
            ReservationCourseTable res = multimediaEntities.ReservationCourseTable.FirstOrDefault(s => s.Id == SelectedReservation.Id);
            if (res == null)
                return;
            Reservations.Remove(SelectedReservation);
            multimediaEntities.ReservationCourseTable.Remove(res);
            multimediaEntities.SaveChanges();
        }
    }
}
