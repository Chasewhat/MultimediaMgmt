using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaMgmt.Model.Models
{
    public class EquipmentStatusEx
    {
        public int ID { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string SaleCompany { get; set; }
        public string Type { get; set; }
        public string Configuration { get; set; }
        public Nullable<System.DateTime> ProduceDate { get; set; }
        public string UserDepartment { get; set; }
        public string Place { get; set; }
        public string Keeper { get; set; }
        public Nullable<double> Price { get; set; }
        public string IncreaseType { get; set; }
        public Nullable<double> OriginalPrice { get; set; }
        public System.DateTime Intime { get; set; }
        public Nullable<System.DateTime> UseDate { get; set; }
        public string Remarks { get; set; }
        public float UsageYears { get; set; }
    }
}
