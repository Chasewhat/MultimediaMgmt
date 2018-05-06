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
    public class ClassroomReservationApprovalViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<ClassroomReservationEx> ClassroomReservationExs { get; set; }
        public virtual ClassroomReservationEx SelectedClassroomReservationEx { get; set; }

        public virtual  Dictionary<int, string> DateItems { get; set; }
        public virtual Dictionary<int, string> RoomItems { get; set; }
        public virtual Dictionary<byte, string> ReserveState { get; set; }
        public virtual Dictionary<byte, string> ApproveState { get; set; }
        public virtual int? SelectedDateItem { get; set; }
        public virtual int? SelectedRoomItem { get; set; }
        public virtual int? SelectedReserveState { get; set; }
        public virtual int? SelectedApproveState { get; set; }
        public virtual DateTime? BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

        public int RoomId = 0;

        public ClassroomReservationApprovalViewModel()
        {
            BeginDate = EndDate = DateTime.Now.Date;
            DateItems = Constants.DateItems;
            RoomItems = Constants.RoomItems;
            ReserveState = Constants.ReserveState;
            ApproveState = Constants.ApproveState;
            
        }

        [Command]
        public void Query()
        {
            var data = (from r in multimediaEntities.ClassroomReservation
                       join a in multimediaEntities.ClassroomReservationApproval on r.Id equals a.ClassroomReservationId
                       join s in multimediaEntities.ReservationCourseTable on r.Id equals s.ClassroomReservationId
                       select new ClassroomReservationEx()
                       {
                           Id = r.Id,
                           RoomId = s.RoomId,
                           ReservationPersonId = r.ReservationPersonId,
                           Description = r.Description,
                           ReservationTime = r.ReservationTime,
                           ReservationState = r.ReservationState,
                           ReservationPersonName = r.ReservationPersonName,
                           ApprovalState = a.ApprovalState,
                           Date = s.Date
                       }).AsEnumerable();
            if (SelectedDateItem.HasValue && SelectedDateItem.Value == 1)
            {
                if (BeginDate.HasValue && BeginDate.Value != default(DateTime))
                    data = data.Where(s => s.ReservationTime >= BeginDate.Value);
                if (EndDate.HasValue && EndDate.Value != default(DateTime))
                    data = data.Where(s => s.ReservationTime <= EndDate.Value);
            }
            if (SelectedDateItem.HasValue && SelectedDateItem.Value == 2)
            {
                if (BeginDate.HasValue && BeginDate.Value != default(DateTime))
                    data = data.Where(s => s.Date.ToDateTime("yyyy-MM-dd") >= BeginDate.Value);
                if (EndDate.HasValue && EndDate.Value != default(DateTime))
                    data = data.Where(s => s.Date.ToDateTime("yyyy-MM-dd") <= EndDate.Value);
            }

            if (SelectedRoomItem.HasValue && SelectedRoomItem.Value != 0)
                data = data.Where(s => s.RoomId == RoomId);
            if (SelectedReserveState.HasValue)
                data = data.Where(s => s.ReservationState == SelectedReserveState);
            if (SelectedApproveState.HasValue)
                data = data.Where(s => s.ApprovalState == SelectedApproveState);

            ClassroomReservationExs = data.ToSmartObservableCollection();
        }

        [Command]
        public void Delete()
        {
            if (SelectedClassroomReservationEx == null)
                return;
            int rid = SelectedClassroomReservationEx.Id;
            //删除预约明细
            List<ReservationCourseTable> course = multimediaEntities.ReservationCourseTable.Where(s => s.ClassroomReservationId == rid).ToList();
            foreach (ReservationCourseTable c in course)
                multimediaEntities.ReservationCourseTable.Remove(c);
            ClassroomReservationApproval appro = multimediaEntities.ClassroomReservationApproval.FirstOrDefault(s => s.ClassroomReservationId == rid);
            multimediaEntities.ClassroomReservationApproval.Remove(appro);
            ClassroomReservation reser = multimediaEntities.ClassroomReservation.FirstOrDefault(s => s.Id == rid);
            multimediaEntities.ClassroomReservation.Remove(reser);
            multimediaEntities.SaveChanges();
        }

        [Command]
        public void Approve()
        {
            if (SelectedClassroomReservationEx == null)
                return;
            ClassroomReservationApproval appro = multimediaEntities.ClassroomReservationApproval.FirstOrDefault(s => s.ClassroomReservationId == SelectedClassroomReservationEx.Id);
            appro.ApprovalLevel += 1;
            appro.ApprovalState = 2;
            appro.ApprovalTime = DateTime.Now;
            appro.ApprovalPersonId = Constants.CurrUser.PersonId;
            multimediaEntities.Entry(appro).State = System.Data.Entity.EntityState.Modified;
            multimediaEntities.SaveChanges();
        }

        [Command]
        public void Reject()
        {
            if (SelectedClassroomReservationEx == null)
                return;
            ClassroomReservationApproval appro = multimediaEntities.ClassroomReservationApproval.FirstOrDefault(s => s.ClassroomReservationId == SelectedClassroomReservationEx.Id);
            appro.ApprovalState = 3;
            appro.ApprovalTime = DateTime.Now;
            appro.ApprovalPersonId = Constants.CurrUser.PersonId;
            multimediaEntities.Entry(appro).State = System.Data.Entity.EntityState.Modified;
            multimediaEntities.SaveChanges();
        }
    }
}
