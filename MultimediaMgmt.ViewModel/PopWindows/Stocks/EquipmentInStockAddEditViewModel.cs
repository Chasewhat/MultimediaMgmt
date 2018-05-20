using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Linq;
using MultimediaMgmt.Model;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace MultimediaMgmt.ViewModel.PopWindows
{
    [POCOViewModel]
    public class EquipmentInStockAddEditViewModel : BaseViewModel
    {
        public virtual EquipmentInStock CurrInStock { get; set; }
        public virtual string WindowTitle { get; set; }
        public virtual string ButtonContent { get; set; }

        [Required(ErrorMessage = "设备编码不能为空")]
        public virtual string SerialNumber { get; set; }
        [Required(ErrorMessage = "名称不能为空")]
        public virtual string Name { get; set; }
        [Required(ErrorMessage = "入库时间不能为空")]
        public virtual DateTime Intime { get; set; }

        public virtual List<KeyValuePair<string, string>> EquipmentTypes { get; set; }

        public Action CloseWindow;
        public Action<string> MessageShow;

        private int currId = 0;
        public EquipmentInStockAddEditViewModel(int id)
        {
            currId = id;
            EquipmentTypes = multimediaEntities.EquipmentType.Select(s => new
            {
                Key = s.EquipmentName,
                Value = s.EquipmentCategory
            }).AsEnumerable().Select(s =>
                            new KeyValuePair<string, string>(
                                s.Key,
                                s.Key)).ToList();
            if (id > 0)
            {
                CurrInStock = multimediaEntities.EquipmentInStock.FirstOrDefault(s => s.ID == id);
                WindowTitle = "入库记录编辑";
                ButtonContent = "更新";
            }
            else
            {
                WindowTitle = "入库记录新增";
                ButtonContent = "增加";
            }
            if (CurrInStock == null)
                CurrInStock = new EquipmentInStock() { Intime = DateTime.Now };
            SerialNumber = CurrInStock.SerialNumber;
            Name = CurrInStock.Name;
            Intime = CurrInStock.Intime;
        }

        [Command]
        public void Confirm()
        {
            if (string.IsNullOrEmpty(SerialNumber) ||
                string.IsNullOrEmpty(Name) ||
                Intime == default(DateTime))
            {
                MessageShow("请确认必填项");
                return;
            }
            try
            {
                CurrInStock.SerialNumber = SerialNumber;
                CurrInStock.Name = Name;
                CurrInStock.Intime = Intime;
                if (currId > 0)
                {
                    multimediaEntities.Entry(CurrInStock).State = EntityState.Modified;
                    multimediaEntities.SaveChanges();
                }
                else
                {
                    multimediaEntities.Entry(CurrInStock).State = EntityState.Added;
                    //multimediaEntities.EquipmentInStock.Add(CurrInStock);
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
                CurrInStock = new EquipmentInStock() { Intime = DateTime.Now };
        }
    }
}
