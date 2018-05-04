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
    public class StdCourseTableViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<StdCourseTable> StdCourseTables { get; set; }
        public virtual string PersonId { get; set; }

        public StdCourseTableViewModel()
        {
        }

        [Command]
        public void Query()
        {
            var data = from b in multimediaEntities.StdCourseTable
                       select b;
            if (!string.IsNullOrEmpty(PersonId))
                data = data.Where(s => s.PersonId == PersonId);

            StdCourseTables = data.ToSmartObservableCollection();
        }
    }
}
