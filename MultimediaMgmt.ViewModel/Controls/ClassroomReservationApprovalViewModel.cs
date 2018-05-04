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
        public virtual SmartObservableCollection<ClassroomReservationApproval> ClassroomReservationApprovals { get; set; }
        public virtual ClassroomReservationApproval SelectedClassroomReservationApproval { get; set; }
        public virtual string PersonId { get; set; }
        public virtual DateTime BeginDate { get; set; }
        public virtual DateTime EndDate { get; set; }

        public ClassroomReservationApprovalViewModel()
        {
            BeginDate = EndDate = DateTime.Now.Date;
        }

        [Command]
        public void Query()
        {
            var data = from b in multimediaEntities.ClassroomReservationApproval
                       select b;
            if (!string.IsNullOrEmpty(PersonId))
                data = data.Where(s => s.ApprovalPersonId == PersonId);

            data = data.Where(s => s.ApprovalTime >= BeginDate && s.ApprovalTime <= EndDate);

            ClassroomReservationApprovals = data.ToSmartObservableCollection();
        }

        public void Edit()
        {
            if (SelectedClassroomReservationApproval == null)
                return;
            multimediaEntities.Entry(SelectedClassroomReservationApproval).State = System.Data.Entity.EntityState.Modified;
            multimediaEntities.SaveChanges();
        }
    }
}
