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
        public virtual SmartObservableCollection<ClassroomReservationEx> Reservations { get; set; }
        public virtual ClassroomReservationEx SelectedReservation { get; set; }
        public virtual string PersonId { get; set; }
        public virtual DateTime? BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual string RoomNum { get; set; }
        public virtual Dictionary<byte, string> ReserveState { get; set; }

        public Action<string> MessageShow;

        public ReservationViewModel()
        {
            ReserveState = Constants.ReserveState;
        }

        [Command]
        public void Query()
        {
            int tempRoom = 0;
            if (!string.IsNullOrEmpty(RoomNum))
            {
                ClassRoom room = multimediaEntities.ClassRoom.FirstOrDefault(s => s.RoomNum == RoomNum);
                if (room != null)
                    tempRoom = room.Id;
            }
            var data = (from r in multimediaEntities.ClassroomReservation
                            //join s in multimediaEntities.ReservationCourseTable on r.Id equals s.ClassroomReservationId
                        join a in multimediaEntities.ClassroomReservationApproval on r.Id equals a.ClassroomReservationId
                        where a.ApprovalState == 1
                        select new ClassroomReservationEx()
                        {
                            Id = r.Id,
                            //RoomId = s.RoomId,
                            ReservationPersonId = r.ReservationPersonId,
                            Description = r.Description,
                            ReservationTime = r.ReservationTime,
                            ReservationState = r.ReservationState,
                            ReservationPersonName = r.ReservationPersonName + "(" + r.ReservationPersonId + ")",
                            ApprovalState = a.ApprovalState,
                            //Date = s.Date,
                            Courses = (from c in r.ReservationCourseTable
                                       join m in multimediaEntities.ClassRoom on c.RoomId equals m.Id into tempc
                                       from o in tempc.DefaultIfEmpty()
                                       join d in multimediaEntities.IcCard on c.PersonId equals d.PersonId into tempp
                                       from p in tempp.DefaultIfEmpty().Take(1)
                                       where tempRoom == 0 || c.RoomId == tempRoom
                                       select new ReservationCourseTableEx()
                                       {
                                           Id = c.Id,
                                           Date = c.Date,
                                           BeginTime = c.BeginTime,
                                           EndTime = c.EndTime,
                                           ClassOrd = c.ClassOrd,
                                           Name = (p == null ? "" : p.Name) + "(" + c.PersonId + ")",
                                           CourseName = c.CourseName,
                                           RoomNum = (o == null ? c.RoomId + "" : o.RoomNum),
                                           RoomId = c.RoomId
                                       }).ToList()
                        }).AsEnumerable().Where(s => s.Courses.Count > 0).OrderByDescending(s=>s.ReservationTime).AsEnumerable();
            if (!string.IsNullOrEmpty(PersonId))
                data = data.Where(s => s.ReservationPersonId == PersonId);
            //if (!string.IsNullOrEmpty(RoomNum))
            //    data = data.Where(s => s.RoomNum == RoomNum);
            if (BeginDate.HasValue && BeginDate.Value != default(DateTime))
                data = data.Where(s => s.ReservationTime >= BeginDate);
            if (EndDate.HasValue && EndDate.Value != default(DateTime))
                data = data.Where(s => s.ReservationTime <= EndDate);

            Reservations = data.ToSmartObservableCollection();
            //var data = (from b in multimediaEntities.ReservationCourseTable
            //            join a in multimediaEntities.ClassroomReservationApproval on b.ClassroomReservationId equals a.ClassroomReservationId
            //            join r in multimediaEntities.ClassroomReservation on b.ClassroomReservationId equals r.Id
            //            join c in multimediaEntities.ClassRoom on b.RoomId equals c.Id
            //            join p in multimediaEntities.IcCard on b.PersonId equals p.PersonId
            //            where a.ApprovalState == 1
            //            select new ReservationEx()
            //            {
            //                Id = b.Id,
            //                Date = b.Date,
            //                ClassOrd = b.ClassOrd,
            //                BeginTime = b.BeginTime,
            //                EndTime = b.EndTime,
            //                RoomId = b.RoomId,
            //                RoomNum = c.RoomNum,
            //                PersonId = b.PersonId,
            //                CourseName = b.CourseName,
            //                ClassroomReservationId = b.ClassroomReservationId,
            //                TerminalId = c.TerminalId,
            //                Name = p.Name
            //            }).AsEnumerable();
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
            if (!SelectedReservation.ReservationTime.HasValue ||
                SelectedReservation.ReservationTime.Value.Date <= DateTime.Now.Date)
            {
                MessageShow("只能删除今日之后的预约记录");
                return;
            }
            int rid = SelectedReservation.Id;
            //删除预约明细
            ClassroomReservation reser = multimediaEntities.ClassroomReservation.FirstOrDefault(s => s.Id == rid);
            multimediaEntities.ClassroomReservation.Attach(reser);
            multimediaEntities.ClassroomReservation.Remove(reser);
            multimediaEntities.SaveChanges();
            MessageShow("删除成功");
            Query();
        }
    }
}
