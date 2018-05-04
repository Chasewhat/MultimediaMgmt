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
    public class SurveillanceLogViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<SurveillanceLog> SurveillanceLogs { get; set; }

        public virtual SurveillanceLog SelectedSurveillanceLog { get; set; }
        public virtual DateTime? BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

        public SurveillanceLogViewModel()
        {
            BeginDate = EndDate = DateTime.Now.Date;
        }

        [Command]
        public void Query()
        {
            var data = from b in multimediaEntities.SurveillanceLog
                       select b;
            if (BeginDate != null && BeginDate.Value != default(DateTime))
                data = data.Where(s => s.LogDate >= BeginDate);
            if (EndDate != null && EndDate.Value != default(DateTime))
                data = data.Where(s => s.LogDate <= EndDate);

            SurveillanceLogs = data.ToSmartObservableCollection();
        }
    }
}
