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
    public class EquipmentLoanLogAddEditViewModel : BaseViewModel
    {
        public virtual EquipmentLoanLog CurrLoanLog { get; set; }
        public virtual string WindowTitle { get; set; }
        public virtual string ButtonContent { get; set; }

        [Required(ErrorMessage = "设备编码不能为空")]
        public virtual string SerialName { get; set; }
        [Required(ErrorMessage = "借出日期不能为空")]
        public virtual DateTime LoanDate { get; set; }
        [Required(ErrorMessage = "借用人不能为空")]
        public virtual string Borrower { get; set; }


        public Action CloseWindow;
        public Action<string> MessageShow;

        private int currId = 0;
        public EquipmentLoanLogAddEditViewModel(int id)
        {
            currId = id;
            if (id > 0)
            {
                CurrLoanLog = multimediaEntities.EquipmentLoanLog.FirstOrDefault(s => s.ID == id);
                WindowTitle = "借出记录编辑";
                ButtonContent = "更新";
            }
            else
            {
                WindowTitle = "借出记录新增";
                ButtonContent = "增加";
            }
            if (CurrLoanLog == null)
                CurrLoanLog = new EquipmentLoanLog() { LoanDate = DateTime.Now.Date };
            SerialName = CurrLoanLog.SerialName;
            LoanDate = CurrLoanLog.LoanDate;
            Borrower = CurrLoanLog.Borrower;
        }

        [Command]
        public void Confirm()
        {
            if (string.IsNullOrEmpty(SerialName) ||
                LoanDate == default(DateTime) ||
                string.IsNullOrEmpty(Borrower))
            {
                MessageShow("请确认必填项");
                return;
            }
            try
            {
                CurrLoanLog.SerialName = SerialName;
                CurrLoanLog.LoanDate = LoanDate;
                CurrLoanLog.Borrower = Borrower;
                if (currId > 0)
                {
                    multimediaEntities.Entry(CurrLoanLog).State = EntityState.Modified;
                    multimediaEntities.SaveChanges();
                }
                else
                {
                    multimediaEntities.Entry(CurrLoanLog).State = EntityState.Added;
                    //multimediaEntities.EquipmentLoanLog.Add(CurrLoanLog);
                    multimediaEntities.SaveChanges();
                }
                MessageShow("保存成功!");
            }
            catch (Exception ex)
            {
                MessageShow(string.Format("保存失败:{0}", ex.Message));
            }
            if (currId > 0)
                CloseWindow();
            else
                CurrLoanLog = new EquipmentLoanLog() { LoanDate = DateTime.Now.Date };
        }
    }
}
