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
using MultimediaMgmt.ViewModel.Notice;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class EquipmentRepairLogViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<EquipmentRepairLog> EquipmentRepairLogs { get; set; }
        public virtual EquipmentRepairLog SelectedEquipmentRepairLog { get; set; }
        public virtual string SerialNumber { get; set; }

        public EquipmentRepairLogViewModel()
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
            var data = from b in multimediaEntities.EquipmentRepairLog
                       select b;
            if (!string.IsNullOrEmpty(SerialNumber))
                data = data.Where(s => s.SerialNumber == SerialNumber);

            EquipmentRepairLogs = data.ToSmartObservableCollection();
        }

        [Command]
        public void Reset()
        {
            SerialNumber = null;
        }
    }
}
