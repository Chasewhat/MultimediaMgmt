using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Common.Helper;
using MultimediaMgmt.ViewModel.Notice;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class EquipmentScrapLogViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<EquipmentScrapLogEx> EquipmentScrapLogs { get; set; }
        public virtual EquipmentScrapLogEx SelectedEquipmentScrapLog { get; set; }
        public virtual string SerialNumber { get; set; }

        public EquipmentScrapLogViewModel()
        {
            //订阅设备状态查询事件
            NOTICE.GetEvent<EquipmentStatusInquiryEvent>().Subscribe(QueryStatus);
        }

        public void QueryStatus(string number)
        {
            SerialNumber = number;
            Query();
        }

        [Command]
        public void Query()
        {
            var data = from b in multimediaEntities.EquipmentScrapLog
                       join s in multimediaEntities.EquipmentInStock on b.SerialNumber equals s.SerialNumber
                       select new EquipmentScrapLogEx()
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
                           Date = b.Date,
                           Declarant = b.Declarant,
                           Remarks = s.Remarks
                       };
            if (!string.IsNullOrEmpty(SerialNumber))
                data = data.Where(s => s.SerialNumber == SerialNumber);

            EquipmentScrapLogs = data.ToSmartObservableCollection();
        }

        [Command]
        public void Reset()
        {
            SerialNumber = null;
        }
    }
}
