using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model;
using MultimediaMgmt.Common.Helper;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MultimediaMgmt.ViewModel.PopWindows
{
    [POCOViewModel]
    public class EquipmentTransferLogAddEditViewModel : BaseViewModel
    {
        public virtual EquipmentTransferLog CurrTransferLog { get; set; }
        public virtual string WindowTitle { get; set; }
        public virtual string ButtonContent { get; set; }

        [Required(ErrorMessage = "设备编码不能为空")]
        public virtual string SerialNumber { get; set; }
        [Required(ErrorMessage = "转移日期不能为空")]
        public virtual DateTime TransferDate { get; set; }


        public Action CloseWindow;
        public Action<string> MessageShow;

        private int currId = 0;
        public EquipmentTransferLogAddEditViewModel(int id)
        {
            currId = id;
            if (id > 0)
            {
                CurrTransferLog = multimediaEntities.EquipmentTransferLog.FirstOrDefault(s => s.ID == id);
                WindowTitle = "转移记录编辑";
                ButtonContent = "更新";
            }
            else
            {
                WindowTitle = "转移记录新增";
                ButtonContent = "增加";
            }
            if (CurrTransferLog == null)
                CurrTransferLog = new EquipmentTransferLog() { TransferDate = DateTime.Now.Date };
            SerialNumber = CurrTransferLog.SerialNumber;
            TransferDate = CurrTransferLog.TransferDate;
        }

        [Command]
        public void Confirm()
        {
            if (string.IsNullOrEmpty(SerialNumber) ||
                TransferDate == default(DateTime))
            {
                MessageShow("请确认必填项");
                return;
            }
            CurrTransferLog.SerialNumber = SerialNumber;
            CurrTransferLog.TransferDate = TransferDate;
            if (currId > 0)
            {
                multimediaEntities.Entry(CurrTransferLog).State = EntityState.Modified;
                multimediaEntities.SaveChanges();
            }
            else
            {
                multimediaEntities.EquipmentTransferLog.Add(CurrTransferLog);
                multimediaEntities.SaveChanges();
            }
            CloseWindow();
        }
    }
}
