using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class EquipmentMgmtViewModel : BaseViewModel
    {
        public virtual object Equipments { get; set; }
        public EquipmentMgmtViewModel()
        {
            Equipments = multimediaEntities.ClassRoom.ToSmartObservableCollection();
        }
    }
}
