using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Linq;
using MultimediaMgmt.Model;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MultimediaMgmt.ViewModel.PopWindows
{
    [POCOViewModel]
    public class EquipmentRepairLogAddEditViewModel : BaseViewModel
    {
        public virtual EquipmentRepairLog CurrRepairLog { get; set; }
        public virtual string WindowTitle { get; set; }
        public virtual string ButtonContent { get; set; }

        [Required(ErrorMessage = "设备编码不能为空")]
        public virtual string SerialNumber { get; set; }

        [Required(ErrorMessage = "申报日期不能为空")]
        public virtual DateTime DeclarationDate { get; set; }

        public Action CloseWindow;
        public Action<string> MessageShow;

        private int currId = 0;
        public EquipmentRepairLogAddEditViewModel(int id, string serialNum)
        {
            currId = id;
            if (id > 0)
            {
                CurrRepairLog = multimediaEntities.EquipmentRepairLog.FirstOrDefault(s => s.ID == id);
                WindowTitle = "维修记录编辑";
                ButtonContent = "更新";
            }
            else
            {
                WindowTitle = "维修记录新增";
                ButtonContent = "增加";
            }
            if (CurrRepairLog == null)
                CurrRepairLog = new EquipmentRepairLog()
                {
                    DeclarationDate = DateTime.Now.Date,
                    SerialNumber = serialNum
                };
            SerialNumber = CurrRepairLog.SerialNumber;
            DeclarationDate = CurrRepairLog.DeclarationDate;
        }

        [Command]
        public void Confirm()
        {
            if (string.IsNullOrEmpty(SerialNumber) ||
                DeclarationDate == default(DateTime))
            {
                MessageShow("请确认必填项");
                return;
            }
            try
            {
                CurrRepairLog.SerialNumber = SerialNumber;
                CurrRepairLog.DeclarationDate = DeclarationDate;
                if (currId > 0)
                {
                    multimediaEntities.Entry(CurrRepairLog).State = EntityState.Modified;
                    multimediaEntities.SaveChanges();
                }
                else
                {
                    multimediaEntities.Entry(CurrRepairLog).State = EntityState.Added;
                    //multimediaEntities.EquipmentRepairLog.Add(CurrRepairLog);
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
                multimediaEntities.Entry(CurrRepairLog).State = EntityState.Detached;
        }
    }
}
