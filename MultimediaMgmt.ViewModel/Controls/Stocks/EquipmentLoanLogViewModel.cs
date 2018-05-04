using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Common.Helper;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class EquipmentLoanLogViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<EquipmentLoanLogEx> EquipmentLoanLogs { get; set; }
        public virtual EquipmentLoanLogEx SelectedEquipmentLoanLog { get; set; }
        public virtual string SerialNumber { get; set; }

        public EquipmentLoanLogViewModel()
        {
        }

        [Command]
        public void Query()
        {
            var data = from b in multimediaEntities.EquipmentLoanLog
                       join s in multimediaEntities.EquipmentInStock on b.SerialName equals s.SerialNumber
                       select new EquipmentLoanLogEx()
                       {
                           ID = b.ID,
                           SerialNumber = b.SerialName,
                           Name = s.Name,
                           Manufacturer = s.Manufacturer,
                           SaleCompany = s.SaleCompany,
                           Type = s.Type,
                           Configuration = s.Configuration,
                           ProduceDate = s.ProduceDate,
                           UserDepartment = s.UserDepartment,
                           Place = s.Place,
                           Keeper = s.Keeper,
                           Price = s.Price,
                           IncreaseType = s.IncreaseType,
                           OriginalPrice = s.OriginalPrice,
                           Intime = s.Intime,
                           UseDate = s.UseDate,
                           LoanDate = b.LoanDate,
                           PredictReturnDate = b.PredictReturnDate,
                           RealityReturnDate = b.RealityReturnDate,
                           Borrower = b.Borrower,
                           Department = b.Department,
                           Authorize = b.Authorize,
                           Remarks = b.Remarks
                       };
            if (!string.IsNullOrEmpty(SerialNumber))
                data = data.Where(s => s.SerialNumber == SerialNumber);

            EquipmentLoanLogs = data.ToSmartObservableCollection();
        }
    }
}
