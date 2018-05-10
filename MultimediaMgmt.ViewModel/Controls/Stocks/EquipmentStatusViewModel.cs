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
    public class EquipmentStatusViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<EquipmentStatusEx> EquipmentStatuss { get; set; }
        public virtual EquipmentStatusEx SelectedEquipmentStatus { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual DateTime? InBegin { get; set; }
        public virtual DateTime? InEnd { get; set; }
        public virtual DateTime? UseBegin { get; set; }
        public virtual DateTime? UseEnd { get; set; }

        public EquipmentStatusViewModel()
        {
        }

        [Command]
        public void Query()
        {
            var data = (from s in multimediaEntities.EquipmentInStock
                        join b in multimediaEntities.EquipmentScrapLog on s.SerialNumber equals b.SerialNumber into joins
                        from j in joins.DefaultIfEmpty()
                        where j == null
                        select s).AsEnumerable().Select(s =>
                        new EquipmentStatusEx()
                        {
                            ID = s.ID,
                            SerialNumber = s.SerialNumber,
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
                            Remarks = s.Remarks,
                            UsageYears = (s.UseDate.HasValue ? (float)Math.Round((double)DateTime.Now.Date.Subtract(s.UseDate.Value).Days / 365, 1) : 0)
                        });
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

            EquipmentStatuss = data.ToSmartObservableCollection();
        }
    }
}
