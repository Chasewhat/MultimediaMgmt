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
    public class EquipmentInStockViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<EquipmentInStock> EquipmentInStocks { get; set; }
        public virtual EquipmentInStock SelectedEquipmentInStock { get; set; }
        protected void OnSelectedEquipmentInStockChanged()
        {
            StatusInquiry();
        }
        public virtual string SerialNumber { get; set; }
        public virtual DateTime? InBegin { get; set; }
        public virtual DateTime? InEnd { get; set; }
        public virtual DateTime? UseBegin { get; set; }
        public virtual DateTime? UseEnd { get; set; }
        public virtual string Name { get; set; }
        public virtual List<KeyValuePair<string, string>> EquipmentTypes { get; set; }

        public EquipmentInStockViewModel()
        {
            EquipmentTypes = multimediaEntities.EquipmentType.Select(s => new
            {
                Key = s.EquipmentName,
                Value = s.EquipmentCategory
            }).AsEnumerable().Select(s =>
                            new KeyValuePair<string, string>(
                                s.Key,
                                s.Key)).ToList();
        }

        [Command]
        public void Query()
        {
            var data = from b in multimediaEntities.EquipmentInStock
                       select b;
            if (!string.IsNullOrEmpty(SerialNumber))
                data = data.Where(s => s.SerialNumber == SerialNumber);
            if (InBegin.HasValue && InBegin.Value != default(DateTime))
                data = data.Where(s => s.Intime >= InBegin);
            if (InEnd.HasValue && InEnd.Value != default(DateTime))
                data = data.Where(s => s.Intime <= InEnd);

            if (UseBegin.HasValue && UseBegin.Value != default(DateTime))
                data = data.Where(s => s.UseDate >= UseBegin);
            if (UseEnd.HasValue && UseEnd.Value != default(DateTime))
                data = data.Where(s => s.UseDate <= UseEnd);
            if (!string.IsNullOrEmpty(Name))
                data = data.Where(s => s.Name == Name);
            EquipmentInStocks = data.ToSmartObservableCollection();
        }

        [Command]
        public void Reset()
        {
            SerialNumber = Name = null;
            InBegin = InEnd = UseBegin = UseEnd = null;
        }

        //[Command]
        public void StatusInquiry()
        {
            if (SelectedEquipmentInStock != null)
                //发布设备状态查询事件
                NOTICE.GetEvent<EquipmentStatusInquiryEvent>().Publish(SelectedEquipmentInStock.SerialNumber);
        }
    }
}
