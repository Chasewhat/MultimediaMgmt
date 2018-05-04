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
    public class EquipmentTransferLogViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<EquipmentTransferLogEx> EquipmentTransferLogs { get; set; }
        public virtual EquipmentTransferLogEx SelectedEquipmentTransferLog { get; set; }
        public virtual string SerialNumber { get; set; }

        public EquipmentTransferLogViewModel()
        {
        }

        [Command]
        public void Query()
        {
            var data = from b in multimediaEntities.EquipmentTransferLog
                       join s in multimediaEntities.EquipmentInStock on b.SerialNumber equals s.SerialNumber
                       select new EquipmentTransferLogEx()
                       {
                           ID = b.ID,
                           SerialNumber = b.SerialNumber,
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
                           TransferDate = b.TransferDate,
                           Project = b.Project,
                           Department = b.Department,
                           Location = b.Location,
                           Principal = b.Principal,
                           Remarks = s.Remarks
                       };
            if (!string.IsNullOrEmpty(SerialNumber))
                data = data.Where(s => s.SerialNumber == SerialNumber);

            EquipmentTransferLogs = data.ToSmartObservableCollection();
        }
    }
}
