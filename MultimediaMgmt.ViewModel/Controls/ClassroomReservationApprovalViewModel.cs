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
using System.Windows;
using System.Threading;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class ClassroomReservationApprovalViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<ClassroomReservationEx> ClassroomReservationExs { get; set; }
        public virtual ClassroomReservationEx SelectedClassroomReservationEx { get; set; }

        public virtual Dictionary<int, string> DateItems { get; set; }
        public virtual Dictionary<int, string> RoomItems { get; set; }
        public virtual Dictionary<byte, string> ReserveState { get; set; }
        public virtual Dictionary<byte, string> ApproveState { get; set; }
        public virtual int? SelectedDateItem { get; set; }
        public virtual int? SelectedRoomItem { get; set; }
        public virtual byte? SelectedReserveState { get; set; }
        public virtual byte? SelectedApproveState { get; set; }
        public virtual DateTime? BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

        public Func<string, MessageBoxResult> MessageShow;

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

            if (SelectedRoomItem.HasValue)
                data = data.Where(s => s.RoomId == RoomId);
            if (SelectedReserveState.HasValue)
                data = data.Where(s => s.ReservationState == SelectedReserveState);
            if (SelectedApproveState.HasValue)
                data = data.Where(s => s.ApprovalState == SelectedApproveState);

            ClassroomReservationExs = data.ToSmartObservableCollection();
        }

        [Command]
        public void Reset()
        {
            SelectedDateItem = SelectedRoomItem =
                SelectedReserveState = SelectedApproveState = null;
            BeginDate = EndDate = null;
        }

        [Command]
        public void Delete()
        {
            if (SelectedClassroomReservationEx == null)
                return;
            int rid = SelectedClassroomReservationEx.Id;
            //删除预约明细
            ClassroomReservation reser = multimediaEntities.ClassroomReservation.FirstOrDefault(s => s.Id == rid);
            multimediaEntities.ClassroomReservation.Attach(reser);
            multimediaEntities.ClassroomReservation.Remove(reser);
            //List<ReservationCourseTable> course = multimediaEntities.ReservationCourseTable.Where(s => s.ClassroomReservationId == rid).ToList();
            //foreach (ReservationCourseTable c in course)
            //    multimediaEntities.ReservationCourseTable.Remove(c);
            //ClassroomReservationApproval appro = multimediaEntities.ClassroomReservationApproval.FirstOrDefault(s => s.ClassroomReservationId == rid);
            //multimediaEntities.ClassroomReservationApproval.Remove(appro);
            multimediaEntities.SaveChanges();
            Query();
        }

        [Command]
        public void Approve()
        {
            if (SelectedClassroomReservationEx == null
                || SelectedClassroomReservationEx.ApprovalState != 0)
                return;
            if (MessageShow(string.Format("确认批准{0}的预约申请?", SelectedClassroomReservationEx.ReservationPersonName))
                != MessageBoxResult.Yes)
                return;
            ClassroomReservation reser = multimediaEntities.ClassroomReservation.FirstOrDefault(s => s.Id == SelectedClassroomReservationEx.Id);
            if (Approve(ref reser))
            {
                foreach (ReservationCourseTable courseTable in reser.ReservationCourseTable)
                {
                    courseTable.ClassroomReservation = null;
                }
                foreach (ClassroomReservationApproval approval in reser.ClassroomReservationApproval)
                {
                    approval.ClassroomReservation = null;
                }
            }
            Query();
        }

        [Command]
        public void Reject()
        {
            if (SelectedClassroomReservationEx == null
                || SelectedClassroomReservationEx.ApprovalState != 0)
                return;
            if (MessageShow(string.Format("确认拒绝{0}的预约申请?", SelectedClassroomReservationEx.ReservationPersonName))
                != MessageBoxResult.Yes)
                return;
            ClassroomReservation reser = multimediaEntities.ClassroomReservation.FirstOrDefault(s => s.Id == SelectedClassroomReservationEx.Id);
            if (DisApprove(ref reser))
            {
                foreach (ReservationCourseTable courseTable in reser.ReservationCourseTable)
                {
                    courseTable.ClassroomReservation = null;
                }
                foreach (ClassroomReservationApproval approval in reser.ClassroomReservationApproval)
                {
                    approval.ClassroomReservation = null;
                }
            }
            Query();
        }


        public bool Approve(ref ClassroomReservation reservation)
        {
            int Id = reservation.Id;
            try
            {
                ClassroomReservation updateReservation = multimediaEntities.ClassroomReservation.Where(e => e.Id == Id).FirstOrDefault();
                ClassroomReservationApproval approval = updateReservation.ClassroomReservationApproval.Where(p => p.ApprovalLevel == 1).FirstOrDefault();
                if (null == approval)
                {
                    approval = new ClassroomReservationApproval()
                    {
                        ApprovalLevel = 1,
                        ClassroomReservation = updateReservation,
                    };
                    updateReservation.ClassroomReservationApproval.Add(approval);
                }
                approval.ApprovalState = 1;
                approval.ApprovalPersonId = Constants.CurrUser.LoginName;
                approval.ApprovalTime = DateTime.Now;
                approval.Description = "";
                if (null != reservation.ClassroomReservationApproval.Where(p => p.ApprovalLevel == 1).FirstOrDefault())
                {
                    approval.Description = reservation.ClassroomReservationApproval.Where(p => p.ApprovalLevel == 1).FirstOrDefault().Description;
                }
                multimediaEntities.SaveChanges();
                reservation = updateReservation;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DisApprove(ref ClassroomReservation reservation)
        {
            int Id = reservation.Id;
            try
            {
                ClassroomReservation updateReservation = multimediaEntities.ClassroomReservation.Where(e => e.Id == Id).FirstOrDefault();
                ClassroomReservationApproval approval = updateReservation.ClassroomReservationApproval.Where(p => p.ApprovalLevel == 1).FirstOrDefault();
                if (null == approval)
                {
                    approval = new ClassroomReservationApproval()
                    {
                        ApprovalLevel = 1,
                        ClassroomReservation = updateReservation,
                    };
                    updateReservation.ClassroomReservationApproval.Add(approval);
                }
                approval.ApprovalState = 2;
                approval.ApprovalPersonId = Constants.CurrUser.LoginName;
                approval.ApprovalTime = DateTime.Now;
                approval.Description = "";
                if (null != reservation.ClassroomReservationApproval.Where(p => p.ApprovalLevel == 1).FirstOrDefault())
                {
                    approval.Description = reservation.ClassroomReservationApproval.Where(p => p.ApprovalLevel == 1).FirstOrDefault().Description;
                }
                multimediaEntities.SaveChanges();
                reservation = updateReservation;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
