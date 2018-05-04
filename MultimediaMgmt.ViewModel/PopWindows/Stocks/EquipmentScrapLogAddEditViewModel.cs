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
    public class EquipmentScrapLogAddEditViewModel : BaseViewModel
    {
        public virtual EquipmentScrapLog CurrScrapLog { get; set; }
        public virtual string WindowTitle { get; set; }
        public virtual string ButtonContent { get; set; }

        [Required(ErrorMessage = "设备编码不能为空")]
        public virtual string SerialNumber { get; set; }
        [Required(ErrorMessage = "报废日期不能为空")]
        public virtual DateTime Date { get; set; }


        public Action CloseWindow;
        public Action<string> MessageShow;

        private int currId = 0;
        public EquipmentScrapLogAddEditViewModel(int id)
        {
            currId = id;
            if (id > 0)
            {
                CurrScrapLog = multimediaEntities.EquipmentScrapLog.FirstOrDefault(s => s.ID == id);
                WindowTitle = "报废记录编辑";
                ButtonContent = "更新";
            }
            else
            {
                WindowTitle = "报废记录新增";
                ButtonContent = "增加";
            }
            if (CurrScrapLog == null)
                CurrScrapLog = new EquipmentScrapLog() { Date = DateTime.Now.Date };
            SerialNumber = CurrScrapLog.SerialNumber;
            Date = CurrScrapLog.Date;
        }

        [Command]
        public void Confirm()
        {
            if (string.IsNullOrEmpty(SerialNumber) ||
                Date == default(DateTime))
            {
                MessageShow("请确认必填项");
                return;
            }
            CurrScrapLog.SerialNumber = SerialNumber;
            CurrScrapLog.Date = Date;
            if (currId > 0)
            {
                multimediaEntities.Entry(CurrScrapLog).State = EntityState.Modified;
                multimediaEntities.SaveChanges();
            }
            else
            {
                multimediaEntities.EquipmentScrapLog.Add(CurrScrapLog);
                multimediaEntities.SaveChanges();
            }
            CloseWindow();
        }
    }
}
