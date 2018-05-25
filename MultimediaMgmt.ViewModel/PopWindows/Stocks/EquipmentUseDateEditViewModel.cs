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
    public class EquipmentUseDateEditViewModel : BaseViewModel
    {
        public virtual EquipmentInStock CurrSerial { get; set; }
        public virtual string WindowTitle { get; set; }
        public virtual string ButtonContent { get; set; }

        public virtual string SerialNumber { get; set; }
        [Required(ErrorMessage = "启用日期不能为空")]
        public virtual DateTime? UseDate { get; set; }


        public Action CloseWindow;
        public Action<string> MessageShow;

        private int currId = 0;
        public EquipmentUseDateEditViewModel(int id)
        {
            currId = id;
            if (id > 0)
            {
                CurrSerial = multimediaEntities.EquipmentInStock.FirstOrDefault(s => s.ID == id);
                WindowTitle = "入库记录编辑";
                ButtonContent = "更新";
            }
            else
            {
                WindowTitle = "入库记录新增";
                ButtonContent = "增加";
            }
            if (CurrSerial == null)
                CurrSerial = new EquipmentInStock() { UseDate = DateTime.Now.Date };
            SerialNumber = CurrSerial.SerialNumber;
            UseDate = CurrSerial.UseDate;
        }

        [Command]
        public void Confirm()
        {
            if (string.IsNullOrEmpty(SerialNumber) ||
                !UseDate.HasValue || UseDate.Value == default(DateTime))
            {
                MessageShow("请确认必填项");
                return;
            }
            try
            {
                CurrSerial.SerialNumber = SerialNumber;
                CurrSerial.UseDate = UseDate;
                if (currId > 0)
                {
                    multimediaEntities.Entry(CurrSerial).State = EntityState.Modified;
                    multimediaEntities.SaveChanges();
                }
                else
                {
                    multimediaEntities.Entry(CurrSerial).State = EntityState.Added;
                    //multimediaEntities.EquipmentInStock.Add(CurrSerial);
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
                multimediaEntities.Entry(CurrSerial).State = EntityState.Detached;
        }
    }
}
