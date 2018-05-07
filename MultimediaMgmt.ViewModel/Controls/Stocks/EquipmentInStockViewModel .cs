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
    public class EquipmentInStockViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<EquipmentInStock> EquipmentInStocks { get; set; }
        public virtual EquipmentLoanLogEx SelectedEquipmentInStock { get; set; }
        public virtual string SerialNumber { get; set; }

        public EquipmentInStockViewModel()
        {
        }

        [Command]
        public void Query()
        {
            var data = from b in multimediaEntities.EquipmentInStock
                       select b;
            if (!string.IsNullOrEmpty(SerialNumber))
                data = data.Where(s => s.SerialNumber == SerialNumber);

            EquipmentInStocks = data.ToSmartObservableCollection();
        }
    }
}
